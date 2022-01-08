using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.Mechanics.Battle;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class BattleSystem : IEngineSystem
    {
        public delegate void OnStartAttackEvent();

        public static OnStartAttackEvent OnStartAttack;

        private const float FIGHT_TIME = 3.8f;
        private const float ATTACK_DELAY = 0.0f;
        private IBattleActor attacker;
        private IBattleActor defender;
        private int attackCount;
        private bool battleStarted;
        private BattleSimulation battleSimulation;
        private int attackerAttackCount;
        private int defenderAttackCount;
        private int currentAttackIndex;
        public bool IsFinished;
        public IBattleRenderer BattleRenderer { get; set; }


        
        public void StartBattle(IBattleActor attacker, IBattleActor defender)
        {
            this.attacker = attacker;
            this.defender = defender;
            battleSimulation = new BattleSimulation(attacker,defender);
            battleSimulation.StartBattle(false);
            battleStarted = true;
            IsFinished = false;
            currentAttackIndex = 0;
            attackerAttackCount = attacker.BattleComponent.BattleStats.GetAttackCountAgainst(defender);
            defenderAttackCount = defender.BattleComponent.BattleStats.GetAttackCountAgainst(attacker);
            //BattleRenderer.Show(attacker, defender, GetAttackSequence());
            
        }

        // public void ContinueBattle(IBattleActor attacker, IBattleActor defender)
        // {
        //     ContinueBattle(battleSimulation.AttackSequence[currentAttackIndex]);
        // }
        // private void ContinueBattle(bool attackerAttacking)
        // {
        //    
        //     if (attackerAttacking)
        //         DoAttack(attacker, defender);
        //     else
        //         DoAttack(defender, attacker);
        //     currentAttackIndex++;
        // }

        // private static bool DoAttack(IBattleActor attacker, IBattleActor defender)
        // {
        //     bool crit = defender.SpBars == 0;
        //     bool magic = attacker.BattleComponent.BattleStats.GetDamageType() == DamageType.Magic;
        //     bool eff = false;
        //
        //     defender.BattleComponent.InflictDamage(attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender),magic, crit,eff, defender);
        //     return defender.Hp > 0;
        // }

        // public bool[] GetAttackSequence()
        // {
        //     return battleStarted ? battleSimulation.AttackSequence.ToArray() : null;
        // }


      
        IEnumerator Delay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
        // public void EndBattle()
        // {
        //         //
        //         // defender.SpBars--;
        //         // attacker.SpBars--;
        //         if (!attacker.IsAlive())
        //         {
        //             attacker.Die();
        //         }
        //         if (!defender.IsAlive())
        //         {
        //             defender.Die();
        //         }
        //
        //    
        //         battleStarted = false;
        //         //BattleRenderer.Hide();
        //         IsFinished = true;
        //         //GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.BattleEnded);
        //     
        //
        //
        //
        // }
    
        public BattleSimulation GetBattleSimulation(IBattleActor attacker, IBattleActor defender)
        {
            battleSimulation = new BattleSimulation(attacker, defender);
            battleSimulation.StartBattle(false);

            return battleSimulation;
        }


        public BattlePreview GetBattlePreview(IBattleActor attacker, IBattleActor defender)
        {
            var battlePreview = ScriptableObject.CreateInstance<BattlePreview>();
            battlePreview.Attacker = attacker;
            battlePreview.Defender = defender;
            battleSimulation = new BattleSimulation(attacker, defender);
            battleSimulation.StartBattle(true);
            battlePreview.AttacksData = battleSimulation.AttacksData;

            battlePreview.AttackerStats = new BattlePreviewStats(attacker.BattleComponent.BattleStats.GetDamage(), 
                attacker.Stats.Attributes.AGI, defender.BattleComponent.BattleStats.GetDamageType(), 
                defender.BattleComponent.BattleStats.GetDamageType()==DamageType.Physical ? attacker.BattleComponent.BattleStats.GetArmor() : attacker.Stats.Attributes.FAITH, 
                attacker.Stats.Attributes.DEX, attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender), attacker.BattleComponent.BattleStats.GetHitAgainstTarget(defender),
                attacker.BattleComponent.BattleStats.GetAttackCountAgainst(defender), attacker.Hp, attacker.Stats.MaxHp, 
                battleSimulation.Attacker.Hp);//, attacker.Sp, attacker.Stats.MaxSp, battleSimulation.Attacker.Sp, attacker.SpBars, battleSimulation.Attacker.SpBars, attacker.MaxSpBars);

            battlePreview.DefenderStats = new BattlePreviewStats(defender.BattleComponent.BattleStats.GetDamage(), 
                defender.Stats.Attributes.AGI, attacker.BattleComponent.BattleStats.GetDamageType(), 
                attacker.BattleComponent.BattleStats.GetDamageType()==DamageType.Physical? defender.BattleComponent.BattleStats.GetArmor() : defender.Stats.Attributes.FAITH,
                defender.Stats.Attributes.DEX, defender.BattleComponent.BattleStats.GetDamageAgainstTarget(attacker),defender.BattleComponent.BattleStats.GetHitAgainstTarget(attacker),
                battleSimulation.DefenderAttackCount, defender.Hp, defender.Stats.MaxHp,
                battleSimulation.Defender.Hp);//, defender.Sp, defender.Stats.MaxSp, battleSimulation.Defender.Sp,  defender.SpBars, battleSimulation.Defender.SpBars, defender.MaxSpBars);
            return battlePreview;
        }

        public void Init()
        {
            
        }

        public void Deactivate()
        {
            
        }

        public void Activate()
        {
         
        }

        public void Update()
        {
            
        }
    }
}