using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAnimationBlinkController : MonoBehaviour {
    float totalFrames = 60;
    float frameNumber = 0;
    Animator animator;
  // Use this for initialization
  CursorAnimationBlinkController[] controllers;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled=false;
        animator.SetFloat("NormTime", FindObjectOfType<AnimationTimer>().normalizedTime);

        animator.enabled = true;
    }
    void Update()
    {
        animator.SetFloat("NormTime", FindObjectOfType<AnimationTimer>().normalizedTime);

    }
}
