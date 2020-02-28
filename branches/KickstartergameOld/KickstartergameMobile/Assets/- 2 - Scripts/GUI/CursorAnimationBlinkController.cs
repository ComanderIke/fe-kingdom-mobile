using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAnimationBlinkController : MonoBehaviour {
    Animator animator;
  // Use this for initialization
  CursorAnimationBlinkController[] controllers;
    AnimationTimer timer;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled=false;
        timer = FindObjectOfType<AnimationTimer>();
        animator.SetFloat("NormTime", timer.normalizedTime);

        animator.enabled = true;
    }
    void Update()
    {
        animator.SetFloat("NormTime", timer.normalizedTime);

    }
}
