using System;
using System.Collections;
using UnityEngine;

public class MonoUtility : MonoBehaviour
{
    public static MonoUtility Instance;

    private void Awake()
    {
        if(Instance==null)
            Instance = this;
        else
        {
            Destroy(this);
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
}