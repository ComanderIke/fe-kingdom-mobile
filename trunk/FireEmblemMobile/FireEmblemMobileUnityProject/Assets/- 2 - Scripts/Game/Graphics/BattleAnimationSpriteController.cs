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
    public TimelineAsset attackMirrored;
    public TimelineAsset dodge;
    public TimelineAsset dodgeMirrored;
    // Start is called before the first frame update
    
    public void WalkIn()
    {
        PlayableDirector.Play(walkIn);
    }
    public void Attack(bool mirror=false)
    {
        if(mirror)
            PlayableDirector.Play(attackMirrored);
        else
            PlayableDirector.Play(attack);
    }

    public void Dodge(bool mirror=false)
    {
        if(mirror)
            PlayableDirector.Play(dodgeMirrored);
        else
            PlayableDirector.Play(dodge);
    }
}
