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
            Debug.Log("PlayIdle");
            characterLeft.Idle(playSpeed);
        }
        
        if (rightCharacterDied)
        {
            characterRight.Hide();
        }
        else
        {
            Debug.Log("PlayIdle");
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
Debug.Log("Attacking Character: "+(leftCharacterAttacker ? "characterLeft" : "characterRight"));

        if (attackingCharacter.HasPrepareAnimation())
        {
            attackingCharacter.Prepare(playSpeed);
            attackingCharacter.OnPrepareFinished = null;
            attackingCharacter.OnPrepareFinished += () =>
                {
                    Attack(attackingCharacter);
                    Defend(attackData, defendingCharacter);
                };
        }
        else
        {
            Attack(attackingCharacter);
            Defend(attackData, defendingCharacter);
        }
    }

   

    private void Attack(AnimatedCombatCharacter attacker)
        {
            attacker.Attack(playSpeed);
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
        if (character == characterLeft)
        {
            Debug.Log("Left Character Died");

            leftCharacterDied = true;
            rightCharacterDied = false;
        }
        else
        {
            Debug.Log("Right Character Died");
            rightCharacterDied = true;
            leftCharacterDied = false;
        }
        

        OnDamaged?.Invoke(character, dmg, critical);
    }

    public void Cleanup()
    {
        characterLeft.Actor.BattleGO = null;
        characterRight.Actor.BattleGO = null;
    }
      
    public void Init(Camera camera1)
    {
        characterLeft.InitCamera(camera1);
        characterRight.InitCamera(camera1);
        
    }
}