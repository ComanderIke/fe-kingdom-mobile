using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class MonoUtility : MonoBehaviour
{
    public static MonoUtility Instance;
    private static Dictionary<Object, List<Coroutine>> coroutines;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            coroutines = new Dictionary<Object, List<Coroutine>>();
        }
        else
        {
            Destroy(this);
        }
        
    }
    public static void DelayFunction(Object source,Action action, float delay)
    {
        if (coroutines.ContainsKey(source))
        {
            if (coroutines[source] == null)
                coroutines[source] = new List<Coroutine>();
            coroutines[source].Add(Instance.StartCoroutine(Instance.Delay(action, delay)));
        }
        else
        {
            coroutines.Add(source, new List<Coroutine>());
            coroutines[source].Add(Instance.StartCoroutine(Instance.Delay(action, delay)));
        }
    }
    public static void DelayFunction(Action action, float delay)
    {
        Instance.StartCoroutine(Instance.Delay(action, delay));
    }

    IEnumerator Delay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
        
    }

    public static void StopCoroutines(Object source)
    {
        if (coroutines.ContainsKey(source))
        {
            if (coroutines[source] != null)
            {
                foreach (var coroutine in coroutines[source])
                {
                    Instance.StopCoroutine(coroutine);
                }
            }
        }
    }
}