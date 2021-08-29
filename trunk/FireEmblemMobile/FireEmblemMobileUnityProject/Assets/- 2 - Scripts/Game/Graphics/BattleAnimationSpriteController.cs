using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleAnimationSpriteController : MonoBehaviour
{
    public PlayableDirector PlayableDirector;

    public TimelineAsset walkIn;

    public TimelineAsset attack;
    public TimelineAsset idle;
    public TimelineAsset dodge;
    public TimelineAsset damaged;
    public TimelineAsset death;
    // Start is called before the first frame update
    
    public void WalkIn()
    {
        PlayableDirector.Play(walkIn);
    }
    public void Idle()
    {
        PlayableDirector.Play(idle);
    }
    public void Attack()
    {
        PlayableDirector.Play(attack);
    }
    public void Death()
    {
        PlayableDirector.Play(death);
    }


    public void Dodge()
    {
        PlayableDirector.Play(dodge);
    }

    public double GetAttackDuration()
    {
        return PlayableDirector.duration;
    }

    public void Damaged()
    {
        PlayableDirector.Play(damaged);
    }
}
