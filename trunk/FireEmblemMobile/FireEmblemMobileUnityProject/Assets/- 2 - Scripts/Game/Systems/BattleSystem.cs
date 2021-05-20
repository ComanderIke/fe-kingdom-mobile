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
            battleSimulation.StartBattle();
            battleStarted = true;
            IsFinished = false;
            currentAttackIndex = 0;
            attackerAttackCount = attacker.BattleComponent.BattleStats.GetAttackCountAgainst(defender);
            defenderAttackCount = defender.BattleComponent.BattleStats.GetAttackCountAgainst(attacker);
            BattleRenderer.Show(attacker, defender, GetAttackSequence());
            
        }

        public void ContinueBattle(IBattleActor attacker, IBattleActor defender)
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

        private static bool DoAttack(IBattleActor attacker, IBattleActor defender)
        {
            bool crit = defender.SpBars == 0;
            bool magic = attacker.BattleComponent.BattleStats.GetDamageType() == DamageType.Magic;
            bool eff = false;

            defender.BattleComponent.InflictDamage(attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender),magic, crit,eff, defender);
            
            
            // defender.Sp -= attacker.BattleComponent.BattleStats.GetTotalSpDamageAgainstTarget(defender);
            // if (attacker is Human humanAttacker && humanAttacker.EquippedWeapon != null)
            // {
            //     attacker.Sp -= humanAttacker.EquippedWeapon.Weight;
            // }
            // if (defender is Human humanDefender && humanDefender.EquippedWeapon != null)
            // {
            //     defender.Sp -= humanDefender.EquippedWeapon.Weight;
            // }
            return defender.Hp > 0;
        }

        public bool[] GetAttackSequence()
        {
            return battleStarted ? battleSimulation.AttackSequence.ToArray() : null;
        }
  

        public void EndBattle()
        {
            defender.SpBars--;
            attacker.SpBars--;
            if (!attacker.IsAlive())
            {
                attacker.Die();
            }
            if (!defender.IsAlive())
            {
                defender.Die();
            }

           
            battleStarted = false;
            //BattleRenderer.Hide();
            IsFinished = true;
            //GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.BattleEnded);
            
            
            
        }
    
        

        public BattlePreview GetBattlePreview(IBattleActor attacker, IBattleActor defender)
        {
            var battlePreview = ScriptableObject.CreateInstance<BattlePreview>();
            battlePreview.Attacker = attacker;
            battlePreview.Defender = defender;
            battleSimulation = new BattleSimulation(attacker, defender);
            battleSimulation.StartBattle();

            battlePreview.AttackerStats = new BattlePreviewStats(attacker.BattleComponent.BattleStats.GetDamage(), attacker.Stats.Spd, defender.BattleComponent.BattleStats.GetDamageType(), defender.BattleComponent.BattleStats.GetDamageType()==DamageType.Physical ? attacker.Stats.Def : attacker.Stats.Res, attacker.Stats.Skl, attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender), attacker.BattleComponent.BattleStats.GetAttackCountAgainst(defender), attacker.Hp, attacker.Stats.MaxHp, battleSimulation.Attacker.Hp, battleSimulation.DefenderDamage, attacker.Sp, attacker.Stats.MaxSp, battleSimulation.Attacker.Sp, battleSimulation.DefenderSpDamage);

            battlePreview.DefenderStats = new BattlePreviewStats(defender.BattleComponent.BattleStats.GetDamage(), defender.Stats.Spd, attacker.BattleComponent.BattleStats.GetDamageType(), attacker.BattleComponent.BattleStats.GetDamageType()==DamageType.Physical? defender.Stats.Def : defender.Stats.Res, defender.Stats.Skl, defender.BattleComponent.BattleStats.GetDamageAgainstTarget(attacker), defender.BattleComponent.BattleStats.GetAttackCountAgainst(attacker), defender.Hp, defender.Stats.MaxHp, battleSimulation.Defender.Hp, battleSimulation.AttackerDamage, defender.Sp, defender.Stats.MaxSp, battleSimulation.Defender.Sp, battleSimulation.AttackerSpDamage);
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