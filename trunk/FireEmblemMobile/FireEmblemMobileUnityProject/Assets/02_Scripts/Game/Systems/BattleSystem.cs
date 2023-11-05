using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Passive;
using Game.GameInput;
using Game.Grid;
using Game.GUI;
using Game.Manager;
using Game.Mechanics.Battle;
using Game.States;
using GameEngine;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Mechanics
{
    public class BattleSystem : IEngineSystem, ICombatInformation
    {
        public delegate void OnStartAttackEvent();

        public static OnStartAttackEvent OnStartAttack;
        public  static event Action<AttackResult> OnBattleFinished;
        public static event Action<AttackResult> OnBattleFinishedBeforeAfterBattleStuff;

        private const float FIGHT_TIME = 3.8f;
        private const float ATTACK_DELAY = 0.0f;
        private IBattleActor attacker;
        private IBattleActor defender;
        private IAttackableTarget attackableTarget;
        private int attackCount;
        private BattleSimulation battleSimulation;
  
        private int defenderAttackCount;
    
        public IBattleAnimation BattleAnimation { get; set; }
        private List<SkillMixin> attackerActivatedSkillMixins;
        private List<SkillMixin> defenderActivatedSkillMixins;


        public BattleSystem()
        {
            defenderActivatedSkillMixins = new List<SkillMixin>();
            attackerActivatedSkillMixins = new List<SkillMixin>();
        }
        public void StartBattle(IBattleActor attacker, IAttackableTargetThatCantFightBack attackableTarget)
        {
            this.attacker = attacker;
            this.attackableTarget = attackableTarget;
            battleSimulation = new BattleSimulation(attacker,attackableTarget);
            battleSimulation.StartBattle(false, true);
            
            //defenderAttackCount = defender.BattleComponent.BattleStats.GetAttackCountAgainst(attacker);
            //BattleRenderer.Show(attacker, defender, GetAttackSequence());
            
        }
        public void StartBattle(IBattleActor attacker, IBattleActor defender, bool grid, bool continuos =false)
        {
            Debug.Log("SSTTTTTTTTTAAAAAAART BAAAAAAATTTTTTTLLLLLLE" +attacker+" "+defender);
            this.attacker = attacker;
            this.defender = defender;
            ActivateSkillsAtBattleStart();
            BattlePreview preview = GetBattlePreview(attacker, defender, attacker.GridComponent.GridPosition, grid);
         
            battleSimulation = GetBattleSimulation(attacker, (IBattleActor)defender, grid, continuos);
            Debug.Log("ACTIVATED SKILLS: ");
            foreach (var skill in attackerActivatedSkillMixins)
            {
                Debug.Log("ACTIVATED ATTACKER COMBAT SKILL "+skill.skill.Name);
            }
            foreach (var skill in defenderActivatedSkillMixins)
            {
                Debug.Log("ACTIVATED DEFENDER COMBAT SKILL "+skill.skill.Name);
            }
            foreach (var combatRound in battleSimulation.combatRounds)
            {
                foreach (var attackData in combatRound.AttacksData)
                {
                    foreach (var activatedSkill in attackData.activatedAttackSkills)
                    {
                        Debug.Log("ACTIVATED ATTACK SKILL: "+ activatedSkill.Name+" Attacker: "+attackData.attacker);
                    }
                }
            }

            battleSimulation.AttackerActivatedCombatSkills = SkillMixinToSkillList(attackerActivatedSkillMixins);
            battleSimulation.DefenderActivatedCombatSkills = SkillMixinToSkillList(defenderActivatedSkillMixins);

         
            BattleAnimation.Show(battleSimulation, preview, attacker, (IBattleActor)defender);
            BattleAnimation.OnFinished -= EndBattle;
            BattleAnimation.OnFinished += EndBattle;
            

        }

        private List<Skill> SkillMixinToSkillList(List<SkillMixin> mixins)
        {
            var returnList = new List<Skill>();
            foreach (var skillMixin in mixins)
            {
                returnList.Add(skillMixin.skill);
            }
            return returnList;
        }

        void ActivateSkillsAtBattleStart()
        {
            foreach (var skillMixin in attackerActivatedSkillMixins)
            {
                if (skillMixin is CombatSkillMixin csm)
                {
                    csm.Activate((Unit)attacker, (Unit)defender, true);
                }
                else if (skillMixin is CombatPassiveMixin cpm)
                {
                    cpm.Activate((Unit)attacker, (Unit)defender);
                }
            }
            foreach (var skillMixin in defenderActivatedSkillMixins)
            {
                if (skillMixin is CombatPassiveMixin cpm)
                {
                    cpm.Activate((Unit)defender, (Unit)attacker);
                }
            }
        }

        public void AddAttackerActivatedSkills(SkillMixin skill)
        {
            attackerActivatedSkillMixins.Add(skill);
        }
        public void AddDefenderActivatedSkills(SkillMixin skill)
        {
            defenderActivatedSkillMixins.Add(skill);
        }
        public void RemoveAttackerActivatedSkills(SkillMixin skillMixin)
        {
            if(attackerActivatedSkillMixins.Contains(skillMixin))
                attackerActivatedSkillMixins.Remove(skillMixin);
        }
        public void RemoveDefenderActivatedSkills(SkillMixin skillMixin)
        {
            if(defenderActivatedSkillMixins.Contains(skillMixin))
                defenderActivatedSkillMixins.Remove(skillMixin);
        }

        public void DeactivateCombatSkills()
        {
            for (int i = attackerActivatedSkillMixins.Count - 1; i >= 0; i--)
            {
                if( attackerActivatedSkillMixins[i]is CombatSkillMixin csm)
                    csm.Deactivate();
                if (attackerActivatedSkillMixins[i] is CombatPassiveMixin psm)
                    psm.Deactivate((Unit)attacker, (Unit)defender);
                attackerActivatedSkillMixins.RemoveAt(i);
            }
            for (int i = defenderActivatedSkillMixins.Count - 1; i >= 0; i--)
            {
                if (defenderActivatedSkillMixins[i] is CombatPassiveMixin psm)
                    psm.Deactivate((Unit)attacker, (Unit)defender);
                defenderActivatedSkillMixins.RemoveAt(i);
            }
        }
        private void EndBattle(int lastCombatRoundIndex)
        {
            int hpDelta = attacker.Hp - battleSimulation.combatRounds[lastCombatRoundIndex].AttackerHP;
            ((Unit) attacker).InflictDirectDamage((Unit)defender, hpDelta,defender.GetEquippedWeapon().DamageType, false);
           // attacker.Hp = battleSimulation.Attacker.Hp;
            if (battleSimulation.AttackableTarget == null)
            {
                hpDelta = defender.Hp - battleSimulation.combatRounds[lastCombatRoundIndex].DefenderHP;
                ((Unit) defender).InflictDirectDamage((Unit)attacker, hpDelta,attacker.GetEquippedWeapon().DamageType, false);
                //defender.Hp = battleSimulation.Defender.Hp;
            }
            else
            {
                hpDelta = defender.Hp - battleSimulation.combatRounds[lastCombatRoundIndex].DefenderHP;
                ((Unit) defender).InflictDirectDamage((Unit)attacker, hpDelta,attacker.GetEquippedWeapon().DamageType, false);
                //defender.Hp = battleSimulation.AttackableTarget.Hp;
            }

            DeactivateCombatSkills();
            BattleAnimation.Hide();
            CheckExp();
            attacker = null;
            defender = null;
            OnBattleFinishedBeforeAfterBattleStuff?.Invoke(battleSimulation.AttackResult);
           
            //After Exp Animation and possibly level up animation finished hide battleAnimation and invoke battle finished

        }

        void CheckExp()
        {
         
            
            var system = ServiceProvider.Instance.GetSystem<UnitProgressSystem>();
            Debug.Log("GET SYSTEM: "+system +" from serviceProvider: "+ServiceProvider.Instance);
            var task = new AfterBattleTasks(system, (Unit)attacker, defender);
            task.StartTask();
            task.OnFinished += () =>
            {
                Debug.Log("AfterBattleTaskFinished");
                OnBattleFinished?.Invoke(battleSimulation.AttackResult);
            };
        }
        public BattleSimulation GetBattleSimulation(IBattleActor attacker, IBattleActor defender, bool grid, bool continuos=false)
        {
   
            battleSimulation = new BattleSimulation(attacker, defender, continuos); 
            battleSimulation.StartBattle(false, grid);

            return battleSimulation;
        }
        public BattleSimulation GetBattleSimulation(IBattleActor attacker, IAttackableTarget attackableTarget, bool grid)
        {
         
            battleSimulation = new BattleSimulation(attacker, attackableTarget);
            battleSimulation.StartBattle(false, grid);

            return battleSimulation;
        }

        public BattlePreview GetBattlePreview(IBattleActor attacker, IAttackableTarget defender, GridPosition attackPosition, bool grid=true)
        {
            var battlePreview = ScriptableObject.CreateInstance<BattlePreview>();
            battlePreview.Attacker = attacker;
            if (defender is IBattleActor defenderActor)
            {
                battlePreview.Defender = defenderActor;
                battleSimulation = new BattleSimulation(attacker, defenderActor, attackPosition);
                battleSimulation.StartBattle(true, grid);
                
                battlePreview.AttacksData = battleSimulation.combatRounds[0].AttacksData;
                Debug.Log(attacker.Hp+" "+battleSimulation.combatRounds[0].AttackerStats.MaxHp+ " "+battleSimulation.combatRounds[0].AttackerStats.CurrentHp);
                Debug.Log(defender.Hp+" "+battleSimulation.combatRounds[0].DefenderStats.MaxHp+ " "+battleSimulation.combatRounds[0].DefenderStats.CurrentHp);
                battlePreview.AttackerStats = new BattlePreviewStats(battleSimulation.combatRounds[0].AttackerStats,true, battleSimulation.combatRounds[0].AttackerHP);
                battlePreview.DefenderStats = new BattlePreviewStats(battleSimulation.combatRounds[0].DefenderStats,battleSimulation.combatRounds[0].DefenderCanCounter, battleSimulation.combatRounds[0].DefenderHP);
                // battlePreview.AttackerStats = new BattlePreviewStats(attacker.BattleComponent.BattleStats.GetDamage(),
                //     attacker.Stats.BaseAttributes.AGI, defenderActor.BattleComponent.BattleStats.GetDamageType(),
                //     defenderActor.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                //         ? attacker.BattleComponent.BattleStats.GetPhysicalResistance()
                //         : attacker.Stats.BaseAttributes.FAITH,
                //     attacker.Stats.BaseAttributes.DEX,
                //     attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defenderActor),
                //     attacker.BattleComponent.BattleStats.GetHitAgainstTarget(defenderActor),
                //     attacker.BattleComponent.BattleStats.GetCritAgainstTarget(defenderActor),
                //     battleSimulation.combatRounds[0].AttackerAttackCount, attacker.Hp, attacker.MaxHp,
                //     battleSimulation.Attacker.Hp); //, attacker.Sp, attacker.Stats.MaxSp, battleSimulation.Attacker.Sp, attacker.SpBars, battleSimulation.Attacker.SpBars, attacker.MaxSpBars);
                //
                // battlePreview.DefenderStats = new BattlePreviewStats(defenderActor.BattleComponent.BattleStats.GetDamage(),
                //     defenderActor.Stats.BaseAttributes.AGI, defenderActor.BattleComponent.BattleStats.GetDamageType(),
                //     attacker.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                //         ? defenderActor.BattleComponent.BattleStats.GetPhysicalResistance()
                //         : defenderActor.Stats.BaseAttributes.FAITH,
                //     defenderActor.Stats.BaseAttributes.DEX,
                //     defenderActor.BattleComponent.BattleStats.GetDamageAgainstTarget(attacker),
                //     defenderActor.BattleComponent.BattleStats.GetHitAgainstTarget(attacker),
                //     defenderActor.BattleComponent.BattleStats.GetCritAgainstTarget(attacker),
                //     battleSimulation.combatRounds[0].DefenderAttackCount, defender.Hp, defenderActor.MaxHp,
                //     battleSimulation.Defender.Hp); //, defender.Sp, defender.Stats.MaxSp, battleSimulation.Defender.Sp,  defender.SpBars, battleSimulation.Defender.SpBars, defender.MaxSpBars);
            }
            else
            {
                battlePreview.TargetObject = defender;
                
                battleSimulation = new BattleSimulation(attacker, defender, attackPosition);
                battleSimulation.StartBattle(true, grid);
                
                battlePreview.AttacksData = battleSimulation.combatRounds[0].AttacksData;
                Debug.Log("BattlePreview: " + battleSimulation.combatRounds[0].AttackerAttackCount + "DefenderAttackCount: " +
                          battleSimulation.combatRounds[0].DefenderAttackCount);
                battlePreview.AttackerStats = new BattlePreviewStats(attacker.BattleComponent.BattleStats.GetDamage(),
                    attacker.Stats.BaseAttributes.AGI, attacker.BattleComponent.BattleStats.GetDamageType(),0,
                    attacker.Stats.BaseAttributes.DEX,
                    attacker.BattleComponent.BattleStats.GetDamage(),
                    100,
                    0,
                    battleSimulation.combatRounds[0].DefenderAttackCount, attacker.Hp, attacker.MaxHp,
                    battleSimulation.Attacker
                        .Hp, true); //, attacker.Sp, attacker.Stats.MaxSp, battleSimulation.Attacker.Sp, attacker.SpBars, battleSimulation.Attacker.SpBars, attacker.MaxSpBars);
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
                        .Hp,false);
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

        public void CleanUp()
        {
            BattleAnimation.Cleanup();
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