using System;
using Game.GameActors.Players;
using Game.GameInput;
using Game.GUI;
using UnityEngine;

public class AnimatedCombatCharacter
{
    private BattleAnimationSpriteController spriteController;
    private ImpactPosition impactPosition;
    public GameObject GameObject { get; set; }
    private bool left;
    public IBattleActor Actor { get; set; }

    public AnimatedCombatCharacter(IBattleActor actor,GameObject gameObject, bool left)
    {
        Actor = actor;
        this.GameObject = gameObject;
        this.impactPosition = gameObject.GetComponentInChildren<ImpactPosition>();
        this.spriteController = gameObject.GetComponentInChildren<BattleAnimationSpriteController>();
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

    public void WalkIn(float playSpeed)
    {
        spriteController.WalkIn(playSpeed);
    }

    public void Hide()
    {
        Debug.Log("Hide Character: "+left+" "+GameObject.name);
        GameObject.SetActive(false);
    }

    public bool HasPrepareAnimation()
    {
        return spriteController.HasPrepareAnimation();
    }

    public void Prepare(float playSpeed)
    {
        spriteController.Prepare(playSpeed);
        MonoUtility.DelayFunction(PrepareFinished, (float)spriteController.GetCurrentAnimationDuration());
    }
    void PrepareFinished()
    {
        OnPrepareFinished?.Invoke();
    }
    public Action OnPrepareFinished;
    public Action OnAttackFinished;

    public void Attack(float playSpeed)
    {
        spriteController.Attack(playSpeed);
    
        MonoUtility.DelayFunction(AttackFinished, (float)spriteController.GetCurrentAnimationDuration());
    }

    void AttackFinished()
    {
        OnAttackFinished?.Invoke();
    }

    public void Damaged(float playSpeed)
    {
        spriteController.Damaged(playSpeed);
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

    
    private  Camera camera;
    public void InitCamera(Camera camera)
    {
        this.camera = camera;
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