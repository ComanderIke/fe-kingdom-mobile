using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Classes
{
    
    [System.Serializable]
    public abstract class CharClass
    {

        public List<Skill> skills;
        public List<WeaponCategory> weaponType;
        public Stats stats;
        public List<int> AttackRanges;
        public int movRange = 4;
        public int hpgrowth = 0;
        public int speedgrowth = 0;
        public int accuracygrowth = 0;
        public int attackgrowth = 0;
        public int defensegrowth = 0;
		public int spiritgrowth = 0;

        public CharClass()
        {
			weaponType = new List<WeaponCategory> ();
            AttackRanges = new List<int>();
            skills = new List<Skill>();
        }

    }
}
