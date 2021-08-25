using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameInput;

namespace Game.Mechanics
{
    public struct AttackData
    {
        public bool attacker;
        public int Dmg;
        public bool hit;
    }
    public class BattleSimulation
    {
       
        public IBattleActor Attacker { get; private set; }
        public IBattleActor Defender { get; private set; }
        public List<AttackData> AttacksData;
        public BattleSimulation(IBattleActor attacker, IBattleActor defender)
        {
            
            Attacker = attacker.Clone() as IBattleActor;
            Defender = defender.Clone() as IBattleActor;
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
            if(attackData.hit)
                defender.Hp -= damage;
          //  defender.Sp -= spDamage;
           
            //if (defender is Human humanDefender && humanDefender.EquippedWeapon != null)
            //{
            //    defender.Sp -= humanDefender.EquippedWeapon.Weight;
            //}
            return defender.Hp > 0;
        }
        public void StartBattle()
        {
            int attackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender);
            int defenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker);
           
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