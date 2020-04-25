using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AnimationQueue :MonoBehaviour
{
    public delegate void AnimationEndedEvent();
    public static AnimationEndedEvent OnAnimationEnded;
    static Queue<Action> queue = new Queue<Action>();
    void OnEnable()
    {
        OnAnimationEnded += AnimationEnded;
    }
    private void OnDisable()
    {
        OnAnimationEnded -= AnimationEnded;
    }

    public static void Add(Action action)
    {
        queue.Enqueue(action);
        if (queue.Count == 1)
        {
            NextAnimation();
        }
    }
    public static void AnimationEnded()
    {
        queue.Dequeue();
        NextAnimation();
    }
    public static void NextAnimation()
    {
        if(queue.Count>0)
            queue.Peek()?.Invoke();
    }
   
}
