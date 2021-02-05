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
        public IBattleRenderer BattleRenderer { get; set; }


        
        public void StartBattle(IBattleActor attacker, IBattleActor defender)
        {
            this.attacker = attacker;
            this.defender = defender;
            battleSimulation = new BattleSimulation(attacker,defender);
            battleSimulation.StartBattle();
            battleStarted = true;
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
            defender.BattleComponent.InflictDamage(attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender), defender);
            defender.Sp -= attacker.BattleComponent.BattleStats.GetTotalSpDamageAgainstTarget(defender);
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
            battleStarted = false;
            //BattleRenderer.Hide();
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
        public int CalculateExperiencePoints(IBattleActor expReceiver, IBattleActor enemyFought)
        {
            int levelDifference = expReceiver.ExperienceManager.Level - enemyFought.ExperienceManager.Level;
            bool killEXP = !enemyFought.IsAlive();
            int expLeft = enemyFought.ExperienceManager.ExpLeftToDrain;
            int maxEXPDrain = ExperienceManager.MAX_EXP_TO_DRAIN;
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
            enemyFought.ExperienceManager.ExpLeftToDrain -= exp;
            if (enemyFought.ExperienceManager.ExpLeftToDrain < 0)
                enemyFought.ExperienceManager.ExpLeftToDrain = 0;
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

        public BattlePreview GetBattlePreview(IBattleActor attacker, IBattleActor defender)
        {
            var battlePreview = ScriptableObject.CreateInstance<BattlePreview>();
            battlePreview.Attacker = attacker;
            battlePreview.Defender = defender;
            battleSimulation = new BattleSimulation(attacker, defender);
            battleSimulation.StartBattle();

            battlePreview.AttackerStats = new BattlePreviewStats(attacker.BattleComponent.BattleStats.GetDamage(), attacker.Stats.Spd, defender.BattleComponent.BattleStats.IsPhysical(), defender.BattleComponent.BattleStats.IsPhysical() ? attacker.Stats.Def : attacker.Stats.Res, attacker.Stats.Skl, attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender), attacker.BattleComponent.BattleStats.GetAttackCountAgainst(defender), attacker.Hp, attacker.Stats.MaxHp, battleSimulation.Attacker.Hp, battleSimulation.DefenderDamage, attacker.Sp, attacker.Stats.MaxSp, battleSimulation.Attacker.Sp, battleSimulation.DefenderSpDamage);

            battlePreview.DefenderStats = new BattlePreviewStats(defender.BattleComponent.BattleStats.GetDamage(), defender.Stats.Spd, attacker.BattleComponent.BattleStats.IsPhysical(), attacker.BattleComponent.BattleStats.IsPhysical() ? defender.Stats.Def : defender.Stats.Res, defender.Stats.Skl, defender.BattleComponent.BattleStats.GetDamageAgainstTarget(attacker), defender.BattleComponent.BattleStats.GetAttackCountAgainst(attacker), defender.Hp, defender.Stats.MaxHp, battleSimulation.Defender.Hp, battleSimulation.AttackerDamage, defender.Sp, defender.Stats.MaxSp, battleSimulation.Defender.Sp, battleSimulation.AttackerSpDamage);
            return battlePreview;
        }

        public void Init()
        {
            
        }
    }
}