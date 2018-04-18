﻿using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteController : MonoBehaviour {

    public Animator maskBlinkAnimator;
    public Animator spriteAnimator;
    public CameraShaker cameraShaker;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ShakeAnimation(float roughness) {
        cameraShaker.ShakeOnce(5f,roughness, .1f, 1f);
    }

    public void StartBlinkAnimation()
    {
        maskBlinkAnimator.SetTrigger("Play");
    }
    public void StartAttackAnimation()
    {
        spriteAnimator.SetTrigger("Attack");
    }
}
