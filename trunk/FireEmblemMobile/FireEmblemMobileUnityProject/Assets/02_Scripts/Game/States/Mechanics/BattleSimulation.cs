using System;
using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameInput;
using Game.Grid;
using UnityEngine;

namespace Game.Mechanics
{
    public struct AttackData
    {
        public bool attacker;
        public int Dmg;
        public bool hit;
        public bool kill;
        public bool crit;
    }

    public class BattleSimulation: ICombatResult
    {
       
        public IBattleActor Attacker { get; private set; }
        public IBattleActor Defender { get; private set; }
        public IAttackableTarget AttackableTarget { get; private set; }
        
        public List<CombatRound> combatRounds;

        public GridPosition attackPosition;
        public BattleSimulation(IBattleActor attacker, IAttackableTarget attackableTarget):this(attacker, attackableTarget, ((Unit)attacker).GridComponent.GridPosition)
        {
           
        }

        public BattleSimulation(IBattleActor attacker, IAttackableTarget attackableTarget, GridPosition attackPosition)
        {
            Debug.Log("Constructor for IAttackableTarget");
           
                
            combatRounds = new List<CombatRound>();
            Attacker = attacker.Clone() as IBattleActor;
            AttackableTarget = attackableTarget.Clone() as IAttackableTarget;
            var combatRound = new CombatRound()
            {
                RoundIndex = 0,
                AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamage(),
                AttackerAttackCount = 1,
                AttacksData = new List<AttackData>()
            };
            this.attackPosition =attackPosition;
            combatRounds.Add(combatRound);
        }

        public BattleSimulation(IBattleActor attacker, IBattleActor defender, bool continuos = false):this(attacker, defender, ((Unit)attacker).GridComponent.GridPosition, continuos)
        {
            
            
        }
        public BattleSimulation(IBattleActor attacker, IBattleActor defender, GridPosition attackPosition, bool continuos = false)
        {
            Debug.Log("Constructor for IBattleActor");
            combatRounds = new List<CombatRound>();
            Attacker = attacker.Clone() as IBattleActor;
            Defender = defender.Clone() as IBattleActor;
            if (continuos)
            {
                //Multiple Rounds Get Count
                
                //Try out 2 rounds
                var combatRound = new CombatRound()
                {
                    RoundIndex = 0,
                    AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(Defender),
                    DefenderDamage = Defender.BattleComponent.BattleStats.GetDamageAgainstTarget(Attacker),
                    AttackerHit = Attacker.BattleComponent.BattleStats.GetHitAgainstTarget(Defender),
                    DefenderHit = Defender.BattleComponent.BattleStats.GetHitAgainstTarget(Attacker),
                    AttackerCrit = Attacker.BattleComponent.BattleStats.GetCritAgainstTarget(Defender),
                    DefenderCrit = Defender.BattleComponent.BattleStats.GetCritAgainstTarget(attacker),
                    AttackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender),
                    DefenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker),
                    AttacksData = new List<AttackData>()
                };

                combatRounds.Add(combatRound);
                var combatRound2 = new CombatRound()
                {
                    RoundIndex = 1,
                    AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(Defender),
                    DefenderDamage = Defender.BattleComponent.BattleStats.GetDamageAgainstTarget(Attacker),
                    AttackerHit = Attacker.BattleComponent.BattleStats.GetHitAgainstTarget(Defender),
                    DefenderHit = Defender.BattleComponent.BattleStats.GetHitAgainstTarget(Attacker),
                    AttackerCrit = Attacker.BattleComponent.BattleStats.GetCritAgainstTarget(Defender),
                    DefenderCrit = Defender.BattleComponent.BattleStats.GetCritAgainstTarget(attacker),
                    AttackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender),
                    DefenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker),
                    AttacksData = new List<AttackData>()
                };

                combatRounds.Add(combatRound2);
            }
            else
            {
                //1 Round of Combat as normal
       
                var combatRound = new CombatRound()
                {
                    RoundIndex = 0,
                    AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(Defender),
                    DefenderDamage = Defender.BattleComponent.BattleStats.GetDamageAgainstTarget(Attacker),
                    AttackerHit = Attacker.BattleComponent.BattleStats.GetHitAgainstTarget(Defender),
                    DefenderHit = Defender.BattleComponent.BattleStats.GetHitAgainstTarget(Attacker),
                    AttackerCrit = Attacker.BattleComponent.BattleStats.GetCritAgainstTarget(Defender),
                    DefenderCrit = Defender.BattleComponent.BattleStats.GetCritAgainstTarget(attacker),
                    AttackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender),
                    DefenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker),
                    AttacksData = new List<AttackData>()
                };

                combatRounds.Add(combatRound);

            }
           
            this.attackPosition =attackPosition;
        }
        public bool DoAttack(IBattleActor attacker, IBattleActor defender, ref AttackData attackData)
        {
            int damage = attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender);
            //int spDamage= attacker.BattleComponent.BattleStats.GetTotalSpDamageAgainstTarget(defender);
            var hitRng = UnityEngine.Random.Range(0, 101);
            var critRng = UnityEngine.Random.Range(0, 101);
           // Debug.Log("HitRNG: Attacker: "+hitRng+ " hitRate: "+attacker.BattleComponent.BattleStats.GetHitAgainstTarget(defender));
            attackData.hit =  hitRng<= attacker.BattleComponent.BattleStats.GetHitAgainstTarget(defender);
            attackData.crit =  critRng<= attacker.BattleComponent.BattleStats.GetCritAgainstTarget(defender)&&attackData.hit;
            if (attackData.crit)
                damage *= 2;
            if (attacker == Attacker)
            {
                attackData.Dmg = Math.Min(defender.Hp, damage);
            }
            else
            {
                attackData.Dmg = Math.Min(defender.Hp, damage);
            }
            if(attackData.hit||certainHit)
                defender.Hp -= damage;
            return defender.Hp > 0;
        }

        private bool certainHit = false;

        private void StartRound(CombatRound combatRound, bool certainHit, bool grid)
        {
            if (AttackableTarget != null)
            {
                combatRound.AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamage();
                AttackableTarget.Hp -= combatRound.AttackerDamage;
                var AttackData = new AttackData();
                AttackData.Dmg = combatRound.AttackerDamage;
                AttackData.hit = true;
                AttackData.attacker = true;
                AttackData.crit = false;
                combatRound.AttacksData.Add(AttackData);
                return;
            }
            int attackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender);
            int defenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker);
            Debug.Log("Battle Base AttackCounts: "+attackerAttackCount + " " + defenderAttackCount);
            GridPosition attackerGridPos = attackPosition;
            Debug.Log("TODO GridPosition-1?!?!?!");
            Debug.Log("DefenderAttackCount: "+defenderAttackCount);
            if (grid&&!((IGridActor)Defender).GetActorGridComponent().CanAttack(attackerGridPos.X, attackerGridPos.Y))
            {
                defenderAttackCount = 0;
            }
            Debug.Log("AttackCount: "+attackerAttackCount);
            Debug.Log("DefenderAttackCount: "+defenderAttackCount);
            combatRound.DefenderAttackCount = defenderAttackCount;
            bool death = false;
            while ((attackerAttackCount > 0||defenderAttackCount>0)&&!death)
            {
                AttackData attackData=new AttackData();
                
                if (attackerAttackCount > 0)
                {
                    attackData.attacker = true;
                    if (DoAttack(Attacker, Defender, ref attackData))
                    {
                        
                        attackerAttackCount--;
                        // if (Defender.SpBars <= 0)
                        // {
                        //     defenderAttackCount = 0;
                        // }
                        // if (Attacker.SpBars <= 0)
                        // {
                        //     attackerAttackCount = 0;
                        // }
                    }
                    else
                    {
                        death = true;
                        attackData.kill = true;
                        combatRound.AttacksData.Add(attackData);
                        break;
                    }
                    combatRound.AttacksData.Add(attackData);
                }
                if (defenderAttackCount > 0)
                {
                    attackData.attacker = false;
                    if (DoAttack(Defender, Attacker, ref attackData))
                    {
                       
                        defenderAttackCount--;
                    }
                    else
                    {
                        death = true;
                        attackData.kill = true;
                        combatRound.AttacksData.Add(attackData);
                        break;
                    }
                    combatRound.AttacksData.Add(attackData);
                }
            }
        }
        public void StartBattle(bool certainHit, bool grid)
        {
            this.certainHit = certainHit;
            Debug.Log(Attacker);
            Debug.Log(Defender);
            foreach (var combatRound in combatRounds)
            {
                StartRound(combatRound, certainHit, grid);
            }

            Debug.Log(Attacker);
            Debug.Log(Defender);
            if (Attacker.IsAlive() && Defender.IsAlive())
                BattleResult = BattleResult.Draw;
            if (!Defender.IsAlive())
                BattleResult = BattleResult.Win;
            if (!Attacker.IsAlive())
                BattleResult = BattleResult.Loss;

            // Attacker.SpBars--; 
            // if(defenderAttackCount!=0)
            //     Defender.SpBars--;
        }


        public Vector2Int GetAttackPosition()
        {
            return new Vector2Int(attackPosition.X, attackPosition.Y);
        }

        public BattleResult BattleResult { get; set; }
        public int GetDamageRatio()
        {
           
            if (Defender == null)
                return Attacker.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(AttackableTarget);
            //Debug.Log("Damage Ration: "+Attacker+" "+Attacker.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Defender)+" Defender: "+Defender+" "+Defender.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Attacker));
            if (combatRounds[0].DefenderAttackCount == 0)
                return Attacker.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Defender);
            Debug.Log(Attacker+" ");
            Debug.Log(" "+Defender);
            Debug.Log(Attacker.BattleComponent+" ");
            Debug.Log(" "+Defender.BattleComponent);
            Debug.Log(Attacker.BattleComponent.BattleStats+" ");
            Debug.Log(" "+Defender.BattleComponent.BattleStats);
            return Attacker.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Defender) -
                   Defender.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Attacker);
        }

        public int GetTileDefenseBonuses()
        {
            return Attacker.GetTile().TileData.defenseBonus;
        }

       

        public int GetTileAvoidBonuses()
        {
            return Attacker.GetTile().TileData.avoBonus;
        }
    }
}