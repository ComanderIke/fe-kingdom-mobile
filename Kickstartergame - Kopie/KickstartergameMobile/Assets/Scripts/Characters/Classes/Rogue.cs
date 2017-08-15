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
    public class Rogue : CharClass
    {


        public Rogue()
        {
			base.weaponType.Add( WeaponScript.dagger);
            stats = new Stats(18, 4, 10, 3, 9, 3);
            movRange = 5;
            //moveRange

            AttackRanges.Add(1);
            hpgrowth = 15;
            speedgrowth = 25;
            accuracygrowth = 20;
            attackgrowth = 15;
            defensegrowth = 10;
			spiritgrowth = 5;
        }


    }
}
