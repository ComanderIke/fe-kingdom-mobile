using System;
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

        public BonusAttackStats()
        {
            BonusAttack = false;
            AttackEffects = new Dictionary<AttackEffectEnum, object>();
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
    }

    public class BattleStats
    {
        private const int AGILITY_TO_DOUBLE = 5;

        private readonly IBattleActor owner;
        private bool preventDoubleAttacks = false;
        public List<ImmunityType> Immunities { get; set; }
        public BonusAttackStats BonusAttackStats { get; set; }
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
            int weaponDamage = 0;
            
            if (owner.GetEquippedWeapon() != null)
                weaponDamage = owner.GetEquippedWeapon().GetDamage();
            
            int unmodifiedAttack = owner.Stats.CombinedAttributes().STR + weaponDamage;
            if (GetDamageType()==DamageType.Magic)
            {
                unmodifiedAttack = owner.Stats.CombinedAttributes().INT + weaponDamage;
            }
            
           
            int attack = unmodifiedAttack;
            if (attackModifier != null)
            {
                attack += attackModifier.Sum(modi => ((int) (unmodifiedAttack * modi)) - unmodifiedAttack);
            }

            attack += owner.Stats.CombinedBonusStats().Attack;

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
            if (owner.GetEquippedWeapon() != null)
            {
                return owner.GetEquippedWeapon().DamageType;
            }

            return DamageType.Physical;
        }
        public int GetAttackDamage()
        {
            if (owner.GetEquippedWeapon() != null)
            {
                if (owner.GetEquippedWeapon().DamageType != DamageType.Magic&&owner.GetEquippedWeapon().DamageType != DamageType.Faith)
                    return owner.Stats.BaseAttributes.STR + owner.GetEquippedWeapon().GetDamage();
                else if (owner.GetEquippedWeapon().DamageType == DamageType.Faith)
                    return owner.Stats.BaseAttributes.FAITH + owner.GetEquippedWeapon().GetDamage();
                else
                    return owner.Stats.BaseAttributes.INT + owner.GetEquippedWeapon().GetDamage();
            }

            return owner.Stats.BaseAttributes.STR+owner.Stats.CombinedBonusStats().Attack;
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

        public const int HIT_DEX_MULT=2;
        
        public int GetHitrate()
        {
       
         
            //Debug.Log("TODO ATTACK SPEED CALC");
            // Debug.Log(human.EquippedWeapon.Hit);
            //Debug.Log(human.Stats.Attributes.DEX);
            
            return (owner.Stats.CombinedAttributes().DEX- owner.GetEquippedWeapon().GetWeight()) * HIT_DEX_MULT+ owner.GetEquippedWeapon().GetHit()+ owner.Stats.CombinedBonusStats().Hit;
            
        }
        public const int AVO_AGI_MULT=2;
        public int GetAvoid()
        {
           
         
           return  (owner.Stats.CombinedAttributes().AGI  - owner.GetEquippedWeapon().GetWeight())* AVO_AGI_MULT+ owner.Stats.CombinedBonusStats().Avoid;
            
            
        }

        public int GetHitAgainstTarget(IBattleActor target)
        {
            return GetHitrate() - target.BattleComponent.BattleStats.GetAvoid();
        }
   

        public int GetAttackSpeed()
        {
            int spd = owner.Stats.CombinedAttributes().AGI - owner.GetEquippedWeapon().GetWeight()+ owner.Stats.CombinedBonusStats().AttackSpeed;
          
            return spd;
           
        }

        public int GetCrit()
        {
            return owner.Stats.CombinedAttributes().LCK+owner.Stats.CombinedAttributes().DEX+ owner.Stats.CombinedBonusStats().Crit;
        }
        public const int CRIT_AVO_LCK_MULT=2;
        public int GetCritAvoid()
        {
            return owner.Stats.CombinedAttributes().LCK*CRIT_AVO_LCK_MULT+ owner.Stats.CombinedBonusStats().CritAvoid;
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
            return Math.Max(0,GetCrit() - defender.BattleComponent.BattleStats.GetCritAvoid());
        }

        public void SetPreventDoubleAttacks(bool prevent)
        {
            preventDoubleAttacks = prevent;
        }

        public int GetStatFromEnum(BonusStats.CombatStatType type)
        {
            switch (type)
            {
                case BonusStats.CombatStatType.Attack: return GetAttackDamage();
                case BonusStats.CombatStatType.Avoid: return GetAvoid();
                case BonusStats.CombatStatType.Crit: return GetCrit();
                case BonusStats.CombatStatType.Critavoid: return GetCritAvoid();
                case BonusStats.CombatStatType.Hit: return GetHitrate();
                case BonusStats.CombatStatType.MagicResistance: return GetFaithResistance();
                case BonusStats.CombatStatType.PhysicalResistance: return GetPhysicalResistance();
                case BonusStats.CombatStatType.AttackSpeed: return GetAttackSpeed();
            }

            return -1;
        }

        public int GetStatWithoutBonusesFromEnum(BonusStats.CombatStatType type)
        {
            Debug.Log("TODO Make Functions that use just base Attributes and equipment but no effects/terrain");
            switch (type)
            {
                case BonusStats.CombatStatType.Attack: return GetAttackDamage();
                case BonusStats.CombatStatType.Avoid: return GetAvoid();
                case BonusStats.CombatStatType.Crit: return GetCrit();
                case BonusStats.CombatStatType.Critavoid: return GetCritAvoid();
                case BonusStats.CombatStatType.Hit: return GetHitrate();
                case BonusStats.CombatStatType.MagicResistance: return GetFaithResistance();
                case BonusStats.CombatStatType.PhysicalResistance: return GetPhysicalResistance();
                case BonusStats.CombatStatType.AttackSpeed: return GetAttackSpeed();
            }

            return -1;
        }
    }
}