using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameInput;

namespace Game.Mechanics
{
    public class BattleSimulation
    {
        public IBattleActor Attacker { get; private set; }
        public IBattleActor Defender { get; private set; }
        public List<Boolean> AttackSequence;
        public List<int> AttackerDamage;
        public List<int> DefenderDamage;
        public List<int> AttackerSpDamage;
        public List<int> DefenderSpDamage;
        public BattleSimulation(IBattleActor attacker, IBattleActor defender)
        {
            
            Attacker = attacker.Clone() as IBattleActor;
            Defender = defender.Clone() as IBattleActor;
            AttackSequence = new List<bool>();
            AttackerDamage = new List<int>();
            DefenderDamage = new List<int>();
            AttackerSpDamage = new List<int>();
            DefenderSpDamage = new List<int>();
        }
        public bool DoAttack(IBattleActor attacker, IBattleActor defender)
        {
            int damage = attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender);
            //int spDamage= attacker.BattleComponent.BattleStats.GetTotalSpDamageAgainstTarget(defender);
            if (attacker == Attacker)
            {
                AttackerDamage.Add(Math.Min(defender.Hp, damage));
                //AttackerSpDamage.Add(Math.Min(defender.Sp, spDamage));
                if (attacker is Human humanAttacker && humanAttacker.EquippedWeapon != null)
                {
                    //DefenderSpDamage.Add(Math.Min(attacker.Sp, humanAttacker.EquippedWeapon.Weight));
                    //attacker.Sp -= humanAttacker.EquippedWeapon.Weight;
                    

                }
            }
            else
            {
                DefenderDamage.Add(Math.Min(defender.Hp, damage));
               // DefenderSpDamage.Add(Math.Min(defender.Sp, spDamage));
                if (attacker is Human humanAttacker && humanAttacker.EquippedWeapon != null)
                {
                    //AttackerSpDamage.Add(Math.Min(attacker.Sp, humanAttacker.EquippedWeapon.Weight));
                    //attacker.Sp -= humanAttacker.EquippedWeapon.Weight;
                   
                }
            }
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
                if (attackerAttackCount > 0)
                {
                    AttackSequence.Add(true);
                    if (DoAttack(Attacker, Defender))
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
                        break;
                    }
                }
                if (defenderAttackCount > 0)
                {
                    AttackSequence.Add(false);
                    if (DoAttack(Defender, Attacker))
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
                        break;
                    }
                }
            }

            Attacker.SpBars--;
            Defender.SpBars--;
        }

        
    }
}