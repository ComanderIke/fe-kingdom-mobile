﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
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
            Debug.Log("Queue Count: "+queue.Count+" "+action);
            if (queue.Count == 1)
            {
                NextAnimation();
            }
        }
        public static void AnimationEnded()
        {
            if (queue.Count != 0)
            {
                queue.Peek().runAfterAction?.Invoke();
                queue.Dequeue();
                NextAnimation();
            }
        }
        public static void NextAnimation()
        {
            if (queue.Count > 0)
                queue.Peek().action?.Invoke();
            else
            {
                Debug.Log("All Anims Ended");
                OnAllAnimationsEnded?.Invoke();
            }
                
        }
   
    }
}