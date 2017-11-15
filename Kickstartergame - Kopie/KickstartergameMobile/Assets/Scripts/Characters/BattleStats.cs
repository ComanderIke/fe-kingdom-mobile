using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class BattleStats
    {
        const int AGILITY_TO_DOUBLE = 0;
        private LivingObject owner;

        public BattleStats(LivingObject owner)
        {
            this.owner = owner;
        }

        public bool CanKillTarget(LivingObject target)
        {
            return GetDamageAgainstTarget(target) >= target.Stats.HP;
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

        public bool CanDoubleAttack(LivingObject c)
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
            return owner.Stats.Accuracy + hit;
        }

        public int GetHitAgainstTarget(LivingObject target)
        {
            return (int)Mathf.Clamp((GetHitRate() - target.Stats.Speed) * 10 + 50, 0, 100);
        }

        public int GetDamage()
        {
            int weaponDamage = 0;
            if (owner is Human)
            {
                Human c = (Human)owner;
                if (c.EquipedWeapon != null)
                    weaponDamage = c.EquipedWeapon.Dmg;
            }
            return (int)Mathf.Clamp((owner.Stats.Attack + weaponDamage), 0, Mathf.Infinity);
        }

        public int GetDamageAgainstTarget(LivingObject target)
        {
            float multiplier = 1.0f;
            return (int)(multiplier * Mathf.Clamp(GetDamage() - target.Stats.Defense, 1, Mathf.Infinity));
        }

        public int GetTotalDamageAgainstTarget(LivingObject target)
        {
            int attacks = 1;
            float multiplier = 1.0f;
            if (CanDoubleAttack(target))
                attacks = 2;
            return (int)(multiplier * attacks * Mathf.Clamp(GetDamage() - target.Stats.Defense, 0, Mathf.Infinity));
        }
    }
}
