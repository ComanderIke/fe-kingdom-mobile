﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.GameActors.Units.Numbers;
using Game.GameInput;
using Game.GUI.PopUpText;
using UnityEngine;

namespace Game.Mechanics.Battle
{
    public class BattleStats
    {
        private const int AGILITY_TO_DOUBLE = 5;

        private readonly IBattleActor owner;

        public BattleStats(IBattleActor owner)
        {
            this.owner = owner;
        }
        public int GetDefense()
        {
            
            //Debug.Log(owner.GetTile().X+" "+owner.GetTile().Y);
            return GetPhysicalResistance() + owner.GetTile().TileData.defenseBonus;
        }
        public bool CanKillTarget(IBattleActor target, float attackMultiplier)
        {
            return GetDamageAgainstTarget(target, attackMultiplier) >= target.Hp;
        }

        public int GetAttackCountAgainst(IBattleActor c)
        {
            int attackCount = 1;
            // if (owner.SpBars == 0)
            //     return 0;

            if ( owner.BattleComponent.BattleStats.GetAttackSpeed()- (c.BattleComponent.BattleStats.GetAttackSpeed() + AGILITY_TO_DOUBLE) >= 0)
            {
                attackCount++;
            }
          
            return attackCount;
        }

        public bool CanDoubleAttack(IBattleActor c)
        {
            return owner.BattleComponent.BattleStats.GetAttackSpeed() >= c.BattleComponent.BattleStats.GetAttackSpeed() + AGILITY_TO_DOUBLE;
        }

        public int GetDamage(float attackModifier)
        {
            var list = new List<float> {attackModifier};
            return GetDamage(list);
        }

        public int GetDamage(List<float> attackModifier = null)
        {
            int weaponDamage = 0;
            if (owner is Human c)
            {
                if (c.EquippedWeapon != null)
                    weaponDamage = c.EquippedWeapon.GetDamage();
            }
            int unmodifiedAttack = owner.Stats.Attributes.STR + weaponDamage;
            if (GetDamageType()==DamageType.Magic)
            {
                unmodifiedAttack = owner.Stats.Attributes.INT + weaponDamage;
            }
            
           
            int attack = unmodifiedAttack;
            if (attackModifier != null)
            {
                attack += attackModifier.Sum(modi => ((int) (unmodifiedAttack * modi)) - unmodifiedAttack);
            }

            return (int) Mathf.Clamp(attack, 0, Mathf.Infinity);
        }

        public int GetDamageAgainstTarget(IBattleActor target, float atkMultiplier = 1.0f)
        {
            var atkMulti = new List<float> { atkMultiplier };
           
            return GetDamageAgainstTarget(target, atkMulti);

        }

        public int GetDamageAgainstTarget(IBattleActor target, List<float> atkMultiplier)
        {
            float dmgMult = 1;
            // if (target.SpBars <= 0)
            // {
            //     dmgMult = 2;
            // }

            int defense = 0;
            if (GetDamageType()==DamageType.Magic)
            {
                defense = target.BattleComponent.BattleStats.GetMagicResistance();
            }
            else if (GetDamageType()==DamageType.Faith)
            {
                defense = target.BattleComponent.BattleStats.GetFaithResistance();
            }
            else
                defense = target.BattleComponent.BattleStats.GetDefense();
            
            return (int) (Mathf.Clamp((GetDamage(atkMultiplier) - defense)*dmgMult, 0, Mathf.Infinity));
        }

       

        public DamageType GetDamageType()
        {
            if (owner is Human human && human.EquippedWeapon != null)
            {
                return human.EquippedWeapon.DamageType;
            }

            return DamageType.Physical;
        }
        public int GetAttackDamage()
        {
            if (owner is Human human && human.EquippedWeapon != null)
            {
                if (human.EquippedWeapon.WeaponType != WeaponType.Magic&&human.EquippedWeapon.WeaponType != WeaponType.FaithMagic)
                    return human.Stats.Attributes.STR + human.EquippedWeapon.GetDamage();
                else if (human.EquippedWeapon.WeaponType == WeaponType.FaithMagic)
                    return human.Stats.Attributes.FAITH + human.EquippedWeapon.GetDamage();
                else
                    return human.Stats.Attributes.INT + human.EquippedWeapon.GetDamage();
            }

            return owner.Stats.Attributes.STR;
        }

        public int GetTotalDamageAgainstTarget(IBattleActor target)
        {
            int attacks = 1;
            float multiplier = 1.0f;
            if (CanDoubleAttack(target))
                attacks = 2;
            int defense = 0;
            if (GetDamageType() == DamageType.Magic)
            {
                defense = target.BattleComponent.BattleStats.GetMagicResistance();
            }
            else if(GetDamageType() == DamageType.Faith)
            {
                defense = target.BattleComponent.BattleStats.GetFaithResistance();
            }
            else
            {
                defense = target.BattleComponent.BattleStats.GetDefense();
            }
            return (int) (multiplier * attacks * Mathf.Clamp(GetDamage() -defense, 0, Mathf.Infinity));
        }

        
        public int GetHitrate()
        {
       
            if (owner is Human human)
            {
                //Debug.Log("TODO ATTACK SPEED CALC");
               // Debug.Log(human.EquippedWeapon.Hit);
                //Debug.Log(human.Stats.Attributes.DEX);
                return (human.Stats.Attributes.DEX- human.EquippedWeapon.GetWeight()) * 2 + human.EquippedWeapon.GetHit();
            }  

            return owner.Stats.Attributes.DEX * 2;
        }
        public int GetAvoid()
        {
            var tileBonus = 0;
            if(owner.GetTile()!=null)
                tileBonus=owner.GetTile().TileData.avoBonus;
            if (owner is Human human)
            {
                //Debug.Log("TODO ATTACK SPEED CALC");
                
                return  tileBonus+ (human.Stats.Attributes.AGI  - human.EquippedWeapon.GetWeight())* 2;
            }
            
            return tileBonus + owner.Stats.Attributes.AGI * 2;
        }

        public int GetHitAgainstTarget(IBattleActor target)
        {
            return GetHitrate() - target.BattleComponent.BattleStats.GetAvoid();
        }
   

        public int GetAttackSpeed()
        {
            if (owner is Human human)
            {
                //Debug.Log("TODO ATTACK SPEED CALC");
                return human.Stats.Attributes.AGI - human.EquippedWeapon.GetWeight()+ owner.GetTile().TileData.speedMalus;
            }

            return owner.Stats.Attributes.AGI + owner.GetTile().TileData.speedMalus;
        }

        public int GetCrit()
        {
            return owner.Stats.Attributes.LCK+owner.Stats.Attributes.DEX;
        }

        public int GetCritAvoid()
        {
            return owner.Stats.Attributes.LCK*2;
        }

        public int GetPhysicalResistance()
        {
            // if (owner is Human human&&human.EquippedArmor!=null)
            // {
            //     
            //     return human.EquippedArmor.armor;
            // }
        
            return owner.Stats.Attributes.DEF+ owner.GetTile().TileData.defenseBonus;
        }

        public int GetFaithResistance()
        {
            return owner.Stats.Attributes.FAITH+ owner.GetTile().TileData.defenseBonus;
        }
        public int GetMagicResistance()
        {
            return owner.Stats.Attributes.INT+ owner.GetTile().TileData.defenseBonus;
        }

        public int GetAttackDamage(DamageType damageType)
        {
           
                if (damageType == DamageType.Magic)
                    return owner.Stats.Attributes.INT;
                else if (damageType == DamageType.Faith)
                    return owner.Stats.Attributes.FAITH;
                else
                    return owner.Stats.Attributes.STR;
            

                return owner.Stats.Attributes.STR;
        }

        public int GetCritAgainstTarget(IBattleActor defender)
        {
            return Math.Max(0,GetCrit() - defender.BattleComponent.BattleStats.GetCritAvoid());
        }
    }
}