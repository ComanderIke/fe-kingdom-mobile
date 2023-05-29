using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using LostGrace;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

#if UNITY_EDITOR
[CustomEditor(typeof(BattleAnimationSpriteController))]
public class BattleAnimationSpriteControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields
 
        BattleAnimationSpriteController script = (BattleAnimationSpriteController)target;
 
        // draw checkbox for the bool
        script.hasPrepare = EditorGUILayout.Toggle("Has Prepare Animation: ", script.hasPrepare);
        if (script.hasPrepare) // if bool is true, show other fields
        {
            script.prepareAttack = EditorGUILayout.ObjectField("Prepare Animation", script.prepareAttack, typeof(TimelineAsset), true) as TimelineAsset;
        }
    }
}
#endif
public class BattleAnimationSpriteController : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    [SerializeField]
    RectTransform attractorTransform;
    [SerializeField]
    ExpBarController expBar;

    [SerializeField] private HitFeedbackController hitFeedbackController;

   
    
    public TimelineAsset walkIn;
    public TimelineAsset attack;
    public TimelineAsset idle;
    public TimelineAsset dodge;
    public TimelineAsset damaged;
    public TimelineAsset death;
    [HideInInspector]
    public bool hasPrepare = false;
    [HideInInspector]
    public TimelineAsset prepareAttack;

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

    
    public void Damaged(float playSpeed,DamagedState damagedState)
    {
        Debug.Log("TODO HIT FEEDBACK CONTROLLER");
        //hitFeedbackController.SetState(damagedState);
        PlayAtSpeed(damaged, playSpeed);
    }
    public double GetCurrentAnimationDuration()
    {
        return PlayableDirector.duration;
    }

    void PlayAtSpeed(TimelineAsset clip, float speed)
    {
        PlayableDirector.playableAsset = clip;
        PlayableDirector.RebuildGraph(); // the graph must be created before getting the playable graph
        PlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        PlayableDirector.Play();
    }
    public bool HasPrepareAnimation()
    {
        return hasPrepare;
    }

    public void Prepare(float playSpeed)
    {
        PlayAtSpeed(prepareAttack, playSpeed);
    }

    public RectTransform GetAttractorTransform()
    {
        return attractorTransform;
    }

    public ExpBarController GetExpRenderer()
    {
        return expBar;
    }
}
public enum DamagedState
{
    NoDamage,
    Damage,
    HighDmg
}