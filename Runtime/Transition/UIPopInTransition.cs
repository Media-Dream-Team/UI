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

        public override void SetupTransition()
        {
            if (target == null) return;
            target.localScale = startScale;
        }

        public override void ResetTransition()
        {
            if (target == null) return;
            target.localScale = startScale;
        }

        public override async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            if (target == null)
            {
                await UniTask.CompletedTask;
                return;
            }

            target.localScale = startScale;

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
            }

            await UniTask.CompletedTask;
        }
    }
}
