﻿using Assets.Utility;
using System;
using UnityEngine;

namespace Assets.GUI
{
    public class CursorAnimationBlinkController : MonoBehaviour
    {
        private Animator animator;
        private readonly CursorAnimationBlinkController[] controllers;
        private AnimationTimer timer;
        //public float duration = 1;
        public float maxAlpha = 1;
        public float minAlpha = 0;
        private void Start()
        {
            //animator = GetComponent<Animator>();
            //animator.enabled = false;
            //timer = FindObjectOfType<AnimationTimer>();
            //animator.SetFloat("NormTime", timer.NormalizedTime);
            //animator.enabled = true;
            float start = FindObjectOfType<AnimationTimer>().AnimationTimeBlinkSprites;
            float duration = FindObjectOfType<AnimationTimer>().AnimationTimeBlinkSpritesDuration;
            //Debug.Log(start+ " "+transform.position.x + " "+transform.position.y);
           
            if (start >= 0.5)//FadeIn
            {
                float map = start - 0.5f;
                float mapValued = MathUtility.MapValues(map, 0, 0.5f, minAlpha, maxAlpha);
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, mapValued);
                LeanTween.alpha(gameObject, maxAlpha, (duration / 2) - (duration / 2) * ((start - 0.5f) / 0.5f)).setOnComplete(() => LeanTween.alpha(gameObject, minAlpha, duration / 2).setLoopPingPong());
            }
            else//Fade Out
            {
                float map = Math.Abs(start - 0.5f);
                MathUtility.MapValues(start, 0, 0.5f, minAlpha, maxAlpha);
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, map );
                LeanTween.alpha(gameObject, minAlpha, (duration / 2) - (duration / 2) * ((start) / 0.5f)).setOnComplete(() => LeanTween.alpha(gameObject, maxAlpha, duration / 2).setLoopPingPong());
            }
        }


        //private void Update()
        //{
        //    animator.SetFloat("NormTime", timer.NormalizedTime);
        //}
    }
}