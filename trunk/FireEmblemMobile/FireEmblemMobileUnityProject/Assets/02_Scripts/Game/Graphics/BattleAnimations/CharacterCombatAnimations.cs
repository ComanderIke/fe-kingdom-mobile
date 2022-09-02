using System;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GUI.PopUpText;
using Game.Mechanics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterCombatAnimations : MonoBehaviour
{
    private AnimatedCombatCharacter characterLeft;
    private AnimatedCombatCharacter characterRight;
    private float playSpeed = 1.0f;
    public Action OnAttackFinished;
    private bool leftCharacterDied;
    private bool rightCharacterDied;
   
    private float attackDuration = 0.0f;
    private bool leftCharacterAttacker;

    public void Reset()
    {
        if (characterLeft != null)
            characterLeft.Destroy();
        if (characterRight != null)
            characterRight.Destroy();
    }

    public void SetLeftCharacterAttacker(bool b)
    {
        leftCharacterAttacker = b;
    }
    public void SpawnLeftCharacter(Unit character)
    {
        characterLeft = new AnimatedCombatCharacter(Instantiate(character.visuals.CharacterSpriteSet.battleAnimatedSprite, transform), true);
    }
    public void SpawnRightCharacter(Unit character)
    {
        characterRight = new AnimatedCombatCharacter(Instantiate(character.visuals.CharacterSpriteSet.battleAnimatedSprite, transform), false);

    }

    public void SetPlaySpeed(float speed)
    {
        playSpeed = speed;
    }
    public void WalkIn(bool left)
    {
        if (left)
        {
            characterLeft.WalkIn(playSpeed);
            characterRight.Idle(playSpeed);
        }
        else
        {

            characterLeft.Idle(playSpeed);
            characterRight.WalkIn(playSpeed);
        }
    }

    private void AttackFinished()
    {
        OnAttackFinished?.Invoke();
        if (leftCharacterDied)
        {
            characterLeft.Hide();
        }
        else
        {
            characterLeft.Idle(playSpeed);
        }
        if (rightCharacterDied)
        {
            characterRight.Hide();
        }
        else
        {
            characterRight.Idle(playSpeed);
        }
    }
    public void CharacterAttack(bool attacker, bool leftCharacterAttacker)
    {
        if (attacker)
            leftCharacterAttacker = !leftCharacterAttacker;
        var attackingCharacter = leftCharacterAttacker ? characterLeft : characterRight;


        if (attackingCharacter.HasPrepareAnimation())
        {
            attackingCharacter.Prepare(playSpeed);
            
            MonoUtility.DelayFunction(()=>
            {
                if (attacker)
                {
                    Attack(leftCharacterAttacker ? characterLeft : characterRight);
                    Defend(leftCharacterAttacker ? characterRight : characterLeft);
                }
                else
                {
                    Attack(leftCharacterAttacker ? characterRight : characterLeft);
                    Defend(leftCharacterAttacker ? characterLeft : characterRight);
                }
            },(float)attackerSpriteController.GetCurrentAnimationDuration());
        }
        else
        {
            if (attacker)
            {
                Attack(leftCharacterAttacker ? characterLeft : characterRight);
                Defend(leftCharacterAttacker ? characterRight : characterLeft);
            }
            else
            {
                Attack(leftCharacterAttacker ? characterRight : characterLeft);
                Defend(leftCharacterAttacker ? characterLeft : characterRight);
            }

            
        }
    }
    private void Attack(AnimatedCombatCharacter attacker)
    {
        attacker.Attack(playSpeed);
        attacker.OnAttackFinished -= OnAttackFinishedAncor;
        attacker.OnAttackFinished += OnAttackFinishedAncor;
    }

    private void Defend(AttackData attackData, AnimatedCombatCharacter defender)
    {
        if (attackData.hit)
         {
             OnDamageDealt?.Invoke(attackData);
             if (attackData.kill)
                 Death(defender, attackData.Dmg, attackData.crit);
             else
                 Damaged(defender, attackData.Dmg, attackData.crit);
         }
         else
         {
             Dodge(defender);
         }
    }
     
    void OnAttackFinishedAncor()
    {
        OnAttackFinished?.Invoke();
    }
    public static event Action<AttackData> OnDamageDealt;
    public static event Action<AnimatedCombatCharacter> OnDodge;
    public static event Action<AnimatedCombatCharacter, int, bool> OnDamaged;
    void Dodge(AnimatedCombatCharacter character)
    {
        character.Dodge(playSpeed);
        OnDodge?.Invoke(character);
        
         
    }
    void Damaged(AnimatedCombatCharacter character, int dmg, bool critical)
    {
        character.Damaged(playSpeed);
        OnDamaged?.Invoke(character, dmg, critical);
        
         
    }
    void Death(AnimatedCombatCharacter character, int dmg, bool critical)
    {
        character.Death(playSpeed);
        if (leftCharacterAttacker)
            rightCharacterDied = true;
        else
            leftCharacterDied = true;
        OnDamaged?.Invoke(character, dmg, critical);
        
         
    }
}