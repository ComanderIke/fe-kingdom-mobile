using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using UnityEngine;

namespace Game.Mechanics.Battle
{
    public class BattleStats
    {
        private const int AGILITY_TO_DOUBLE = 5;
        private const float FRONTAL_ATTACK_MOD = 1.5f;

        private readonly Unit owner;
        public float FrontalAttackModifier { get; set; }

        public BattleStats(Unit owner)
        {
            this.owner = owner;
            FrontalAttackModifier = FRONTAL_ATTACK_MOD;
        }

        public bool CanKillTarget(Unit target, float attackMultiplier)
        {
            return GetDamageAgainstTarget(target, attackMultiplier) >= target.Hp;
        }

        public int GetAttackCountAgainst(Unit c)
        {
            int attackCount = 1;
            //if (owner.Stats.Spd - (c.Stats.Spd) > 0)
            //{
            //    attackCount++;
            //}

            if (owner.Stats.Spd - (c.Stats.Spd + AGILITY_TO_DOUBLE) >= 0)
            {
                attackCount++;
            }

            return attackCount;
        }

        public bool CanDoubleAttack(Unit c)
        {
            return owner.Stats.Spd >= c.Stats.Spd + AGILITY_TO_DOUBLE;
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

        public int GetDamageAgainstTarget(Unit target, float atkMultiplier = 1.0f)
        {
            var atkMulti = new List<float> { atkMultiplier };
            if (target.Sp <= 0)
                atkMulti.Add(2.0f);
            return GetDamageAgainstTarget(target, atkMulti);

        }

        public int GetDamageAgainstTarget(Unit target, List<float> atkMultiplier)
        {
            if (target.Sp <= 0)
                atkMultiplier.Add(2);
            return (int) (Mathf.Clamp(GetDamage(atkMultiplier) - target.Stats.Def, 1, Mathf.Infinity));
        }

        public bool IsPhysical()
        {
            if (owner is Human human && human.EquippedWeapon != null)
            {
                return human.EquippedWeapon.WeaponType != WeaponType.Magic;
            }

            return true;
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
                return (int) Mathf.Clamp(damage - owner.Stats.Def, 1, Mathf.Infinity);
        }

        public int GetTotalDamageAgainstTarget(Unit target)
        {
            int attacks = 1;
            float multiplier = 1.0f;
            if (CanDoubleAttack(target))
                attacks = 2;
            return (int) (multiplier * attacks * Mathf.Clamp(GetDamage() - target.Stats.Def, 0, Mathf.Infinity));
        }

        

        public AttackData CreateAttackData(Unit target, List<float> attackMultipliers,
            List<AttackAttributes> attackAttributes)
        {
            if (attackAttributes.Contains(AttackAttributes.FrontalAttack))
            {
                attackMultipliers.Add(FRONTAL_ATTACK_MOD);
            }

            float attackMultiplier = attackMultipliers.Aggregate(1.0f, (current, f) => current * f);

            int dmg = GetDamageAgainstTarget(target, attackMultiplier);

            if (dmg >= target.Hp)
            {
                attackAttributes.Add(AttackAttributes.Lethal);
            }

            var attackData = new AttackData(owner, dmg, attackMultiplier, attackAttributes);
            return attackData;
        }

        public int GetTotalSpDamageAgainstTarget(Unit attacker)
        {
            return Math.Max(owner.Stats.Skl - attacker.Stats.Skl,1);
        }
    }
}