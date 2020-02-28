using Assets.Scripts.AI.AttackReactions;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Assets.Scripts.GameStates;

public class BattleSystem : EngineSystem
{
    public delegate void OnStartAttack(AttackType attackType = null);
    public static OnStartAttack onStartAttack;

    public delegate void OnAllyTakesDamage(int damage, bool magic=false);
    public static OnAllyTakesDamage onAllyTakesDamage;

    public delegate void OnEnemyTakesDamage(int damage, bool magic=false);
    public static OnEnemyTakesDamage onEnemyTakesDamage;

    public delegate void OnAllyMisses();
    public static OnAllyMisses onAllyMisses;

    public delegate void OnEnemyMisses();
    public static OnEnemyMisses onEnemyMisses;

    private const float FIGHT_TIME = 3.8f;
    private const float ATTACK_DELAY = 0.0f;
    private Unit attacker;
    private Unit defender;
    private AttackReaction reaction;
    private DefenseType defense;
    public bool isDefense;
    private bool frontAttack;
    private bool surpriseAttack;
    private int attackCount;
    private UISystem uiController;

    public BattleSystem(Unit attacker, Unit defender)
    {
        this.attacker = attacker;
        this.defender = defender;
        uiController = MainScript.instance.GetSystem<UISystem>();
        if (attacker.BattleStats.IsFrontalAttack(defender))
        {
            frontAttack = true;
        }
        else if (attacker.BattleStats.IsBackSideAttack(defender))
        {
            surpriseAttack = true;
        }
        if (attacker.Player.IsPlayerControlled)
        {
            isDefense = false;
        }
        else{
            isDefense = true;
        }
            attackCount = attacker.BattleStats.GetAttackCountAgainst(defender);
    }

    public void DoAttack(AttackType attackType)
    {
        attackCount--;
        if (attackCount >= 0)
            MainScript.instance.StartCoroutine(Attack(attackType));
    }
    public void Counter()
    {
        Human human = (Human)defender;
        defense = human.DefenseTypes.Find(a => a.name == "Counter");
    }
    public void Dodge()
    {
        Human human = (Human)defender;
        defense = human.DefenseTypes.Find(a => a.name == "Dodge");
    }
    public void Guard()
    {
        Human human = (Human)defender;
        defense = human.DefenseTypes.Find(a => a.name == "Guard");
    }

    public static bool DoesAttackHit(Unit attacker, Unit defender, AttackType attackType = null, DefenseType defense=null, bool surpriseAttack=false)
    {
        int hit = attacker.BattleStats.GetHitAgainstTarget(defender)
            + (defense == null ? 0 : defense.Hit)
            + (attackType == null ? 0 : attackType.Hit)
            + (surpriseAttack ? attacker.BattleStats.SurpriseAttackBonusHit : 0);
        return UnityEngine.Random.Range(1, 101) <= hit;
    }

    private List<float> GetAttackModifiers(AttackType attackType)
    {
        List<float> attackModifiers = new List<float>();
        if (attackType != null)
            attackModifiers.Add(attackType.DamageMultiplier);
        if (frontAttack)
            attackModifiers.Add(attacker.BattleStats.FrontalAttackModifier);
        if (defense != null)
            attackModifiers.Add(defense.DamageMultiplier);
        return attackModifiers;
    }

    public static void PlayAllyAttackAnimation(int damage=0)
    {
        GameObject.FindObjectOfType<AllySpriteController>().StartAttackAnimation();
        if (damage != 0)
        {
            GameObject.FindObjectOfType<EnemySpriteController>().StartBlinkAnimation();
        }
    }
    public static void PlayEnemyAttackAnimation(int damage=0)
    {
        
        GameObject.FindObjectOfType<EnemySpriteController>().StartAttackAnimation();
        if (damage != 0)
        {
            GameObject.FindObjectOfType<AllySpriteController>().StartBlinkAnimation();
        }
    }
    private void SingleAttack(Unit attacker, Unit defender, AttackType attackType)
    {
        if (DoesAttackHit(attacker, defender, attackType, defense,surpriseAttack))
        {
            List<float> attackModifier = GetAttackModifiers(attackType);

            if (attackType != null && attackType.name == "SpecialAttack")
            {
                ((Human)attacker).SpecialAttackManager.equippedSpecial.UseSpecial(attacker, attacker.BattleStats.GetDamage(attackModifier), defender);
            }
            else
            {
                int damage = defender.InflictDamage(attacker.BattleStats.GetDamage(attackModifier), attacker);
                if (attacker.Player.IsPlayerControlled)
                {
                    PlayAllyAttackAnimation(damage);
                    onEnemyTakesDamage(damage);
                }
                if (defender.Player.IsPlayerControlled)
                {
                    PlayEnemyAttackAnimation(damage);
                    onAllyTakesDamage(damage);
                }
            }
        }
        else
        {
            if (attacker.Player.IsPlayerControlled)
            {
                PlayAllyAttackAnimation();
                onAllyMisses();
            }
            else
            {
                PlayEnemyAttackAnimation();
                onEnemyMisses();
            }
        }

    }

    IEnumerator Attack(AttackType attackType)
    {
        yield return new WaitForSeconds(ATTACK_DELAY);
        SingleAttack(attacker, defender, attackType);
        if (!defender.IsAlive())
        {
            EndFight();
            yield break;
        }
        int reactionChance = 50;
        if (frontAttack)
            reactionChance = 100;
        if (surpriseAttack)
            reactionChance = 0;
        if (UnityEngine.Random.Range(1, 101) <= reactionChance && attackCount == 0 && defender is Monster)
        {
            yield return new WaitForSeconds(1.5f);
            Monster m = (Monster)defender;
            reaction = m.GetRandomAttackReaction();
            reaction.TargetPositions.Add(attacker.GridPosition.GetPos());
            UnitActionSystem.onReactionFinished += EndFight;
            ExecuteReaction();
            //yield break;
        }
        yield return new WaitForSeconds(1.5f);
        if (defense != null && defense.name == "Counter")
        {
            yield return new WaitForSeconds(1.5f);
            SingleAttack(defender, attacker, null);
            yield return new WaitForSeconds(1.0f);
        }
        if (attackCount == 0)
            EndFight();
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(1.0f);
        if (!attacker.IsAlive())
        {
            attacker.Die();
        }
        if (!defender.IsAlive())
        {
            defender.Die();
        }
        UnitActionSystem.onCommandFinished -= EndFight;
        UISystem.onContinuePressed = null;
        MainScript.instance.GameStateManager.SwitchState(GameStateManager.GameplayState);
        attacker.UnitTurnState.UnitTurnFinished();
        UnitActionSystem.onCommandFinished();
    }

    private void EndFight()
    {
        MainScript.instance.StartCoroutine(End());
    }
    private void ExecuteReaction()
    {
        UISystem.onContinuePressed -= ExecuteReaction;
        reaction.Execute();
    }
}

