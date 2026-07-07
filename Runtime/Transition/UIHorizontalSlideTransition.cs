using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

namespace MeDream.UI.transition
{
    public class UIHorizontalSlideTransition : UITransition
    {
        [Title("Reference")]
        [SerializeField] private RectTransform target;

        [Title("Settings")]
        [SerializeField] private float startX = -1000f;
        [SerializeField] private float endX = 0f;
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
            pos.x = startX;
            target.anchoredPosition = pos;

            if (enableFade && canvasGroup != null)
                canvasGroup.alpha = startAlpha;
        }

        public override void ResetTransition()
        {
            if (target == null) return;
            var pos = target.anchoredPosition;
            pos.x = startX;
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
            pos.x = startX;
            target.anchoredPosition = pos;

            Tween fadeTween = null;
            if (enableFade && canvasGroup != null)
            {
                canvasGroup.alpha = startAlpha;
                fadeTween = canvasGroup.DOFade(endAlpha, duration).SetEase(ease);
            }

            try
            {
                await target.DOAnchorPosX(endX, duration)
                    .SetEase(ease)
                    .ToUniTask(cancellationToken: cancellationToken);
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log("Horizontal slide operation was canceled.");
            }
            finally
            {
                var finalPos = target.anchoredPosition;
                finalPos.x = endX;
                target.anchoredPosition = finalPos;

                fadeTween?.Kill();
                if (enableFade && canvasGroup != null)
                    canvasGroup.alpha = endAlpha;
            }

            await UniTask.CompletedTask;
        }
    }
}
