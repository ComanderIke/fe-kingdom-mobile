﻿using System;
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
        private bool continuos;
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
            combatRounds = new List<CombatRound>();
            Attacker = attacker.Clone() as IBattleActor;
            Defender = defender.Clone() as IBattleActor;
            this.continuos = continuos;
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
                    DefenderCrit = Defender.BattleComponent.BattleStats.GetCritAgainstTarget(Attacker),
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
                combatRound.AttackerHP = Attacker.Hp;
                return;
            }
            int attackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender);
            int defenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker);
    
            GridPosition attackerGridPos = attackPosition;
   
            if (grid&&!((IGridActor)Defender).GetActorGridComponent().CanAttack(attackerGridPos.X, attackerGridPos.Y))
            {
                defenderAttackCount = 0;
            }
        
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

            combatRound.AttackerHP = Attacker.Hp;
            combatRound.DefenderHP = Defender.Hp;
        }
        public void StartBattle(bool certainHit, bool grid)
        {
            this.certainHit = certainHit;


            if (continuos)
            {
                int cnt = 0;
                combatRounds.Clear();
                while (Attacker.IsAlive() && Defender.IsAlive())
                {
                    
                    var combatRound = new CombatRound()
                    {
                        RoundIndex = cnt,
                        AttackerDamage = Attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(Defender),
                        DefenderDamage = Defender.BattleComponent.BattleStats.GetDamageAgainstTarget(Attacker),
                        AttackerHit = Attacker.BattleComponent.BattleStats.GetHitAgainstTarget(Defender),
                        DefenderHit = Defender.BattleComponent.BattleStats.GetHitAgainstTarget(Attacker),
                        AttackerCrit = Attacker.BattleComponent.BattleStats.GetCritAgainstTarget(Defender),
                        DefenderCrit = Defender.BattleComponent.BattleStats.GetCritAgainstTarget(Attacker),
                        AttackerAttackCount = Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender),
                        DefenderAttackCount = Defender.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker),
                        AttacksData = new List<AttackData>()
                    };
                    combatRounds.Add(combatRound);
                    Debug.Log("Round: "+cnt);
                    StartRound(combatRound, certainHit, grid);
                    cnt++;
                }
                Debug.Log("No longer Alive");
                Debug.Log(Attacker.IsAlive());
                Debug.Log(Defender.IsAlive());
            }
            else
            {
                foreach (var combatRound in combatRounds)
                {
                    StartRound(combatRound, certainHit, grid);
                    if (Attacker.IsAlive() && Defender.IsAlive())
                        break;
                }
            }

            if (Attacker.IsAlive() && Defender.IsAlive())
                AttackResult = AttackResult.Draw;
            if (!Defender.IsAlive())
                AttackResult = AttackResult.Win;
            if (!Attacker.IsAlive())
                AttackResult = AttackResult.Loss;
            // foreach (var combatRound in combatRounds)
            // {
            //     Debug.Log("Combat Round: " +combatRound.RoundIndex);
            //    
            //     foreach (var attackData in combatRound.AttacksData)
            //     {
            //         Debug.Log("Attacker: "+attackData.attacker);
            //         Debug.Log("Dmg: "+attackData.Dmg);
            //         Debug.Log("hit: "+attackData.hit);
            //         Debug.Log("kill: "+attackData.kill);
            //     }
            //     Debug.Log("AttackerHP: "+combatRound.AttackerHP);
            //     Debug.Log("DefenderHP: "+combatRound.DefenderHP);
            // }
            // Attacker.SpBars--; 
            // if(defenderAttackCount!=0)
            //     Defender.SpBars--;
        }


        public Vector2Int GetAttackPosition()
        {
            return new Vector2Int(attackPosition.X, attackPosition.Y);
        }

        public AttackResult AttackResult { get; set; }
        public int GetDamageRatio()
        {
           
            if (Defender == null)
                return Attacker.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(AttackableTarget);
            //Debug.Log("Damage Ration: "+Attacker+" "+Attacker.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Defender)+" Defender: "+Defender+" "+Defender.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(Attacker));
            if (combatRounds[0].DefenderAttackCount == 0)
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