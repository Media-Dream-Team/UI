using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace MeDream.UI
{
    public abstract class UITransition : MonoBehaviour
    {
        public abstract UniTask PlayAsync(CancellationToken cancellationToken);

        public virtual void ResetTransition() { }

        public virtual void SetupTransition() { }
    }
}
