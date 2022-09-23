﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameInput;
using Game.Grid;
using Game.GUI;
using Game.Manager;
using Game.Mechanics.Battle;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class BattleSystem : IEngineSystem, ICombatInformation
    {
        public delegate void OnStartAttackEvent();

        public static OnStartAttackEvent OnStartAttack;
        public  static event Action<BattleResult> OnBattleFinished;

        private const float FIGHT_TIME = 3.8f;
        private const float ATTACK_DELAY = 0.0f;
        private IBattleActor attacker;
        private IBattleActor defender;
        private IAttackableTarget attackableTarget;
        private int attackCount;
        private bool battleStarted;
        private BattleSimulation battleSimulation;
        private int attackerAttackCount;
        private int defenderAttackCount;
        private int currentAttackIndex;
        public bool IsFinished;
        public IBattleAnimation BattleAnimation { get; set; }


        public void StartBattle(IBattleActor attacker, IAttackableTarget attackableTarget)
        {
            this.attacker = attacker;
            this.attackableTarget = attackableTarget;
            battleSimulation = new BattleSimulation(attacker,attackableTarget);
            battleSimulation.StartBattle(false, true);
            battleStarted = true;
            IsFinished = false;
            currentAttackIndex = 0;
            attackerAttackCount = 1;
            //defenderAttackCount = defender.BattleComponent.BattleStats.GetAttackCountAgainst(attacker);
            //BattleRenderer.Show(attacker, defender, GetAttackSequence());
            
        }
        public void StartBattle(IBattleActor attacker, IBattleActor defender, bool grid, bool continuos =false)
        {
             this.attacker = attacker;
            this.defender = defender;
            // battleSimulation = new BattleSimulation(attacker,defender);
            // battleSimulation.StartBattle(false);
            // battleStarted = true;
            // IsFinished = false;
            // currentAttackIndex = 0;
            // attackerAttackCount = attacker.BattleComponent.BattleStats.GetAttackCountAgainst(defender);
            // defenderAttackCount = defender.BattleComponent.BattleStats.GetAttackCountAgainst(attacker);
            battleSimulation = GetBattleSimulation(attacker, (IBattleActor)defender, grid, continuos);
            Debug.Log(BattleAnimation+" "+battleSimulation);
            BattleAnimation.Show(battleSimulation, attacker, (IBattleActor)defender);
            BattleAnimation.OnFinished -= EndBattle;
            BattleAnimation.OnFinished += EndBattle;
            //BattleRenderer.Show(attacker, defender, GetAttackSequence());
            
        }
        private void EndBattle()
        {
            Debug.Log("End Battle");
            Debug.Log(attacker);
            Debug.Log(battleSimulation);
            Debug.Log(battleSimulation.Attacker);
            
            attacker.Hp = battleSimulation.Attacker.Hp;
            if (battleSimulation.AttackableTarget == null)
                defender.Hp = battleSimulation.Defender.Hp;
            else
                defender.Hp = battleSimulation.AttackableTarget.Hp;
          
            attacker = null;
            defender = null;
            BattleAnimation.Hide();
            OnBattleFinished?.Invoke(battleSimulation.BattleResult);
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
    
        public BattleSimulation GetBattleSimulation(IBattleActor attacker, IBattleActor defender, bool grid, bool continuos=false)
        {
            Debug.Log("GetBattleSimulation Hereinstead");
            battleSimulation = new BattleSimulation(attacker, defender, continuos);
            battleSimulation.StartBattle(false, grid);

            return battleSimulation;
        }
        public BattleSimulation GetBattleSimulation(IBattleActor attacker, IAttackableTarget attackableTarget, bool grid)
        {
            Debug.Log("GetBattleSimulation Here");
            battleSimulation = new BattleSimulation(attacker, attackableTarget);
            battleSimulation.StartBattle(false, grid);

            return battleSimulation;
        }


        public BattlePreview GetBattlePreview(IBattleActor attacker, IAttackableTarget defender, GridPosition attackPosition)
        {
            var battlePreview = ScriptableObject.CreateInstance<BattlePreview>();
            battlePreview.Attacker = attacker;
            if (defender is IBattleActor defenderActor)
            {
                battlePreview.Defender = defenderActor;
                battleSimulation = new BattleSimulation(attacker, defenderActor, attackPosition);
                battleSimulation.StartBattle(true, true);
                battlePreview.AttacksData = battleSimulation.combatRounds[0].AttacksData;
                Debug.Log("BattlePreview: " + battleSimulation.combatRounds[0].AttackerAttackCount + "DefenderAttackCount: " +
                          battleSimulation.combatRounds[0].DefenderAttackCount);
                battlePreview.AttackerStats = new BattlePreviewStats(attacker.BattleComponent.BattleStats.GetDamage(),
                    attacker.Stats.Attributes.AGI, defenderActor.BattleComponent.BattleStats.GetDamageType(),
                    defenderActor.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                        ? attacker.BattleComponent.BattleStats.GetPhysicalResistance()
                        : attacker.Stats.Attributes.FAITH,
                    attacker.Stats.Attributes.DEX,
                    attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defenderActor),
                    attacker.BattleComponent.BattleStats.GetHitAgainstTarget(defenderActor),
                    attacker.BattleComponent.BattleStats.GetCritAgainstTarget(defenderActor),
                    battleSimulation.combatRounds[0].AttackerAttackCount, attacker.Hp, attacker.MaxHp,
                    battleSimulation.Attacker.Hp); //, attacker.Sp, attacker.Stats.MaxSp, battleSimulation.Attacker.Sp, attacker.SpBars, battleSimulation.Attacker.SpBars, attacker.MaxSpBars);
                Debug.Log(battleSimulation.Defender);
                Debug.Log(battleSimulation.Defender.Hp);
                Debug.Log( defenderActor.Stats.Attributes.AGI);
                Debug.Log(defenderActor.BattleComponent.BattleStats.GetDamage() + " test ");
                battlePreview.DefenderStats = new BattlePreviewStats(defenderActor.BattleComponent.BattleStats.GetDamage(),
                    defenderActor.Stats.Attributes.AGI, defenderActor.BattleComponent.BattleStats.GetDamageType(),
                    attacker.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                        ? defenderActor.BattleComponent.BattleStats.GetPhysicalResistance()
                        : defenderActor.Stats.Attributes.FAITH,
                    defenderActor.Stats.Attributes.DEX,
                    defenderActor.BattleComponent.BattleStats.GetDamageAgainstTarget(attacker),
                    defenderActor.BattleComponent.BattleStats.GetHitAgainstTarget(attacker),
                    defenderActor.BattleComponent.BattleStats.GetCritAgainstTarget(attacker),
                    battleSimulation.combatRounds[0].DefenderAttackCount, defender.Hp, defenderActor.MaxHp,
                    battleSimulation.Defender.Hp); //, defender.Sp, defender.Stats.MaxSp, battleSimulation.Defender.Sp,  defender.SpBars, battleSimulation.Defender.SpBars, defender.MaxSpBars);
            }
            else
            {
                battlePreview.TargetObject = defender;
                
                battleSimulation = new BattleSimulation(attacker, defender, attackPosition);
                battleSimulation.StartBattle(true, true);
                
                battlePreview.AttacksData = battleSimulation.combatRounds[0].AttacksData;
                Debug.Log("BattlePreview: " + battleSimulation.combatRounds[0].AttackerAttackCount + "DefenderAttackCount: " +
                          battleSimulation.combatRounds[0].DefenderAttackCount);
                battlePreview.AttackerStats = new BattlePreviewStats(attacker.BattleComponent.BattleStats.GetDamage(),
                    attacker.Stats.Attributes.AGI, attacker.BattleComponent.BattleStats.GetDamageType(),0,
                    attacker.Stats.Attributes.DEX,
                    attacker.BattleComponent.BattleStats.GetDamage(),
                    100,
                    0,
                    battleSimulation.combatRounds[0].DefenderAttackCount, attacker.Hp, attacker.MaxHp,
                    battleSimulation.Attacker
                        .Hp); //, attacker.Sp, attacker.Stats.MaxSp, battleSimulation.Attacker.Sp, attacker.SpBars, battleSimulation.Attacker.SpBars, attacker.MaxSpBars);
                Debug.Log(attacker.BattleComponent.BattleStats.GetDamageType());
                Debug.Log(battlePreview.DefenderStats);
                Debug.Log(battleSimulation.Defender);
                battlePreview.DefenderStats = new BattlePreviewStats(0,
                    0, attacker.BattleComponent.BattleStats.GetDamageType(),
                    0,
                    0,
                    0,
                    0,
                    0,
                   0, defender.Hp, defender.MaxHp,
                    battleSimulation.AttackableTarget
                        .Hp);
            }
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

        public ICombatResult GetCombatResultAtAttackLocation(IBattleActor attacker, IAttackableTarget targetTarget, Vector2Int tile)
        {
            BattleSimulation battleSim=null;
             if (targetTarget is IBattleActor defender)
            {
                battleSim = new BattleSimulation(attacker, defender, new GridPosition(tile.x, tile.y));
            }
             else
             {
                 battleSim = new BattleSimulation(attacker,targetTarget, new GridPosition(tile.x, tile.y));
              
             }

            battleSim.StartBattle(false, true);
            return battleSim;
        }
    }
}