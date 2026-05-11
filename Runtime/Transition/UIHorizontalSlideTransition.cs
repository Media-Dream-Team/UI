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

        public override void SetupTransition()
        {
            if (target == null) return;
            var pos = target.anchoredPosition;
            pos.x = startX;
            target.anchoredPosition = pos;
        }

        public override void ResetTransition()
        {
            if (target == null) return;
            var pos = target.anchoredPosition;
            pos.x = startX;
            target.anchoredPosition = pos;
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
            }

            await UniTask.CompletedTask;
        }
    }
}
