using System;
using Game.GameActors.Players;
using UnityEngine;

public class AnimatedCombatCharacter
{
    private BattleAnimationSpriteController spriteController;
    private ImpactPosition impactPosition;
    private GameObject gameObject;
    private bool left;
    

    public AnimatedCombatCharacter(GameObject gameObject, bool left)
    {
        this.gameObject = gameObject;
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
        GameObject.Destroy(gameObject);
        
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
        gameObject.SetActive(false);
    }

    public bool HasPrepareAnimation()
    {
        return spriteController.HasPrepareAnimation();
    }

    public void Prepare(float playSpeed)
    {
        spriteController.Prepare(playSpeed);
    }

    public Action OnAttackFinished;
    public void Attack(float playSpeed)
    {
        spriteController.Attack(playSpeed);
        MonoUtility.DelayFunction(OnAttackFinished, (float)spriteController.GetCurrentAnimationDuration());
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
}