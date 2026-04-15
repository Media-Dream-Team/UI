#nullable enable

using Cysharp.Threading.Tasks;
using MeDream.UI.transition;
using Sirenix.OdinInspector;
using System;
using System.Threading;
using UnityEngine;

namespace MeDream.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UITransitionView : UIViewBase, IUIView
    {
        [FoldoutGroup("Transition"), OnValueChanged("ValidateTransitions")]
        [SerializeField] private UITransition? _transitionIn;

        [OnValueChanged("ValidateTransitions"), FoldoutGroup("Transition")]
        [InfoBox("Transition In and Transition Out should not be the same instance.", VisibleIf = "AreTransitionsInvalid", InfoMessageType = InfoMessageType.Error)]
        [SerializeField] private UITransition? _transitionOut;

        [FoldoutGroup("Transition"), SerializeField, ReadOnly] private bool _isTransitioning;
        public bool IsTransitioning => _isTransitioning;
        public override bool IsVisible
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = gameObject.GetComponent<CanvasGroup>();
                return gameObject.activeSelf && _canvasGroup.alpha > 0;
            }
        }

        [SerializeField, FoldoutGroup("Transition")] bool _canInterruptTransition = true;

        protected CancellationTokenSource? _transitionCTS = new();

        private CanvasGroup? _canvasGroup;

        protected virtual void OnEnable()
        {
            if (_transitionIn != null)
                _transitionIn.ResetTransition();
        }

        protected virtual void OnDisable()
        {
            CancelTransitionCTS();

            if (_transitionOut != null)
                _transitionOut.ResetTransition();
        }

        protected virtual void OnDestroy()
        {
            CancelTransitionCTS();
        }

        protected bool AreTransitionsInvalid()
        {
            return _transitionIn != null && _transitionOut == _transitionIn;
        }

        private void ValidateTransitions()
        {
            if (AreTransitionsInvalid())
            {
                Debug.LogError("Error: _transitionIn and _transitionOut should not be the same instance.", this);
            }
        }

        public override void Show()
        {
            OnBeforeShow();

            CancelTransitionCTS();
            _transitionCTS = new CancellationTokenSource();

            if (_canvasGroup != null)
                _canvasGroup.alpha = 1;
            gameObject.SetActive(true);

            OnAfterShow();
        }

        public override void Hide()
        {
            OnBeforeHide();

            CancelTransitionCTS();
            _transitionCTS = new CancellationTokenSource();

            if (gameObject != null)
                gameObject.SetActive(false);

            OnAfterHide();
        }

        public virtual async UniTask SetShowAsync(bool isShow)
        {
            if (isShow)
            {
                await ShowAsync();
            }
            else
            {
                await HideAsync();
            }
        }

        /// <summary>
        /// call Show with default cancellation token
        /// </summary>
        /// <returns></returns>
        public override async UniTask ShowAsync()
        {
            OnBeforeShow();

            if (!_canInterruptTransition && IsVisible)
            {
                return;
            }

            try
            {
                _isTransitioning = true;
                gameObject.SetActive(true);

                CancelTransitionCTS();
                _transitionCTS = new CancellationTokenSource();

                if (_transitionIn != null)
                {
                    await _transitionIn.PlayAsync(_transitionCTS.Token).AttachExternalCancellation(destroyCancellationToken);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }
            finally
            {
                _isTransitioning = false;
                OnAfterShow();
            }
        }

        /// <summary>
        /// call HideAsync with default cancellation token
        /// </summary>
        /// <returns></returns>
        public override async UniTask HideAsync()
        {
            OnBeforeHide();

            if (!_canInterruptTransition && !IsVisible)
            {
                return;
            }

            try
            {
                _isTransitioning = true;
                CancelTransitionCTS();
                _transitionCTS = new CancellationTokenSource();

                CancellationToken token = _transitionCTS.Token;

                if (_transitionOut != null)
                {
                    await _transitionOut.PlayAsync(token).AttachExternalCancellation(destroyCancellationToken);
                }

                if (token.IsCancellationRequested == false)
                {
                    gameObject?.SetActive(false);
                }
            }
            catch (OperationCanceledException)
            {
                // Ignore cancellation exceptions
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            finally
            {
                _isTransitioning = false;
                OnAfterHide();
            }
        }

        public void SetAlpha(float alpha)
        {
            if (_canvasGroup == null)
                _canvasGroup = gameObject.GetComponent<CanvasGroup>();

            _canvasGroup.alpha = alpha;
        }

        public float GetAlpha()
        {
            if (_canvasGroup == null)
                _canvasGroup = gameObject.GetComponent<CanvasGroup>();

            return _canvasGroup.alpha;
        }

        private void CancelTransitionCTS()
        {
            _transitionCTS?.Cancel();
            _transitionCTS?.Dispose();
            _transitionCTS = null;
        }

#if UNITY_EDITOR
        [FoldoutGroup("Transition"), Button("SetUpGenericTransition")]
        public void SetUpGenericTransition()
        {
            SetUpTransitionType<GenericFadeIn, GenericFadeOut>();
        }

        public void SetUpTransitionType<T1, T2>() where T1 : UITransition where T2 : UITransition
        {
            T1 transitionIn = gameObject.GetComponent<T1>() ?? gameObject.AddComponent<T1>();
            _transitionIn = transitionIn;
            T2 transitionOut = gameObject.GetComponent<T2>() ?? gameObject.AddComponent<T2>();
            _transitionOut = transitionOut;

            _transitionIn.SetupTransition();
            _transitionOut.SetupTransition();
        }

        [FoldoutGroup("Transition"), HorizontalGroup("TransitionTest"), Button("ShowAsync"), ShowIf("@UnityEngine.Application.isPlaying")]
        private async UniTask TestShowAsync()
        {
            await ShowAsync();
        }
        [FoldoutGroup("Transition"), HorizontalGroup("TransitionTest"), Button("HideAsync"), ShowIf("@UnityEngine.Application.isPlaying")]
        private async UniTask TestHideAsync()
        {
            await HideAsync();
        }
#endif
    }

    public class UITransitionView<T1, T2> : UITransitionView where T1 : UITransition where T2 : UITransition
    {
#if UNITY_EDITOR
        string ButtonName => $"Setup: ({typeof(T1).Name} - {typeof(T2).Name})";
        [Button("$ButtonName")]
        private void SetUpDefinedTransition()
        {
            SetUpTransitionType<T1, T2>();
        }
#endif
    }
}

