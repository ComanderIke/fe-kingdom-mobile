﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills.Passive;
using Game.GameInput;
using Game.GUI.PopUpText;
using LostGrace;
using UnityEngine;

namespace Game.Mechanics.Battle
{
    public class BonusAttackStats
    {
        public bool BonusAttack { get; set; }
        public Dictionary<AttackEffectEnum, object> AttackEffects { get; set; } //object userData like for Sol healhamount for luna def/res reduction
        public Dictionary<GetHitEffectEnum, object> DefenseEffects { get; set; } 
        public BonusAttackStats()
        {
            BonusAttack = false;
            AttackEffects = new Dictionary<AttackEffectEnum, object>();
            DefenseEffects = new Dictionary<GetHitEffectEnum, object>();
        }

        public void AddAttackEffect(AttackEffectEnum attackEffect, float f)
        {
            if (!AttackEffects.ContainsKey(attackEffect))
            {
                AttackEffects.Add(attackEffect, f);
            }
            else
            {
                if ((float)AttackEffects[attackEffect] < f) //Replace with stronger effect
                    AttackEffects[attackEffect] = f;
            }
        }

        public void AddGetHitEffect(GetHitEffectEnum getHitEffect, float f)
        {
            if (!DefenseEffects.ContainsKey(getHitEffect))
            {
                DefenseEffects.Add(getHitEffect, f);
            }
            else
            {
                if ((float)DefenseEffects[getHitEffect] < f) //Replace with stronger effect
                    DefenseEffects[getHitEffect] = f;
            }
        }
    }

    public class BattleStats
    {
        private const int AGILITY_TO_DOUBLE = 5;
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
            return owner.GetEquippedWeapon().AttackRanges.Contains(attackingRange);
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
            
            
            int unmodifiedAttack = owner.Stats.CombinedAttributes().STR;
            if (GetDamageType()==DamageType.Magic)
            {
                unmodifiedAttack = owner.Stats.CombinedAttributes().INT;
            }

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
                
                int attacks = 1;
                float multiplier = 1.0f;
                if (CanDoubleAttack(battleActor))
                    attacks = 2;
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

        public const int HIT_DEX_MULT=3;
        
        public int GetHitrate()
        {
            
            
            return (owner.Stats.CombinedAttributes().DEX) * HIT_DEX_MULT+  owner.Stats.CombinedBonusStats().Hit;
            
        }
        public const int AVO_AGI_MULT=2;
        public int GetAvoid()
        {
           
         
           return  (owner.Stats.CombinedAttributes().AGI)* AVO_AGI_MULT+ owner.Stats.CombinedBonusStats().Avoid;
            
            
        }

        public int GetHitAgainstTarget(IBattleActor target)
        {
            return GetHitrate() - target.BattleComponent.BattleStats.GetAvoid();
        }
   

        public int GetAttackSpeed()
        {
            int spd = owner.Stats.CombinedAttributes().AGI + owner.Stats.CombinedBonusStats().AttackSpeed;
            if (MovementToAS)
            {
                spd += ((GridActorComponent)owner.GridComponent).MovedTileCount* MovementToASMultiplier;
            }
            return spd;
           
        }

        public int GetCrit()
        {
            int crit = owner.Stats.CombinedAttributes().LCK + owner.Stats.CombinedBonusStats().Crit;
            if (MovementToCrit)
            {
                crit += ((GridActorComponent)owner.GridComponent).MovedTileCount* MovementToCritMultiplier;
            }
            return crit;
        }
        public const int CRIT_AVO_LCK_MULT=2;
        public int GetCritAvoid()
        {
            return 0;
           // return owner.Stats.CombinedAttributes().LCK*CRIT_AVO_LCK_MULT+ owner.Stats.CombinedBonusStats().CritAvoid;
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
                case CombatStats.CombatStatType.Critavoid: return GetCritAvoid();
                case CombatStats.CombatStatType.Hit: return GetHitrate();
                case CombatStats.CombatStatType.Resistance: return GetFaithResistance();
                case CombatStats.CombatStatType.Protection: return GetPhysicalResistance();
                case CombatStats.CombatStatType.AttackSpeed: return GetAttackSpeed();
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
                case CombatStats.CombatStatType.Critavoid: return owner.Stats.GetBonusStatsWithoutWeapon().CritAvoid;
                case CombatStats.CombatStatType.Hit: return owner.Stats.GetBonusStatsWithoutWeapon().Hit;
                case CombatStats.CombatStatType.Resistance: return owner.Stats.GetBonusStatsWithoutWeapon().MagicResistance;
                case CombatStats.CombatStatType.Protection: return owner.Stats.GetBonusStatsWithoutWeapon().Armor;
                case CombatStats.CombatStatType.AttackSpeed: return owner.Stats.GetBonusStatsWithoutWeapon().AttackSpeed;
            }

            return -1;
        }

        
    }
}