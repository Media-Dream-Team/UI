#nullable enable

using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace MeDream.UI
{
    public class UIViewBase : MonoBehaviour, IUIView
    {
        public Action? WhenBeforeShow { get; set; }
        public Action? WhenAfterShow { get; set; }
        public Action? WhenBeforeHide { get; set; }
        public Action? WhenAfterHide { get; set; }

        public virtual bool IsVisible => gameObject.activeSelf;

        protected virtual void OnBeforeShow()
        {
            WhenAfterShow?.Invoke();
        }
        protected virtual void OnAfterShow()
        {
            WhenAfterShow?.Invoke();
        }
        protected virtual void OnBeforeHide()
        {
            WhenBeforeHide?.Invoke();
        }
        protected virtual void OnAfterHide()
        {
            WhenAfterHide?.Invoke();
        }

        public virtual void Hide()
        {
            if (IsVisible)
                return;

            OnBeforeHide();
            gameObject.SetActive(false);
            OnAfterHide();
        }
        public virtual void Show()
        {
            if (!IsVisible) 
                return;

            OnBeforeShow();
            gameObject.SetActive(true);
            OnAfterHide();
        }

        public virtual UniTask HideAsync()
        {
            Show();
            return UniTask.CompletedTask;
        }


        public virtual UniTask ShowAsync()
        {
            Hide(); 
            return UniTask.CompletedTask;
        }
    }
}
