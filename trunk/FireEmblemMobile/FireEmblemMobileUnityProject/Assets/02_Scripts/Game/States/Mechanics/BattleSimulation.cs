using System;
using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Passive;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Map;
using Game.Mechanics.Battle;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Mechanics
{
    public struct AttackData
    {
        public bool attacker;
        public int Dmg;
        public int Heal;
        public bool hit;
        public bool kill;
        public bool crit;
        public List<Skill> activatedAttackSkills;
        public List<Skill> activatedDefenseSkills;
        
    }

    public class BattleSimulation: ICombatResult
    {
        private bool continuos;
        // private bool preview = false;
        public IBattleActor Attacker { get; private set; }
        public IBattleActor Defender { get; private set; }
        public IAttackableTarget AttackableTarget { get; private set; }
        
        public List<CombatRound> combatRounds;

        public GridPosition attackPosition;
        public List<Skill> AttackerActivatedCombatSkills;
        public List<Skill> DefenderActivatedCombatSkills;
        private Tile attackPosTile;
        public BattleSimulation(IBattleActor attacker, IAttackableTarget attackableTarget):this(attacker, attackableTarget, ((Unit)attacker).GridComponent.GridPosition)
        {
           
        }

        public BattleSimulation(IBattleActor attacker, IAttackableTarget attackableTarget, GridPosition attackPosition)
        {
            Debug.Log("Constructor for IAttackableTarget");
            AttackerActivatedCombatSkills = new List<Skill>();
            DefenderActivatedCombatSkills = new List<Skill>();
            attackPosTile = ServiceProvider.Instance.GetSystem<GridSystem>().GetTile(attackPosition.X, attackPosition.Y);
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
            attackPosTile = ServiceProvider.Instance.GetSystem<GridSystem>().GetTile(attackPosition.X, attackPosition.Y);
            Attacker = attacker.Clone() as IBattleActor;
            Defender = defender.Clone() as IBattleActor;
            this.continuos = continuos;
            Attacker.BattleComponent.InitiatesBattle(Defender);
            int range =Math.Abs(attackPosition.X - defender.GridComponent.GridPosition.X)+Math.Abs(
                attackPosition.Y - defender.GridComponent.GridPosition.Y);
            // Debug.Log("Attack Range: "+range+ " "+defender.GridComponent.GridPosition.AsVector()+" "+attackPosition.AsVector());
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
                    AttacksData = new List<AttackData>(),
                    DefenderCanCounter = Defender.BattleComponent.BattleStats.CanCounter(range)
                };
                combatRound.AttackerStats =  new DuringBattleCharacterStats(Attacker.BattleComponent.BattleStats.GetDamage(),
                    Attacker.Stats.BaseAttributes.AGI, Defender.BattleComponent.BattleStats.GetDamageType(),
                    Defender.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                        ? Attacker.BattleComponent.BattleStats.GetPhysicalResistance()
                        : Attacker.Stats.BaseAttributes.FAITH,
                    Attacker.Stats.BaseAttributes.DEX,
                    Attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(Defender),
                    Attacker.BattleComponent.BattleStats.GetHitAgainstTarget(Defender),
                    Attacker.BattleComponent.BattleStats.GetCritAgainstTarget(Defender),
                    Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender), Attacker.Hp, Attacker.MaxHp); 
                combatRound.DefenderStats = new DuringBattleCharacterStats(Defender.BattleComponent.BattleStats.GetDamage(),
                    Defender.Stats.BaseAttributes.AGI, Defender.BattleComponent.BattleStats.GetDamageType(),
                    Attacker.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                        ? Defender.BattleComponent.BattleStats.GetPhysicalResistance()
                        : Defender.Stats.BaseAttributes.FAITH,
                    Defender.Stats.BaseAttributes.DEX,
                    Defender.BattleComponent.BattleStats.GetDamageAgainstTarget(Attacker),
                    Defender.BattleComponent.BattleStats.GetHitAgainstTarget(Attacker),
                    Defender.BattleComponent.BattleStats.GetCritAgainstTarget(Attacker),
                    Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker), Defender.Hp, Defender.MaxHp);
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
                    AttacksData = new List<AttackData>(),
                    DefenderCanCounter = Defender.BattleComponent.BattleStats.CanCounter(range)
                };
                combatRound.AttackerStats =  new DuringBattleCharacterStats(Attacker.BattleComponent.BattleStats.GetDamage(),
                    Attacker.Stats.BaseAttributes.AGI, Defender.BattleComponent.BattleStats.GetDamageType(),
                    Defender.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                        ? Attacker.BattleComponent.BattleStats.GetPhysicalResistance()
                        : Attacker.Stats.BaseAttributes.FAITH,
                    Attacker.Stats.BaseAttributes.DEX,
                    Attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(Defender),
                    Attacker.BattleComponent.BattleStats.GetHitAgainstTarget(Defender),
                    Attacker.BattleComponent.BattleStats.GetCritAgainstTarget(Defender),
                    Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender), Attacker.Hp, Attacker.MaxHp); 
                combatRound.DefenderStats = new DuringBattleCharacterStats(Defender.BattleComponent.BattleStats.GetDamage(),
                    Defender.Stats.BaseAttributes.AGI, Defender.BattleComponent.BattleStats.GetDamageType(),
                    Attacker.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                        ? Defender.BattleComponent.BattleStats.GetPhysicalResistance()
                        : Defender.Stats.BaseAttributes.FAITH,
                    Defender.Stats.BaseAttributes.DEX,
                    Defender.BattleComponent.BattleStats.GetDamageAgainstTarget(Attacker),
                    Defender.BattleComponent.BattleStats.GetHitAgainstTarget(Attacker),
                    Defender.BattleComponent.BattleStats.GetCritAgainstTarget(Attacker),
                    Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker), Defender.Hp, Defender.MaxHp);
                combatRounds.Add(combatRound);

            }
           
            this.attackPosition =attackPosition;
        }
        public bool DoAttack(IBattleActor attacker, IBattleActor defender, ref AttackData attackData, bool preview)
        {
          
          //  if(attacker.BattleComponent.BattleStats.BonusAttackStats.AttackEffects)
          float sol = 0;
          float luna = 0;
          float pavise = 0;
          float wrath = 0;
          bool guaranteedHit = false;
          foreach (var attackEffect in attacker.BattleComponent.BattleStats.BonusAttackStats.AttackEffects)
          {
              switch (attackEffect.Key)
              {
                  case AttackEffectEnum.Luna:
                      luna = (float)attackEffect.Value; break;
                  case AttackEffectEnum.Sol:
                      sol = (float)attackEffect.Value; break;
                  case AttackEffectEnum.GuaranteedHit:
                      guaranteedHit = true;
                      break;
              }
          }
          foreach (var defenseEffect in defender.BattleComponent.BattleStats.BonusAttackStats.DefenseEffects)
          {
              switch (defenseEffect.Key)
              {
                  case GetHitEffectEnum.Pavise:
                      pavise = (float)defenseEffect.Value; break;
                  case GetHitEffectEnum.Wrath:
                      wrath = (float)defenseEffect.Value; break;
              }
          }
          int damage = attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(defender, luna);
         
            //int spDamage= attacker.BattleComponent.BattleStats.GetTotalSpDamageAgainstTarget(defender);
            var hitRng = UnityEngine.Random.Range(0, 101);
            var critRng = UnityEngine.Random.Range(0, 101);
     
            attackData.hit =  guaranteedHit || hitRng < attacker.BattleComponent.BattleStats.GetHitAgainstTarget(defender);
        
            attackData.crit =  critRng< attacker.BattleComponent.BattleStats.GetCritAgainstTarget(defender)&&attackData.hit;
            if (attackData.crit&&!preview)
                damage *= 2;
            damage -= (int)(damage * pavise);
            // if (attacker == Attacker)
            // {
            //     attackData.Dmg = Math.Min(defender.Hp, damage);
            // }
            attackData.Dmg = damage;
            
            int wrathDmg = (int)(damage * wrath);
            if(attackData.hit&&!preview)
                Defender.BattleComponent.BattleStats.WrathDamage += wrathDmg;
            Attacker.BattleComponent.BattleStats.WrathDamage = 0;
            if(attackData.hit||preview)
                defender.Hp -= damage;
            if(sol>0)
                attacker.Hp += (int)(damage * sol);
            return defender.Hp > 0;
        }

       

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

                if (attackerAttackCount > 0)
                {
                    int consecutiveAttack = 1;
                    bool adeptFlag = false;
                    while (consecutiveAttack > 0)
                    {
                        AttackData attackData = new AttackData();
                        attackData.activatedAttackSkills = new List<Skill>();
                        attackData.activatedDefenseSkills = new List<Skill>();

                        attackData.attacker = true;
                        if (!certainHit)
                        {
                            attackData.activatedAttackSkills.AddRange(ActivateAttackSkills(Attacker));
                            attackData.activatedDefenseSkills.AddRange(ActivateDefenseSkills(Defender));
                        }

                        foreach (var attackEffect in Attacker.BattleComponent.BattleStats.BonusAttackStats
                                     .AttackEffects)
                        {
                            switch (attackEffect.Key)
                            {
                                case AttackEffectEnum.Adept:
                                    if (!adeptFlag)
                                    {
                                        adeptFlag = true;
                                        Debug.Log(attackEffect.Value);
                                        consecutiveAttack+=Convert.ToInt32(attackEffect.Value);
                                    }

                                    break;
                                case AttackEffectEnum.Cancel:
                                    defenderAttackCount = 0;
                                    break;
                            }
                        }

                        if (DoAttack(Attacker, Defender, ref attackData, certainHit))
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
                            
                            
                            if (!certainHit)
                            {
                                death = true;
                                attackData.kill = true;
                                defenderAttackCount = 0;
                                combatRound.AttacksData.Add(attackData);
                                break;
                            }
                            attackerAttackCount--;
                           
                        }

                        combatRound.AttacksData.Add(attackData);
                        consecutiveAttack--;
                    }
                }
               
                if (defenderAttackCount > 0)
                {
                    AttackData attackData=new AttackData();
                   
                    attackData.attacker = false;
                    attackData.activatedAttackSkills = new List<Skill>();
                    attackData.activatedDefenseSkills = new List<Skill>();
                    if (!certainHit)
                    {
                        attackData.activatedAttackSkills.AddRange(ActivateAttackSkills(Defender));
                        attackData.activatedAttackSkills.AddRange(ActivateDefenseSkills(Attacker));
                    }

                    if (DoAttack(Defender, Attacker, ref attackData, certainHit))
                    {
                       
                        defenderAttackCount--;
                    }
                    else
                    {
                        if (!certainHit)
                        {
                            attackData.kill = true;
                            death = true;
                            combatRound.AttacksData.Add(attackData);
                            break;
                        }
                        defenderAttackCount--;
                       
                       
                    }
                    combatRound.AttacksData.Add(attackData);
                }
            }
          //  Debug.Log(combatRound.RoundIndex);
          //  Debug.Log("HP After CombatRound: "+Attacker.Hp);
          //  Debug.Log("HP After CombatRound: "+Defender.Hp);
            combatRound.AttackerHP = Attacker.Hp;
            combatRound.DefenderHP = Defender.Hp;
        }

        private IEnumerable<Skill> ActivateAttackSkills(IBattleActor attacker)
        {
            var skills = new List<Skill>();
           // Debug.Log("ACTIVATE ATTACK SKILLS " +attacker.BattleComponent.attackEffects.Count);
            foreach (var attackEffect in attacker.BattleComponent.attackEffects)
            {
                if(attackEffect.attackEffect.ReactToAttack(attacker))
                    skills.Add(attackEffect.skill);
            }

            return skills;
        }
        private IEnumerable<Skill> ActivateDefenseSkills(IBattleActor attacker)
        {
            var skills = new List<Skill>();
            Debug.Log("ACTIVATE DEFENSE SKILLS " +attacker.BattleComponent.defenseEffects.Count);
            foreach (var attackEffect in attacker.BattleComponent.defenseEffects)
            {
                if(attackEffect.attackEffect.ReactToDefense(attacker))
                    skills.Add(attackEffect.skill);
            }

            return skills;
        }

        public void StartBattle(bool certainHit, bool grid)
        {
            Attacker.BattleComponent.BattleStats.WrathDamage = 0;
            Defender.BattleComponent.BattleStats.WrathDamage = 0;
            //this.preview = certainHit;
            MyDebug.LogTODO("TODO if certainHit also check for skills and only do 100% procChance skills");
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
                    combatRound.AttackerStats =  new DuringBattleCharacterStats(Attacker.BattleComponent.BattleStats.GetDamage(),
                        Attacker.Stats.BaseAttributes.AGI, Defender.BattleComponent.BattleStats.GetDamageType(),
                        Defender.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                            ? Attacker.BattleComponent.BattleStats.GetPhysicalResistance()
                            : Attacker.Stats.BaseAttributes.FAITH,
                        Attacker.Stats.BaseAttributes.DEX,
                        Attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(Defender),
                        Attacker.BattleComponent.BattleStats.GetHitAgainstTarget(Defender),
                        Attacker.BattleComponent.BattleStats.GetCritAgainstTarget(Defender),
                        Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Defender), Attacker.Hp, Attacker.MaxHp); 
                    combatRound.DefenderStats = new DuringBattleCharacterStats(Defender.BattleComponent.BattleStats.GetDamage(),
                        Defender.Stats.BaseAttributes.AGI, Defender.BattleComponent.BattleStats.GetDamageType(),
                        Attacker.BattleComponent.BattleStats.GetDamageType() == DamageType.Physical
                            ? Defender.BattleComponent.BattleStats.GetPhysicalResistance()
                            : Defender.Stats.BaseAttributes.FAITH,
                        Defender.Stats.BaseAttributes.DEX,
                        Defender.BattleComponent.BattleStats.GetDamageAgainstTarget(Attacker),
                        Defender.BattleComponent.BattleStats.GetHitAgainstTarget(Attacker),
                        Defender.BattleComponent.BattleStats.GetCritAgainstTarget(Attacker),
                        Attacker.BattleComponent.BattleStats.GetAttackCountAgainst(Attacker), Defender.Hp, Defender.MaxHp);
                    combatRounds.Add(combatRound);
                   
                    StartRound(combatRound, certainHit, grid);
                    cnt++;
                }
        
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
            Attacker.BattleComponent.BattleEnded(Defender);
            Defender.BattleComponent.BattleEnded(Attacker);
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
            
            return attackPosTile.TileData.defenseBonus;
        }

        public int GetTileSpeedBonuses()
        {
            return attackPosTile.TileData.speedMalus;
        }

        public int GetTileAvoidBonuses()
        {
            return attackPosTile.TileData.avoBonus;
        }
    }

   
}