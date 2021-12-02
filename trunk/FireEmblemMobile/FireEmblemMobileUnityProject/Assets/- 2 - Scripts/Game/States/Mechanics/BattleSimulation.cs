using System;
using System.Collections.Generic;
using System.Linq;
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
    }
    public class BattleSimulation
    {
       
        public IBattleActor Attacker { get; private set; }
        public IBattleActor Defender { get; private set; }
        public int AttackerDamage { get; set; }
        public int AttackerHit { get; set; }
        public int AttackerAttackCount { get; set; }
        public int DefenderDamage { get; set; }
        public int DefenderHit { get; set; }
        public int DefenderAttackCount { get; set; }
        public bool AttackerAlive{ get; set; }
        public bool DttackerAlive{ get; set; }

        public List<AttackData> AttacksData;
        public BattleSimulation(IBattleActor attacker, IBattleActor defender)
        {
            
            Attacker = attacker.Clone() as IBattleActor;
            Defender = defender.Clone() as IBattleActor;
            AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(Defender);
            DefenderDamage = Defender.BattleComponent.BattleStats.GetDamageAgainstTarget(Attacker);
            
            AttackerHit = Attacker.BattleComponent.BattleStats.GetHitAgainstTarget(Defender);
            DefenderHit = Defender.BattleComponent.BattleStats.GetHitAgainstTarget(Attacker);
            
            AttackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender);
            DefenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker);
            AttacksData = new List<AttackData>();
        }
        public bool DoAttack(IBattleActor attacker, IBattleActor defender, ref AttackData attackData)
        {
            int damage = attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender);
            //int spDamage= attacker.BattleComponent.BattleStats.GetTotalSpDamageAgainstTarget(defender);
            attackData.hit = UnityEngine.Random.Range(0, 101) <= attacker.BattleComponent.BattleStats.GetHitAgainstTarget(defender);
            if (attacker == Attacker)
            {
              
                attackData.Dmg = Math.Min(defender.Hp, damage);
                //AttackerSpDamage.Add(Math.Min(defender.Sp, spDamage));
                if (attacker is Human humanAttacker && humanAttacker.EquippedWeapon != null)
                {
                    //DefenderSpDamage.Add(Math.Min(attacker.Sp, humanAttacker.EquippedWeapon.Weight));
                    //attacker.Sp -= humanAttacker.EquippedWeapon.Weight;
                    

                }
            }
            else
            {
                
                attackData.Dmg = Math.Min(defender.Hp, damage);
               // DefenderSpDamage.Add(Math.Min(defender.Sp, spDamage));
                if (attacker is Human humanAttacker && humanAttacker.EquippedWeapon != null)
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
            int attackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender);
            int defenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker);
            GridPosition attackerGridPos = ((Unit)Attacker).GridComponent.GridPosition;
            Debug.Log("TODO GridPosition-1?!?!?!");
            if (!((IGridActor)Defender).GridComponent.CanAttack(attackerGridPos.X, attackerGridPos.Y))
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
                        if (Defender.SpBars <= 0)
                        {
                            defenderAttackCount = 0;
                        }
                        if (Attacker.SpBars <= 0)
                        {
                            attackerAttackCount = 0;
                        }
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
                        if (Defender.SpBars <= 0)
                        {
                            defenderAttackCount = 0;
                        }
                        if (Attacker.SpBars <= 0)
                        {
                            attackerAttackCount = 0;
                        }
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
            
            Attacker.SpBars--; 
            Defender.SpBars--;
        }

        
    }
}