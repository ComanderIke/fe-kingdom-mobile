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
    
    public void WalkIn(float playSpeed)
    {
        PlayAtSpeed(walkIn, playSpeed);
    }
    public void Idle(float playSpeed)
    {
        PlayAtSpeed(idle, playSpeed);
    }
    public void Attack(float playSpeed)
    {
        PlayAtSpeed(attack, playSpeed);
    }
    public void Death(float playSpeed)
    {
        PlayAtSpeed(death, playSpeed);
    }


    public void Dodge(float playSpeed)
    {
        PlayAtSpeed(dodge, playSpeed);
    }

    public double GetAttackDuration()
    {
        return PlayableDirector.duration;
    }

    public void Damaged(float playSpeed)
    {
        PlayAtSpeed(damaged, playSpeed);
    }
    void PlayAtSpeed(TimelineAsset clip, float speed)
    {
        PlayableDirector.playableAsset = clip;
        PlayableDirector.RebuildGraph(); // the graph must be created before getting the playable graph
        PlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        PlayableDirector.Play();
    }

}
