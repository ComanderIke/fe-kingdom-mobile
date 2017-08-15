using System;
using Assets.Scripts.Characters.Debuffs;

namespace Assets.Scripts.Characters.Skills
{
    internal class ArmorReduction : Debuff
    {
        private int armorReduction;
        private bool firsttime = true;

        public ArmorReduction(int armorReduction)
        {
            this.armorReduction = armorReduction;
        }

        public override bool TakeEffect(Character c)
        {
            if (currduration > 0)
            {
                base.currduration -= 1;
            }
            else
            {
                return true;
            }
            return false;
        }
        public void Start(Character c)
        {
            c.stats.defense -= armorReduction;
        }
        public override void End(Character c)
        {
            c.stats.defense += armorReduction;
            c.Debuffs.Remove(this);
        }
    }
}