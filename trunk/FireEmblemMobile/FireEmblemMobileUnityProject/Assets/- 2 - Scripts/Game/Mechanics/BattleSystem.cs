using System;
using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Humans;
using Assets.GUI;
using Assets.Mechanics.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using Assets.GameEngine;
using Assets.Game.Manager;

namespace Assets.Mechanics
{
    public class BattleSystem : IEngineSystem
    {
        public delegate void OnStartAttackEvent();

        public static OnStartAttackEvent OnStartAttack;

        private const float FIGHT_TIME = 3.8f;
        private const float ATTACK_DELAY = 0.0f;
        private readonly Unit attacker;
        private readonly Unit defender;
        private int attackCount;
        private bool battleStarted;
        private readonly UiSystem uiController;
        private BattleSimulation battleSimulation;
        private int attackerAttackCount;
        private int defenderAttackCount;
        private int currentAttackIndex;
        public BattleSystem()
        {
            battleStarted = false;
        }
        public BattleSystem(Unit attacker, Unit defender):base()
        {
            this.attacker = attacker;
            this.defender = defender;
            uiController = GridGameManager.Instance.GetSystem<UiSystem>();

        }

        public void StartBattle()
        {
            battleSimulation = new BattleSimulation(attacker,defender);
            battleSimulation.StartBattle();
            battleStarted = true;
            currentAttackIndex = 0;
            attackerAttackCount = attacker.BattleStats.GetAttackCountAgainst(defender);
            defenderAttackCount = defender.BattleStats.GetAttackCountAgainst(attacker);
        }

        public void ContinueBattle()
        {
            ContinueBattle(battleSimulation.AttackSequence[currentAttackIndex]);
        }
        private void ContinueBattle(bool attackerAttacking)
        {
           
            if (attackerAttacking)
                DoAttack(attacker, defender);
            else
                DoAttack(defender, attacker);
            currentAttackIndex++;
        }

        public static bool DoAttack(Unit attacker, Unit defender)
        {
            defender.Hp -= attacker.BattleStats.GetDamageAgainstTarget(defender);
            defender.Sp -= attacker.BattleStats.GetTotalSpDamageAgainstTarget(defender);
            if (attacker is Human humanAttacker && humanAttacker.EquippedWeapon != null)
            {
                attacker.Sp -= humanAttacker.EquippedWeapon.Weight;
            }
            if (defender is Human humanDefender && humanDefender.EquippedWeapon != null)
            {
                defender.Sp -= humanDefender.EquippedWeapon.Weight;
            }
            return defender.Hp > 0;
        }

        public bool[] GetAttackSequence()
        {
            return battleStarted ? battleSimulation.AttackSequence.ToArray() : null;
        }
  

        public void EndBattle()
        {
            if (!attacker.IsAlive())
            {
                attacker.Die();
            }
            if (!defender.IsAlive())
            {
                defender.Die();
            }

            DistributeExperience();

            GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.BattleEnded);
            
            UnitActionSystem.OnCommandFinished();
        }
        private void DistributeExperience()
        {
            if (attacker.IsAlive()&&attacker.Faction.IsPlayerControlled)
            {
                attacker.ExperienceManager.AddExp(CalculateExperiencePoints(attacker, defender));
            }
            if (defender.IsAlive() && defender.Faction.IsPlayerControlled)
            {
                defender.ExperienceManager.AddExp(CalculateExperiencePoints(defender, attacker));
            }
        }
        public int CalculateExperiencePoints(Unit expReceiver, Unit enemyFought)
        {
            int levelDifference = expReceiver.ExperienceManager.Level - enemyFought.ExperienceManager.Level;
            bool killEXP = !enemyFought.IsAlive();
            int expLeft = enemyFought.ExperienceManager.EXPLeftToDrain;
            int maxEXPDrain = enemyFought.ExperienceManager.MaxEXPToDrain;
            float chipExpPercent = 0.2f;
            float killExpPercent = 1.0f;
            int exp =(int)( killEXP == true ? killExpPercent * maxEXPDrain : chipExpPercent * maxEXPDrain);
            if(exp > expLeft)
            {
                exp = expLeft;
            }
            if(!killEXP&&expLeft-exp < maxEXPDrain / 5)
            {
                exp = expLeft - maxEXPDrain / 5;
            }
            enemyFought.ExperienceManager.EXPLeftToDrain -= exp;
            if (enemyFought.ExperienceManager.EXPLeftToDrain < 0)
                enemyFought.ExperienceManager.EXPLeftToDrain = 0;
            if (levelDifference < 0)
            {
                exp = (int)(exp * (1f + ((levelDifference * -1) / 10f)));
            }
            if (levelDifference >= 0)
            {
                exp = (int)(exp * (1f - ((levelDifference) / 10f)));
            }
            if (exp <= 0)
                exp = 0;
            Debug.Log("EXP : " +exp);
            return exp;
        }

        public BattlePreview GetBattlePreview(Unit attacker, Unit defender)
        {
            var battlePreview = new BattlePreview();
            battleSimulation = new BattleSimulation(attacker, defender);
            battleSimulation.StartBattle();

            battlePreview.Attacker = new BattlePreviewStats(attacker.BattleStats.GetDamage(), attacker.Stats.Spd, defender.BattleStats.IsPhysical(), defender.BattleStats.IsPhysical() ? attacker.Stats.Def : attacker.Stats.Res, attacker.Stats.Skl, attacker.BattleStats.GetDamageAgainstTarget(defender), attacker.BattleStats.GetAttackCountAgainst(defender), attacker.Hp, attacker.Stats.MaxHp, battleSimulation.Attacker.Hp, battleSimulation.DefenderDamage, attacker.Sp, attacker.Stats.MaxSp, battleSimulation.Attacker.Sp, battleSimulation.DefenderSpDamage);

            battlePreview.Defender = new BattlePreviewStats(defender.BattleStats.GetDamage(), defender.Stats.Spd, attacker.BattleStats.IsPhysical(), attacker.BattleStats.IsPhysical() ? defender.Stats.Def : defender.Stats.Res, defender.Stats.Skl, defender.BattleStats.GetDamageAgainstTarget(attacker), defender.BattleStats.GetAttackCountAgainst(attacker), defender.Hp, defender.Stats.MaxHp, battleSimulation.Defender.Hp, battleSimulation.AttackerDamage, defender.Sp, defender.Stats.MaxSp, battleSimulation.Defender.Sp, battleSimulation.AttackerSpDamage);
            return battlePreview;
        }
    }
}