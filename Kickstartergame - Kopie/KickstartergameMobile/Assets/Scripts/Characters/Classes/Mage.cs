using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Characters.Classes;
using Assets.Scripts.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters
{
    [System.Serializable]
    public class Mage : CharClass
    {

        public Mage()
        {
			base.weaponType.Add(WeaponScript.magic);

            stats = new Stats(15, 4, 5, 2, 5, 4);
            AttackRanges.Add(1);
            AttackRanges.Add(2);
            hpgrowth = 10;
            speedgrowth = 15;
            accuracygrowth = 10;
            attackgrowth = 20;
            defensegrowth = 5;
			spiritgrowth = 20;
      
        }
    }
}
