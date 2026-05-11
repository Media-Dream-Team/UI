using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MeDream.UI.transition
{
    public class UIFadeInTransition : UITransition
    {
        [Title("Reference")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Title("Settings")]
        [SerializeField] private float fadeDuration = 0.2f;

        public override async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            //await UniTask.CompletedTask;
            if (canvasGroup.alpha == 1)
                canvasGroup.alpha = 0;

            if (canvasGroup.interactable == true)
                canvasGroup.interactable = false;

            if (canvasGroup.blocksRaycasts == true)
                canvasGroup.blocksRaycasts = false;

            try
            {
                await canvasGroup.DOFade(1, fadeDuration).ToUniTask(cancellationToken: cancellationToken);
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log($"Fade in operation was canceled.");
            }
            finally
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            await UniTask.CompletedTask;
        }
    }
}
