using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class BattleAnimationController : MonoBehaviour
{
    public Animator character;
    public string AnimationHash;
    
    public bool trigger=true;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(trigger)
            character.SetTrigger(AnimationHash);
        else
            character.SetBool(AnimationHash, true);
    }
    void OnDisable(){
        if(!trigger)
            character.SetBool(AnimationHash, false);
    }
    
}
