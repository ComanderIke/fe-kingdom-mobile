using System;
using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using LostGrace;
using MoreMountains.Feedbacks;
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
    public TimelineAsset critical;
    public TimelineAsset death;
    [SerializeField] private MMF_Player criticalFeedbacks;
    [SerializeField] private MMF_Player attackFeedbacks;
    [SerializeField] private MMF_Player deathFeedbacks;
    [SerializeField] private MMF_Player dodgeFeedbacks;
    [SerializeField] private MMF_Player prepareFeedbacks;
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
       // PlayAtSpeed(attack, playSpeed);
       attackFeedbacks.PlayFeedbacks();
      
    }
    public void Critical(float playSpeed)
    {
        //PlayAtSpeed(critical, playSpeed);
        criticalFeedbacks.PlayFeedbacks();
    }
    public void Death(float playSpeed)
    {
       // PlayAtSpeed(death, playSpeed);
       deathFeedbacks.PlayFeedbacks();
    }
    public void Dodge(float playSpeed)
    {
        //PlayAtSpeed(dodge, playSpeed);
        dodgeFeedbacks.PlayFeedbacks();
    }

    
    public void Damaged(float playSpeed,DamagedState damagedState)
    {
        Debug.Log("TODO HIT FEEDBACK CONTROLLER");
        hitFeedbackController.SetState(damagedState);
        hitFeedbackController.PlayHitFeedback();
       // PlayAtSpeed(damaged, playSpeed);
    }

   
    public float GetAttackAnimationDuration()
    {
        return attackFeedbacks.TotalDuration;
    }
    public float GetPrepareAnimationDuration()
    {
        return prepareFeedbacks.TotalDuration;
    }
    public float GetCriticalAttackAnimationDuration()
    {
        return criticalFeedbacks.TotalDuration;
    }
    // public double GetCurrentAnimationDuration()
    // {
    //     return PlayableDirector.duration;
    // }

    void PlayAtSpeed(TimelineAsset clip, float speed)
    {
        PlayableDirector.playableAsset = clip;
        PlayableDirector.RebuildGraph(); // the graph must be created before getting the playable graph
        PlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        PlayableDirector.Play();
    }
    void PlayFeedback(MMF_Player clip)
    {
        clip.PlayFeedbacks();
        StartCoroutine(DelayAction(()=>Debug.Log("TEST"),clip.TotalDuration));
    }

    IEnumerator DelayAction(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
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