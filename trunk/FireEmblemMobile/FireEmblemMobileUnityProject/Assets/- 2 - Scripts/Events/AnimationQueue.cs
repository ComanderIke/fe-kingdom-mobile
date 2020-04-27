using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
struct ActionContainer
{
    public Action action;
    public Action runAfterAction;

    public ActionContainer(Action action, Action runAfterAction) : this()
    {
        this.action = action;
        this.runAfterAction = runAfterAction;
    }
}
public class AnimationQueue :MonoBehaviour
{
    public static Action OnAnimationEnded;
    public static event Action OnAllAnimationsEnded;
    static Queue<ActionContainer> queue = new Queue<ActionContainer>();
    void OnEnable()
    {
        OnAnimationEnded += AnimationEnded;
    }
    private void OnDisable()
    {
        OnAnimationEnded -= AnimationEnded;
    }

    public static void Add(Action action, Action runAfterAction = null)
    {
        queue.Enqueue(new ActionContainer(action,runAfterAction));
        if (queue.Count == 1)
        {
            NextAnimation();
        }
    }
    public static void AnimationEnded()
    {
        queue.Peek().runAfterAction?.Invoke();
        queue.Dequeue();
        NextAnimation();
    }
    public static void NextAnimation()
    {
        if (queue.Count > 0)
            queue.Peek().action?.Invoke();
        else
            OnAllAnimationsEnded?.Invoke();
    }
   
}
