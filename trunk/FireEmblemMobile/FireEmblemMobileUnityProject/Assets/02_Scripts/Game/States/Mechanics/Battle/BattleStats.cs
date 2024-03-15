using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units.Components;
using Game.GameActors.Units.Interfaces;
using Game.Systems;
using UnityEngine;

namespace Game.States.Mechanics.Battle
{
    public class BattleStats
    {
        private const int AGILITY_TO_DOUBLE = 5;
        public const int HIT_DEX_MULT=3;
        public const int AVO_AGI_MULT=2;
        public const int CURSE_RES_FTH_MULT = 1;
        public const int CRIT_AVO_LCK_MULT=2;
        public const int CRIT_LCK_MULT = 1;
        public const float CRIT_DEX_MULT = 0f;
        public const float CONSECUTIVE_ATTACKS_MULT = .5f;
        public bool ExcessHitToCrit { get; set; }
        public bool MovementToDmg { get; set; }
        private readonly IBattleActor owner;
        private bool preventDoubleAttacks = false;
        
        public List<ImmunityType> Immunities { get; set; }
        public BonusAttackStats BonusAttackStats { get; set; }
        public int WrathDamage { get; set; }
        public int MovementToDmgMultiplier { get; set; }
        public int MovementToCritMultiplier { get; set; }
        public int MovementToASMultiplier { get; set; }
        public bool MovementToAS { get; set; }
        public bool MovementToCrit { get; set; }
        public bool DealMagicDamage { get; set; }

        public enum ImmunityType
        {
            Critical,
            Effective,
            PhysDamage,
            MagicDamage,
            invulnerable
        }
        public BattleStats(IBattleActor owner)
        {
            this.owner = owner;
            BonusAttackStats = new BonusAttackStats();
            Immunities = new List<ImmunityType>();
        }
        public int GetDefense()
        {
            
       
            return GetPhysicalResistance();
        }
        public bool CanKillTarget(IAttackableTarget target, float attackMultiplier)
        {
            return GetDamageAgainstTarget(target, attackMultiplier) >= target.Hp;
        }
        public bool IsMeleeOnly()
        {
            return owner.GetEquippedWeapon().AttackRanges.Length == 1 &&
                   owner.GetEquippedWeapon().AttackRanges.Contains(1);
        }
        public bool CanCounter(int attackingRange)
        {
            // Debug.Log(owner.GameTransformManager.GameObject.name+" Can Counter: "+attackingRange+" "+owner.GetEquippedWeapon().AttackRanges.Contains(attackingRange));
            return owner.GetEquippedWeapon().AttackRanges.Contains(attackingRange)||attackingRange==0;
        }
        public bool IsRangeOnly()
        {
            return !owner.GetEquippedWeapon().AttackRanges.Contains(1);
        }
        public int GetAttackCountAgainst(IBattleActor c)
        {
            int attackCount = 1;
            // if (owner.SpBars == 0)
            //     return 0;

            if (CanDoubleAttack(c))
            {
                attackCount++;
            }
          
            return attackCount;
        }

        public bool CanDoubleAttack(IBattleActor c)
        {
            if (preventDoubleAttacks)
                return false;
            return GetAttackSpeed() >= c.BattleComponent.BattleStats.GetAttackSpeed() + AGILITY_TO_DOUBLE;
        }

        public int GetDamage(float attackModifier)
        {
            var list = new List<float> {attackModifier};
            return GetDamage(list);
        }

        public int GetDamage(List<float> attackModifier = null)
        {
            
            
            int unmodifiedAttack = (int)(owner.Stats.CombinedAttributes().STR*owner.GetEquippedWeapon().GetStrScaling()+
                                         owner.Stats.CombinedAttributes().DEX* owner.GetEquippedWeapon().GetDexScaling()+
                                          owner.Stats.CombinedAttributes().INT*owner.GetEquippedWeapon().GetIntScaling());

            int attack = unmodifiedAttack;
            if (attackModifier != null)
            {
                attack += attackModifier.Sum(modi => ((int) (unmodifiedAttack * modi)) - unmodifiedAttack);
            }

            attack += owner.Stats.CombinedBonusStats().Attack;
            attack += WrathDamage;

            if (MovementToDmg)
            {
                attack += ((GridActorComponent)owner.GridComponent).MovedTileCount* MovementToDmgMultiplier;
                Debug.Log("Get Damage: "+((GridActorComponent)owner.GridComponent).MovedTileCount);
            }
             

            
            return (int) Mathf.Clamp(attack, 0, Mathf.Infinity);
        }

        public int GetDamageAgainstTarget(IAttackableTarget target, float penetration = 0f,float atkMultiplier = 1.0f)
        {
            var atkMulti = new List<float> { atkMultiplier };
           
            return GetDamageAgainstTarget(target,atkMulti,  penetration);

        }

        public int GetDamageAgainstTarget(IAttackableTarget target, List<float> atkMultiplier, float penetration =0f)
        {
            float dmgMult = 1;
            // if (target.SpBars <= 0)
            // {
            //     dmgMult = 2;
            // }

            int defense = 0;
            if (target is Destroyable)
            {
                return GetDamage();
            }
            else if (target is IBattleActor battleActor)
            {
                if (GetDamageType() == DamageType.Magic||GetDamageType() == DamageType.Faith)
                {
                    defense = battleActor.BattleComponent.BattleStats.GetFaithResistance();
                }
                else
                    defense = battleActor.BattleComponent.BattleStats.GetDefense();

                return (int)(Mathf.Clamp((GetDamage(atkMultiplier) - (defense-(int)(defense*penetration))) * dmgMult, 0, Mathf.Infinity));
            }

            return 0;
        }

       

        public DamageType GetDamageType()
        {
            if (DealMagicDamage)
                return DamageType.Magic;
            if (owner.GetEquippedWeapon() != null)
            {
                return owner.GetEquippedWeapon().DamageType;
            }
           
            return DamageType.Physical;
        }

        public int GetTotalDamageAgainstTarget(IAttackableTarget target)
        {
            if (target is Destroyable)
            {
                return GetDamage();
            }
            else if(target is IBattleActor battleActor)
            {
                
                float attacks = 1;
                float multiplier = 1.0f;
                if (CanDoubleAttack(battleActor))
                    attacks = 1 + CONSECUTIVE_ATTACKS_MULT;
                int defense = 0;
                if (GetDamageType() == DamageType.Magic||GetDamageType() == DamageType.Faith)
                {
                    defense = battleActor.BattleComponent.BattleStats.GetFaithResistance();
                }
                else
                {
                    defense = battleActor.BattleComponent.BattleStats.GetDefense();
                }
                // Debug.Log("Target: " + target);
                // Debug.Log("GetDamage: " + GetDamage());
                // Debug.Log("Defense: " + defense);
                // Debug.Log("Mult: " + multiplier);
                // Debug.Log("Attacks: " + attacks);
                return (int)(multiplier * attacks * Mathf.Clamp(GetDamage() - defense, 0, Mathf.Infinity));
            }

            return 0;
        }

        
        
        public int GetHitrate()
        {
            
            
            return (owner.Stats.CombinedAttributes().DEX-GetWeightReduction()) * HIT_DEX_MULT+  owner.Stats.CombinedBonusStats().Hit;
            
        }
        
        public int GetAvoid()
        {
           
         
           return  (owner.Stats.CombinedAttributes().AGI-GetWeightReduction())* AVO_AGI_MULT+ owner.Stats.CombinedBonusStats().Avoid;
            
            
        }

        public int GetHitAgainstTarget(IBattleActor target)
        {
            return GetHitrate() - target.BattleComponent.BattleStats.GetAvoid();
        }

        public int GetWeightReduction()
        {
            int weight= owner.GetEquippedWeapon().GetWeight()-owner.Stats.CombinedAttributes().STR ;
            if (weight < 0)
                weight = 0;
            return weight;
        }

        public int GetAttackSpeed()
        {
            int spd = owner.Stats.CombinedAttributes().AGI + owner.Stats.CombinedBonusStats().AttackSpeed;
           
            spd -= GetWeightReduction();
            if (MovementToAS)
            {
                spd += ((GridActorComponent)owner.GridComponent).MovedTileCount* MovementToASMultiplier;
            }
            return spd;
           
        }

        public int GetCrit()
        {
            int crit = (int)(owner.Stats.CombinedAttributes().DEX*CRIT_DEX_MULT+owner.Stats.CombinedAttributes().LCK *CRIT_LCK_MULT+ owner.Stats.CombinedBonusStats().Crit);
            if (MovementToCrit)
            {
                crit += ((GridActorComponent)owner.GridComponent).MovedTileCount* MovementToCritMultiplier;
            }
            return crit;
        }
       
        public int GetCritAvoid()
        {
            return 0;
           // return owner.Stats.CombinedAttributes().LCK*CRIT_AVO_LCK_MULT+ owner.Stats.CombinedBonusStats().CritAvoid;
        }

        public int GetCurseResistance()
        {
            return owner.Stats.CombinedAttributes().FAITH * BattleStats.CURSE_RES_FTH_MULT;
        }

        public int GetPhysicalResistance()
        {
            // if (owner is Human human&&human.EquippedArmor!=null)
            // {
            //     
            //     return human.EquippedArmor.armor;
            // }
            int def = owner.Stats.CombinedAttributes().DEF + owner.Stats.CombinedBonusStats().Armor;
   
            return def;
        }

        public int GetFaithResistance()
        {
            int fth = owner.Stats.CombinedAttributes().FAITH+owner.Stats.CombinedBonusStats().MagicResistance;
            
            return fth;
        }

       

        public int GetCritAgainstTarget(IBattleActor defender)
        {
           
            if (ExcessHitToCrit)
            {
                
                int hit = GetHitAgainstTarget(defender);
                
                int excessHit = 0;
                if (hit > 100)
                {
                    excessHit=hit - 100;
                }
               
                return Math.Max(0,excessHit + GetCrit() - defender.BattleComponent.BattleStats.GetCritAvoid());
            }
            else
            {
                return Math.Max(0,GetCrit() - defender.BattleComponent.BattleStats.GetCritAvoid());
            }
            
        }

        public void SetPreventDoubleAttacks(bool prevent)
        {
            preventDoubleAttacks = prevent;
        }

        public int GetStatFromEnum(CombatStats.CombatStatType type)
        {
            switch (type)
            {
                case CombatStats.CombatStatType.Attack: return GetDamage();
                case CombatStats.CombatStatType.Avoid: return GetAvoid();
                case CombatStats.CombatStatType.Crit: return GetCrit();
                case CombatStats.CombatStatType.CritAvoid: return GetCritAvoid();
                case CombatStats.CombatStatType.Hit: return GetHitrate();
                case CombatStats.CombatStatType.Resistance: return GetFaithResistance();
                case CombatStats.CombatStatType.Protection: return GetPhysicalResistance();
                case CombatStats.CombatStatType.AttackSpeed: return GetAttackSpeed();
                case CombatStats.CombatStatType.CurseResistance: return GetCurseResistance();
            }

            return -1;
        }

        public int GetStatOnlyBonusesWithoutWeaponFromEnum(CombatStats.CombatStatType type)
        {
            switch (type)
            {
                case CombatStats.CombatStatType.Attack: return owner.Stats.GetBonusStatsWithoutWeapon().Attack;
                case CombatStats.CombatStatType.Avoid: return owner.Stats.GetBonusStatsWithoutWeapon().Avoid;
                case CombatStats.CombatStatType.Crit: return owner.Stats.GetBonusStatsWithoutWeapon().Crit;
                case CombatStats.CombatStatType.CritAvoid: return owner.Stats.GetBonusStatsWithoutWeapon().CritAvoid;
                case CombatStats.CombatStatType.Hit: return owner.Stats.GetBonusStatsWithoutWeapon().Hit;
                case CombatStats.CombatStatType.Resistance: return owner.Stats.GetBonusStatsWithoutWeapon().MagicResistance;
                case CombatStats.CombatStatType.Protection: return owner.Stats.GetBonusStatsWithoutWeapon().Armor;
                case CombatStats.CombatStatType.AttackSpeed: return owner.Stats.GetBonusStatsWithoutWeapon().AttackSpeed;
                case CombatStats.CombatStatType.CurseResistance: return owner.Stats.GetBonusStatsWithoutWeapon().CurseResistance;

            }

            return -1;
        }


       
    }
}