using Assets.GameActors.Units;
using Assets.GameActors.Units.Humans;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameActors.Items.Weapons;
using UnityEngine;

namespace Assets.Mechanics.Battle
{
    public class BattleStats
    {
        private const int AGILITY_TO_DOUBLE = 0;
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
            if (owner.Stats.Spd - (c.Stats.Spd) > 0)
            {
                attackCount++;
            }

            if (owner.Stats.Spd - (c.Stats.Spd + 5) >= 0)
            {
                attackCount++;
            }

            return attackCount;
        }

        public bool CanDoubleAttack(Unit c)
        {
            return owner.Stats.Spd > c.Stats.Spd + AGILITY_TO_DOUBLE;
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

        public bool IsFrontalAttack(Unit target)
        {
            int deltaX = owner.GridPosition.X - target.GridPosition.X;
            int deltaY = owner.GridPosition.Y - target.GridPosition.Y;
            if (deltaX != 0 && deltaY == 0)
            {
                switch (deltaX)
                {
                    //Target right of attacker and facing right
                    case 1 when !target.GridPosition.FacingLeft:
                    case -1 when target.GridPosition.FacingLeft:
                        return true;
                    default:
                        return false;
                }
            }

            return false;
        }

        public bool IsBackSideAttack(Unit target)
        {
            int deltaX = owner.GridPosition.X - target.GridPosition.X;
            int deltaY = owner.GridPosition.Y - target.GridPosition.Y;
            if (deltaX != 0 && deltaY == 0)
            {
                switch (deltaX)
                {
                    //Target right of attacker and facing left
                    case 1 when target.GridPosition.FacingLeft:
                    case -1 when !target.GridPosition.FacingLeft:
                        return true;
                    default:
                        return false;
                }
            }

            return false;
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
            return Math.Max(Math.Abs(owner.Stats.Skl - attacker.Stats.Skl),1);
        }
    }
}