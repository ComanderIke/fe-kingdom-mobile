using Assets.Scripts.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class BattleStats
    {
        const int AGILITY_TO_DOUBLE = 0;
        const int BASIC_HIT_CHANCE = 70;
        const float FRONTAL_ATTACK_MOD = 1.5f;
        const int SURPRISE_BONUS_HIT = 20;

        private Unit owner;
        public float FrontalAttackModifier { get; set; }
        public int SurpriseAttackBonusHit { get; set; }

        public BattleStats(Unit owner)
        {
            this.owner = owner;
            FrontalAttackModifier = FRONTAL_ATTACK_MOD;
            SurpriseAttackBonusHit = SURPRISE_BONUS_HIT;
        }

        public bool CanKillTarget(Unit target, float attackMultiplier)
        {
            return GetDamageAgainstTarget(target, attackMultiplier) >= target.Stats.HP;
        }

        public int GetCrit()
        {
            int weaponCrit = 0;
            if (owner is Human)
            {
                Human c = (Human)owner;
                if (c.EquipedWeapon != null)
                    weaponCrit = c.EquipedWeapon.Crit;
            }
            return weaponCrit;
        }
        public int GetAttackCountAgainst(Unit c)
        {
            int attackCount = 1;
            if(owner.Stats.Speed - (c.Stats.Speed) > 0)
            {
                attackCount++;
            }
            if (owner.Stats.Speed - (c.Stats.Speed+5) >= 0)
            {
                attackCount++;
            }
            return attackCount;
        }
        public bool CanDoubleAttack(Unit c)
        {
            if (owner.Stats.Speed > c.Stats.Speed + AGILITY_TO_DOUBLE)
            {
                return true;
            }
            return false;
        }

        public int GetHitRate()
        {
            int hit = 0;
            if (owner is Human)
            {
                Human c = (Human)owner;
                if (c.EquipedWeapon != null)
                    hit = c.EquipedWeapon.Hit;
            }
            return BASIC_HIT_CHANCE + owner.Stats.Accuracy + hit;
        }

        public int GetHitAgainstTarget(Unit target)
        {
            return (int)Mathf.Clamp((GetHitRate() - target.Stats.Speed), 0, 100);
        }
        public int GetDamage(float attackModifier)
        {
            List<float> list = new List<float>();
            list.Add(attackModifier);
            return GetDamage(list);
        }
        public int GetDamage(List<float> attackModifier=null)
        {
            int weaponDamage = 0;
            if (owner is Human)
            {
                Human c = (Human)owner;
                if (c.EquipedWeapon != null)
                    weaponDamage = c.EquipedWeapon.Dmg;
            }
            int unmodifiedAttack = owner.Stats.Attack + weaponDamage;
            int attack = unmodifiedAttack;
            if(attackModifier != null)
            {
                foreach(float modi in attackModifier)
                {
                    attack += ((int)(unmodifiedAttack * modi))- unmodifiedAttack;
                }
            }
            return (int)Mathf.Clamp(attack, 0, Mathf.Infinity);
        }

        public int GetDamageAgainstTarget(Unit target, float atkMultiplier=1.0f)
        {
            return (int)(Mathf.Clamp(GetDamage(atkMultiplier) - target.Stats.Defense, 1, Mathf.Infinity));
        }
        public int GetDamageAgainstTarget(Unit target,List<float>atkMultiplier)
        {
            return (int)(Mathf.Clamp(GetDamage(atkMultiplier) - target.Stats.Defense, 1, Mathf.Infinity));
        }
        public int GetReceivedDamage(int damage, bool magic = false)
        {
            if(magic)
                return (int)Mathf.Clamp(damage, 1, Mathf.Infinity);
            else
                return (int)Mathf.Clamp(damage - owner.Stats.Defense, 1, Mathf.Infinity);
        }
        public int GetTotalDamageAgainstTarget(Unit target)
        {
            int attacks = 1;
            float multiplier = 1.0f;
            if (CanDoubleAttack(target))
                attacks = 2;
            return (int)(multiplier * attacks * Mathf.Clamp(GetDamage() - target.Stats.Defense, 0, Mathf.Infinity));
        }

        public bool IsFrontalAttack(Unit target)
        {
            int deltaX = owner.GridPosition.x - target.GridPosition.x;
            int deltaY = owner.GridPosition.y - target.GridPosition.y;
            if (deltaX != 0 && deltaY == 0)
            {
                if (deltaX == 1 && !target.GridPosition.FacingLeft)//Target right of attacker and facing right
                    return true;
                else if (deltaX == -1 && target.GridPosition.FacingLeft)
                    return true;
                return false;
            }
            return false;
        }
        public bool IsBackSideAttack(Unit target)
        {
            int deltaX = owner.GridPosition.x - target.GridPosition.x;
            int deltaY = owner.GridPosition.y - target.GridPosition.y;
            if (deltaX != 0 && deltaY == 0)
            {
                if (deltaX == 1 && target.GridPosition.FacingLeft)//Target right of attacker and facing left
                    return true;
                else if (deltaX ==-1 && !target.GridPosition.FacingLeft)
                    return true;
                return false;
            }
            return false;
        }

        public AttackData CreateAttackData(Unit target,DefenseType defenseType, AttackType attackType, List<float>attackMultipliers, List<AttackAttributes> attackAttributes)
        {
            float attackMultiplier = 1.0f;
            attackMultipliers.Add(attackType.DamageMultiplier);
            attackMultipliers.Add(defenseType.DamageMultiplier);

            if (attackAttributes.Contains(AttackAttributes.FrontalAttack)) {
                attackMultipliers.Add(FRONTAL_ATTACK_MOD);
            }
            foreach (float f in attackMultipliers)
            {
                attackMultiplier *= f;
            }
            int dmg = GetDamageAgainstTarget(target, attackMultiplier);
           
            int hitchance = GetHitAgainstTarget(target) + defenseType.Hit + attackType.Hit + (attackAttributes.Contains(AttackAttributes.SurpriseAttack) ? 0 : SurpriseAttackBonusHit);

            bool didConnect = UnityEngine.Random.Range(1, 101) <= hitchance;
            if (dmg >= target.Stats.HP)
            {
                attackAttributes.Add(AttackAttributes.Lethal);
            }

            AttackData attackData = new AttackData(this.owner,dmg,hitchance, didConnect,attackMultiplier, attackAttributes);
            return attackData;
        }
    }
}
