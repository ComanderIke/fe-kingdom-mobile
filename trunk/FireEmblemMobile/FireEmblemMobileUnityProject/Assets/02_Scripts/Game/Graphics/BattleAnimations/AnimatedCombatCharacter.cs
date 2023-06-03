using System;
using Game.GameActors.Players;
using Game.GameInput;
using Game.GUI;
using LostGrace;
using UnityEngine;

public class AnimatedCombatCharacter
{
    private BattleAnimationSpriteController spriteController;
    private AnimationSpriteSwapper spriteSwapper;
    private ImpactPosition impactPosition;
    public Action OnPrepareFinished;
    public Action OnAttackFinished;
    public GameObject GameObject { get; set; }
    private bool left;
    public IBattleActor Actor { get; set; }

    public AnimatedCombatCharacter(IBattleActor actor,GameObject gameObject, bool left)
    {
        Actor = actor;
        this.GameObject = gameObject;
        this.impactPosition = gameObject.GetComponentInChildren<ImpactPosition>();
        this.spriteController = gameObject.GetComponentInChildren<BattleAnimationSpriteController>();
        
        this.spriteSwapper = gameObject.GetComponentInChildren<AnimationSpriteSwapper>();
        if (spriteSwapper != null)
        {
            Debug.Log("INIT SPRITE SWAPPER");
            spriteSwapper.Init(actor.Visuals.CharacterSpriteSet);
        }

        this.left = left;
        if (!left)
        {
            var localScale = gameObject.transform.localScale;
            localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            gameObject.transform.localScale = localScale;
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(GameObject);
    }

    public void Idle(float playSpeed)
    {
        spriteController.Idle(playSpeed);
    }

    public void Critical(float playSpeed)
    {
        spriteController.Critical(playSpeed);
        MonoUtility.DelayFunction(AttackFinished, (float)spriteController.GetCriticalAttackAnimationDuration());
    }

    public void WalkIn(float playSpeed)
    {
        spriteController.WalkIn(playSpeed);
    }

    public void Hide()
    {
    
        GameObject.SetActive(false);
    }

    public bool HasPrepareAnimation()
    {
        return spriteController.HasPrepareAnimation();
    }

    public void Prepare(float playSpeed)
    {
        spriteController.Prepare(playSpeed);
        MonoUtility.DelayFunction(PrepareFinished, (float)spriteController.GetPrepareAnimationDuration());
    }
    void PrepareFinished()
    {
        OnPrepareFinished?.Invoke();
    }


    public void Attack(float playSpeed)
    {
        spriteController.Attack(playSpeed);
    
        MonoUtility.DelayFunction(AttackFinished, spriteController.GetAttackAnimationDuration());
    }

    void AttackFinished()
    {
        OnAttackFinished?.Invoke();
    }

    public void Damaged(float playSpeed, DamagedState damagedState)
    {
        spriteController.Damaged(playSpeed, damagedState);
    }

    public void Death(float playSpeed)
    {
        spriteController.Death(playSpeed);
    }

    public void Dodge(float playSpeed)
    {
        spriteController.Dodge(playSpeed);
    }

    public Vector3 GetImpactPosition()
    {
        return impactPosition.transform.position;
    }

    public bool IsLeft()
    {
        return left;
    }

    

    public RectTransform GetAttractorTransform()
    {
        return spriteController.GetAttractorTransform();
    }

    public ExpBarController GetExpRenderer()
    {
        return spriteController.GetExpRenderer();
    }
}