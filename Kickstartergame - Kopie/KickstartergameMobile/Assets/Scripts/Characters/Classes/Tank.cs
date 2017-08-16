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
    public class Tank : CharClass
    {
        public Tank()
        {
			base.weaponType.Add(WeaponScript.sword);
			base.weaponType.Add(WeaponScript.axe);

            stats = new Stats(23, 8, 2, 7, 3, 1);
            movRange = 3;
            AttackRanges.Add(1);
            hpgrowth = 25;
            speedgrowth = 5;
            accuracygrowth = 10;
            attackgrowth = 20;
            defensegrowth = 25;
			spiritgrowth = 10;
  
        }

    }
}
