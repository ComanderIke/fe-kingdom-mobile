using System.Collections;

namespace Game.Manager
{
    public interface IServiceProvider
    {
        T GetSystem<T>();
        void StartChildCoroutine(IEnumerator coroutine);
    }
}