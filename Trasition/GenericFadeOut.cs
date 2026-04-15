using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

namespace MeDream.UI.transition
{
    public class GenericFadeOut : UITransition
    {
        [Title("Reference")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Title("Settings")]
        [SerializeField] private float fadeDuration = 0.2f;

        public override async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            if (canvasGroup.alpha == 0)
                canvasGroup.alpha = 1;
            if (canvasGroup.interactable == false)
                canvasGroup.interactable = true;

            if (canvasGroup.blocksRaycasts == false)
                canvasGroup.blocksRaycasts = true;
            try
            {
                await canvasGroup.DOFade(0, fadeDuration).ToUniTask(cancellationToken: cancellationToken);
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log($"Fade out operation was canceled.");
            }
            finally
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            await UniTask.CompletedTask;
        }
    }
}
