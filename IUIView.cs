using Cysharp.Threading.Tasks;

namespace MeDream.UI
{
    public interface IUIView
    {
        bool IsVisible { get; }
        void Show();
        void Hide();
        UniTask ShowAsync();
        UniTask HideAsync();
    }
}