using System;
using System.Collections;
using UnityEngine;

namespace Game.Utility
{
    public static class MonoBehaviourExtensions
    {
        public static void CallWithDelay(this MonoBehaviour mono, Action action,float delay)
        {
            mono.StartCoroutine(CallWithDelayCoroutine(action, delay));
        }

        static IEnumerator CallWithDelayCoroutine(Action action,float delay)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}
