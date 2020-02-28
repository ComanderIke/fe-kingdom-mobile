using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySpriteController : MonoBehaviour {

    public Animator maskBlinkAnimator;
    public Animator spriteAnimator;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
