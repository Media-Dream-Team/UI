using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

namespace MeDream.UI.transition
{
    public class UIVerticalSlideTransition : UITransition
    {
        [Title("Reference")]
        [SerializeField] private RectTransform target;

        [Title("Settings")]
        [SerializeField] private float startY = -1000f;
        [SerializeField] private float endY = 0f;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private Ease ease = Ease.OutCubic;

        [Title("Fade")]
        [SerializeField] private bool enableFade = false;
        [SerializeField, ShowIf("enableFade")] private CanvasGroup canvasGroup;
        [SerializeField, ShowIf("enableFade")] private float startAlpha = 0f;
        [SerializeField, ShowIf("enableFade")] private float endAlpha = 1f;

        public override void SetupTransition()
        {
            if (target == null) return;
            var pos = target.anchoredPosition;
            pos.y = startY;
            target.anchoredPosition = pos;

            if (enableFade && canvasGroup != null)
                canvasGroup.alpha = startAlpha;
        }

        public override void ResetTransition()
        {
            if (target == null) return;
            var pos = target.anchoredPosition;
            pos.y = startY;
            target.anchoredPosition = pos;

            if (enableFade && canvasGroup != null)
                canvasGroup.alpha = startAlpha;
        }

        public override async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            if (target == null)
            {
                await UniTask.CompletedTask;
                return;
            }

            var pos = target.anchoredPosition;
            pos.y = startY;
            target.anchoredPosition = pos;

            Tween fadeTween = null;
            if (enableFade && canvasGroup != null)
            {
                canvasGroup.alpha = startAlpha;
                fadeTween = canvasGroup.DOFade(endAlpha, duration).SetEase(ease);
            }

            try
            {
                await target.DOAnchorPosY(endY, duration)
                    .SetEase(ease)
                    .ToUniTask(cancellationToken: cancellationToken);
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log("Vertical slide operation was canceled.");
            }
            finally
            {
                var finalPos = target.anchoredPosition;
                finalPos.y = endY;
                target.anchoredPosition = finalPos;

                fadeTween?.Kill();
                if (enableFade && canvasGroup != null)
                    canvasGroup.alpha = endAlpha;
            }

            await UniTask.CompletedTask;
        }
    }
}
