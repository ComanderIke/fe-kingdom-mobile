using System;
using System.Collections;
using Game.GUI.Controller;
using MoreMountains.Feedbacks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game.Graphics.BattleAnimations
{
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

        [SerializeField] private Transform impactPosition;
    
        // public TimelineAsset walkIn;
        // public TimelineAsset attack;
        // public TimelineAsset idle;
        // public TimelineAsset dodge;
        // public TimelineAsset damaged;
        // public TimelineAsset critical;
        // public TimelineAsset death;
        [SerializeField] private MMF_Player walkInFeedbacks;
        [SerializeField] private MMF_Player idleFeedbacks;
        [SerializeField] private MMF_Player criticalFeedbacks;
        [SerializeField] private MMF_Player attackFeedbacks;
        [SerializeField] private MMF_Player deathFeedbacks;
        [SerializeField] private MMF_Player dodgeFeedbacks;
        [SerializeField] private MMF_Player prepareFeedbacks;
        [SerializeField] private int sortOrderNormal = 1;
        [SerializeField] private int sortOrderAttack = 2;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [HideInInspector]
        public bool hasPrepare = false;
        [HideInInspector]
        public TimelineAsset prepareAttack;

        public void WalkIn(float playSpeed)
        {
            walkInFeedbacks.PlayFeedbacks();
            spriteRenderer.sortingOrder = sortOrderNormal;
            //PlayAtSpeed(walkIn, playSpeed);
        }
        public void Idle(float playSpeed)
        {
            idleFeedbacks.PlayFeedbacks();
            spriteRenderer.sortingOrder = sortOrderNormal;
            // PlayAtSpeed(idle, playSpeed);
        }

   
        public void Attack(float playSpeed)
        {
            // PlayAtSpeed(attack, playSpeed);
            attackFeedbacks.PlayFeedbacks();
            spriteRenderer.sortingOrder = sortOrderAttack;
      
        }
        public void Critical(float playSpeed)
        {
            //PlayAtSpeed(critical, playSpeed);
            criticalFeedbacks.PlayFeedbacks();
            spriteRenderer.sortingOrder = sortOrderAttack;
        }
        public void Death(float playSpeed)
        {
            // PlayAtSpeed(death, playSpeed);
       
            deathFeedbacks.PlayFeedbacks();
            spriteRenderer.sortingOrder = sortOrderNormal;
        }
        public void Dodge(float playSpeed)
        {
            //PlayAtSpeed(dodge, playSpeed);
            dodgeFeedbacks.PlayFeedbacks();
            spriteRenderer.sortingOrder = sortOrderNormal;
        }

    
        public void Damaged(float playSpeed,DamagedState damagedState)
        {
       
            hitFeedbackController.SetState(damagedState);
            hitFeedbackController.PlayHitFeedback();
            spriteRenderer.sortingOrder = sortOrderNormal;
            // PlayAtSpeed(damaged, playSpeed);
        }
        public float GetIdleAnimationDuration()
        {
            return idleFeedbacks.TotalDuration;
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
            prepareFeedbacks.PlayFeedbacks();
            //PlayAtSpeed(prepareAttack, playSpeed);
            spriteRenderer.sortingOrder = sortOrderNormal;
        }

        public RectTransform GetAttractorTransform()
        {
            return attractorTransform;
        }

        public ExpBarController GetExpRenderer()
        {
            return expBar;
        }

        public Vector3 GetImpactPosition()
        {
            return impactPosition.position;
        }

        public float GetDeathFeedbackDuration()
        {
            return deathFeedbacks.TotalDuration;
        }
    }
    public enum DamagedState
    {
        NoDamage,
        Damage,
        HighDmg
    }
}