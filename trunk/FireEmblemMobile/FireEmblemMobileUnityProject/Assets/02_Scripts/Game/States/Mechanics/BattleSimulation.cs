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
        public int AttackerDamage { get; set; }
        public int AttackerHit { get; set; }
        public int AttackerAttackCount { get; set; }
        public int DefenderDamage { get; set; }
        public int DefenderHit { get; set; }
        public int DefenderAttackCount { get; set; }
        public bool AttackerAlive{ get; set; }
        public bool DttackerAlive{ get; set; }
        public object AttackerCrit { get; set; }
        public object DefenderCrit { get; set; }

        public List<AttackData> AttacksData;
        public GridPosition attackPosition;
        public BattleSimulation(IBattleActor attacker, IAttackableTarget attackableTarget):this(attacker, attackableTarget, ((Unit)attacker).GridComponent.GridPosition)
        {
           
        }

        public BattleSimulation(IBattleActor attacker, IAttackableTarget attackableTarget, GridPosition attackPosition)
        {
            Attacker = attacker.Clone() as IBattleActor;
            AttackableTarget = attackableTarget.Clone() as IAttackableTarget;
            AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamage();
            AttackerAttackCount = 1;
            AttacksData = new List<AttackData>();
            this.attackPosition =attackPosition;
        }

        public BattleSimulation(IBattleActor attacker, IBattleActor defender):this(attacker, defender, ((Unit)attacker).GridComponent.GridPosition)
        {
            
            
        }
        public BattleSimulation(IBattleActor attacker, IBattleActor defender, GridPosition attackPosition)
        {
            
            Attacker = attacker.Clone() as IBattleActor;
            Defender = defender.Clone() as IBattleActor;
            AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(Defender);
            DefenderDamage = Defender.BattleComponent.BattleStats.GetDamageAgainstTarget(Attacker);
            
            AttackerHit = Attacker.BattleComponent.BattleStats.GetHitAgainstTarget(Defender);
            DefenderHit = Defender.BattleComponent.BattleStats.GetHitAgainstTarget(Attacker);
            AttackerCrit = Attacker.BattleComponent.BattleStats.GetCritAgainstTarget(Defender);
            DefenderCrit = Defender.BattleComponent.BattleStats.GetCritAgainstTarget(attacker);
            
            AttackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender);
            DefenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker);
            AttacksData = new List<AttackData>();
            this.attackPosition =attackPosition;
        }
        public bool DoAttack(IBattleActor attacker, IBattleActor defender, ref AttackData attackData)
        {
            int damage = attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender);
            //int spDamage= attacker.BattleComponent.BattleStats.GetTotalSpDamageAgainstTarget(defender);
            var hitRng = UnityEngine.Random.Range(0, 101);
            var critRng = UnityEngine.Random.Range(0, 101);
            Debug.Log("HitRNG: Attacker: "+hitRng+ " hitRate: "+attacker.BattleComponent.BattleStats.GetHitAgainstTarget(defender));
            attackData.hit =  hitRng<= attacker.BattleComponent.BattleStats.GetHitAgainstTarget(defender);
            attackData.crit =  critRng<= attacker.BattleComponent.BattleStats.GetCritAgainstTarget(defender)&&attackData.hit;
            if (attackData.crit)
                damage *= 2;
            if (attacker == Attacker)
            {
                attackData.Dmg = Math.Min(defender.Hp, damage);
                //AttackerSpDamage.Add(Math.Min(defender.Sp, spDamage));
                if (attacker.GetEquippedWeapon() != null)
                {
                    //DefenderSpDamage.Add(Math.Min(attacker.Sp, humanAttacker.EquippedWeapon.Weight));
                    //attacker.Sp -= humanAttacker.EquippedWeapon.Weight;
                }
            }
            else
            {
                attackData.Dmg = Math.Min(defender.Hp, damage);
               // DefenderSpDamage.Add(Math.Min(defender.Sp, spDamage));
                if (attacker.GetEquippedWeapon() != null)
                {
                    //AttackerSpDamage.Add(Math.Min(attacker.Sp, humanAttacker.EquippedWeapon.Weight));
                    //attacker.Sp -= humanAttacker.EquippedWeapon.Weight;
                }
            }
            if(attackData.hit||certainHit)
                defender.Hp -= damage;
          //  defender.Sp -= spDamage;
           
            //if (defender is Human humanDefender && humanDefender.EquippedWeapon != null)
            //{
            //    defender.Sp -= humanDefender.EquippedWeapon.Weight;
            //}
            return defender.Hp > 0;
        }

        private bool certainHit = false;
        public void StartBattle(bool certainHit)
        {
            this.certainHit = certainHit;
            if (AttackableTarget != null)
            {
                AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamage();
                AttackableTarget.Hp -= AttackerDamage;
                var AttackData = new AttackData();
                AttackData.Dmg = AttackerDamage;
                AttackData.hit = true;
                AttackData.attacker = true;
                AttackData.crit = false;
                AttacksData.Add(AttackData);
                return;
            }
            int attackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender);
            int defenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker);
            GridPosition attackerGridPos = attackPosition;
            Debug.Log("TODO GridPosition-1?!?!?!");
            if (!((IGridActor)Defender).GetActorGridComponent().CanAttack(attackerGridPos.X, attackerGridPos.Y))
            {
                defenderAttackCount = 0;
            }

            DefenderAttackCount = defenderAttackCount;
           
            while (attackerAttackCount > 0||defenderAttackCount>0)
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
                        attackData.kill = true;
                        AttacksData.Add(attackData);
                        break;
                    }
                    AttacksData.Add(attackData);
                }
                if (defenderAttackCount > 0)
                {
                    attackData.attacker = false;
                    if (DoAttack(Defender, Attacker, ref attackData))
                    {
                       
                        defenderAttackCount--;
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
                        attackData.kill = true;
                        AttacksData.Add(attackData);
                        break;
                    }
                    AttacksData.Add(attackData);
                }
               
            }
            
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
            Debug.Log("Damage Ration: "+Attacker+" "+Attacker.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Defender)+" Defender: "+Defender+" "+Defender.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Attacker));
            if (DefenderAttackCount == 0)
                return Attacker.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Defender);
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