﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
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
            Debug.Log(owner.GetTile().X+" "+owner.GetTile().Y);
            return owner.Stats.Def + owner.GetTile().TileType.defenseBonus;
        }
        public bool CanKillTarget(IBattleActor target, float attackMultiplier)
        {
            return GetDamageAgainstTarget(target, attackMultiplier) >= target.Hp;
        }

        public int GetAttackCountAgainst(IBattleActor c)
        {
            int attackCount = 1;
            if (owner.SpBars == 0)
                return 0;

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
                    weaponDamage = c.EquippedWeapon.Dmg;
            }

            int unmodifiedAttack = owner.Stats.Str + weaponDamage;
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
            if (target.SpBars <= 0)
            {
                dmgMult = 2;
            }
                
            return (int) (Mathf.Clamp((GetDamage(atkMultiplier) - - target.BattleComponent.BattleStats.GetDefense())*dmgMult, 1, Mathf.Infinity));
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
                if (human.EquippedWeapon.WeaponType != WeaponType.Magic)
                    return human.Stats.Str + human.EquippedWeapon.Dmg;
                else
                    return human.Stats.Mag + human.EquippedWeapon.Dmg;
            }

            return owner.Stats.Str;
        }

        public int GetReceivedDamage(int damage, bool magic = false)
        {
            if (magic)
                return (int) Mathf.Clamp(damage, 1, Mathf.Infinity);
            else
                return (int) Mathf.Clamp(damage -  owner.BattleComponent.BattleStats.GetDefense(), 1, Mathf.Infinity);
        }

        public int GetTotalDamageAgainstTarget(IBattleActor target)
        {
            int attacks = 1;
            float multiplier = 1.0f;
            if (CanDoubleAttack(target))
                attacks = 2;
            return (int) (multiplier * attacks * Mathf.Clamp(GetDamage() - target.BattleComponent.BattleStats.GetDefense(), 0, Mathf.Infinity));
        }

        
        public int GetHitrate()
        {
            if (owner is Human human)
            {
                //Debug.Log("TODO ATTACK SPEED CALC");
                return human.Stats.Skl *2 + human.EquippedWeapon.Hit;
            }  if (owner is Monster monster)
            {
                //Debug.Log("TODO ATTACK SPEED CALC");
                return monster.Stats.Skl *2 + monster.Weapon.Hit;
            }

            return owner.Stats.Skl*2;
        }
        public int GetAvoid()
        {
            if (owner is Human human)
            {
                //Debug.Log("TODO ATTACK SPEED CALC");
                return owner.GetTile().TileType.avoidBonus+human.Stats.Spd * 2 - human.EquippedWeapon.Weight;
            }
            
            return owner.GetTile().TileType.avoidBonus+owner.Stats.Spd*2;
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
                return human.Stats.Spd - human.EquippedWeapon.Weight+ owner.GetTile().TileType.speedMalus;
            }

            return owner.Stats.Spd + owner.GetTile().TileType.speedMalus;
        }
    }
}