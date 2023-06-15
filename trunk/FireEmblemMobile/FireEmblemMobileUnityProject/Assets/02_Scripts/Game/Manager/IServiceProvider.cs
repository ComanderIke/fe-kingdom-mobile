using System.Collections;
using UnityEngine;

namespace Game.Manager
{
    public interface IServiceProvider
    {
        T GetSystem<T>();
        Coroutine StartChildCoroutine(IEnumerator coroutine);
        void StopChildCoroutine(Coroutine coroutine);

        void CleanUp();
    }
}