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

        public override void SetupTransition()
        {
            if (target == null) return;
            var pos = target.anchoredPosition;
            pos.y = startY;
            target.anchoredPosition = pos;
        }

        public override void ResetTransition()
        {
            if (target == null) return;
            var pos = target.anchoredPosition;
            pos.y = startY;
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
            pos.y = startY;
            target.anchoredPosition = pos;

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
            }

            await UniTask.CompletedTask;
        }
    }
}
