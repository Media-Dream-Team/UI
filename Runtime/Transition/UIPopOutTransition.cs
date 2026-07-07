using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

namespace MeDream.UI.transition
{
    public class UIPopOutTransition : UITransition
    {
        [Title("Reference")]
        [SerializeField] private RectTransform target;

        [Title("Settings")]
        [SerializeField] private Vector3 startScale = Vector3.one;
        [SerializeField] private Vector3 endScale = Vector3.zero;
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private Ease ease = Ease.InBack;

        [Title("Fade")]
        [SerializeField] private bool enableFade = false;
        [SerializeField, ShowIf("enableFade")] private CanvasGroup canvasGroup;
        [SerializeField, ShowIf("enableFade")] private float startAlpha = 1f;
        [SerializeField, ShowIf("enableFade")] private float endAlpha = 0f;

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
                Debug.Log("Pop out operation was canceled.");
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
