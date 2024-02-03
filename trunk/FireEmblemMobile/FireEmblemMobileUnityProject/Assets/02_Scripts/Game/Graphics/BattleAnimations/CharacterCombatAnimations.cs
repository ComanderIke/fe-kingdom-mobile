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
      
    public static event Action<AttackData> OnDamageDealt;
    public static event Action<AnimatedCombatCharacter> OnDodge;
    public static event Action<AnimatedCombatCharacter, int, bool> OnDamaged;
    
    private bool leftCharacterAttacker;

    public void Reset()
    {
        if (characterLeft != null)
            characterLeft.Destroy();
        if (characterRight != null)
            characterRight.Destroy();
        leftCharacterDied = false;
        rightCharacterDied = false;
    }

    public void SetLeftCharacterAttacker(bool b)
    {
        leftCharacterAttacker = b;
    }

    public void SpawnLeftCharacter(Unit character)
    {
        characterLeft =
            new AnimatedCombatCharacter(character,
                Instantiate(character.visuals.CharacterSpriteSet.battleAnimatedSprite, transform), true);
        character.BattleGO = characterLeft;
    }

    public void SpawnRightCharacter(Unit character)
    {
        characterRight =
            new AnimatedCombatCharacter(character,
                Instantiate(character.visuals.CharacterSpriteSet.battleAnimatedSprite, transform), false);
        character.BattleGO = characterRight;
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
            Debug.Log("RightCharacterDied");
            characterRight.Hide();
        }
        else
        {
            
            characterRight.Idle(playSpeed);
        }

      
    }

    public void CharacterAttack(AttackData attackData, bool attacker, bool leftCharacterAttacker)
    {
       // Debug.Log("attacker: "+attacker+" LeftAttacker: "+this.leftCharacterAttacker);
        // if (attacker)
        //     leftCharacterAttacker = !leftCharacterAttacker;
        bool b = (leftCharacterAttacker == attacker);
        var attackingCharacter = b ? characterLeft : characterRight;
        var defendingCharacter = b ? characterRight : characterLeft;
        this.leftCharacterAttacker = leftCharacterAttacker;
      //  Debug.Log("LeftCharacterAttacker: "+leftCharacterAttacker);


        if (attackingCharacter.HasPrepareAnimation())
        {
            attackingCharacter.Prepare(playSpeed);
            attackingCharacter.OnPrepareFinished = null;
            attackingCharacter.OnPrepareFinished += () =>
                {
                    Attack(attackingCharacter, defendingCharacter,attackData.crit,attackData.kill);
                    Defend(attackData, defendingCharacter);
                };
        }
        else
        {
            Attack(attackingCharacter,defendingCharacter, attackData.crit, attackData.kill);
            Debug.Log("==============DELAY DEFENCE: "+attackingCharacter.Actor.GetAttackDelay());
            MonoUtility.DelayFunction(()=>Defend(attackData, defendingCharacter),attackingCharacter.Actor.GetAttackDelay());
        }
    }

   

    private void Attack(AnimatedCombatCharacter attacker,AnimatedCombatCharacter defender, bool critical, bool lethal)
        {
            if (critical)
            {
                if (lethal)
                {
                    attacker.Critical(playSpeed, defender.GetDeathDuration());
                }
                else
                {
                    attacker.Critical(playSpeed);
                }
            }
                
            else if (lethal)
            {
                attacker.Attack(playSpeed, defender.GetDeathDuration());
            }
            else
            {
                attacker.Attack(playSpeed);
            }

          
            attacker.OnAttackFinished -= AttackFinished;
            attacker.OnAttackFinished += AttackFinished;
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



    void Dodge(AnimatedCombatCharacter character)
    {
        OnDodge?.Invoke(character);
        character.Dodge(playSpeed); 
       
    
    }

    void Damaged(AnimatedCombatCharacter character, int dmg, bool critical)
    {
        DamagedState state = DamagedState.Damage;
        if (critical)
            state = DamagedState.HighDmg;
        if (dmg == 0)
            state = DamagedState.NoDamage;
        
      
        OnDamaged?.Invoke(character, dmg, critical);
        character.Damaged(playSpeed, state);


    }

    void Death(AnimatedCombatCharacter character, int dmg, bool critical)
    {
        if (character == characterLeft)
        {
            

            leftCharacterDied = true;
            rightCharacterDied = false;
        }
        else
        {
            
            rightCharacterDied = true;
            leftCharacterDied = false;
        }

        OnDamaged?.Invoke(character, dmg, critical);
        character.Death(playSpeed);

    }

    public void Cleanup()
    {
        characterLeft.Actor.BattleGO = null;
        characterRight.Actor.BattleGO = null;
    }
      
 
}