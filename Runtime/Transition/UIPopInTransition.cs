using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

namespace MeDream.UI.transition
{
    public class UIPopInTransition : UITransition
    {
        [Title("Reference")]
        [SerializeField] private RectTransform target;

        [Title("Settings")]
        [SerializeField] private Vector3 startScale = Vector3.zero;
        [SerializeField] private Vector3 endScale = Vector3.one;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private Ease ease = Ease.OutBack;

        [Title("Fade")]
        [SerializeField] private bool enableFade = false;
        [SerializeField, ShowIf("enableFade")] private CanvasGroup canvasGroup;
        [SerializeField, ShowIf("enableFade")] private float startAlpha = 0f;
        [SerializeField, ShowIf("enableFade")] private float endAlpha = 1f;

        public override void SetupTransition()
        {
            if (target == null) return;
            target.localScale = startScale;

            if (enableFade && canvasGroup != null)
                canvasGroup.alpha = startAlpha;
        }

        public override void ResetTransition()
        {
            if (target == null) return;
            target.localScale = startScale;

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

            target.localScale = startScale;

            Tween fadeTween = null;
            if (enableFade && canvasGroup != null)
            {
                canvasGroup.alpha = startAlpha;
                fadeTween = canvasGroup.DOFade(endAlpha, duration).SetEase(ease);
            }

            try
            {
                await target.DOScale(endScale, duration)
                    .SetEase(ease)
                    .ToUniTask(cancellationToken: cancellationToken);
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log("Pop in operation was canceled.");
            }
            finally
            {
                target.localScale = endScale;

                fadeTween?.Kill();
                if (enableFade && canvasGroup != null)
                    canvasGroup.alpha = endAlpha;
            }

            await UniTask.CompletedTask;
        }
    }
}
