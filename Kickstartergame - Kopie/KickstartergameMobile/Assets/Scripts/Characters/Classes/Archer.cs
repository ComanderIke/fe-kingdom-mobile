using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Characters.Classes;
using Assets.Scripts.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters
{
    class Archer : CharClass
    {
        public Archer()
        {
			base.weaponType.Add(WeaponScript.bow);

            //2 aktive und 2 passive Skills
            stats = new Stats(17, 4, 6, 4, 7, 1);
            //moveRange

            AttackRanges.Add(2);
            AttackRanges.Add(3);
            //AttackRanges.Add(4);
            hpgrowth = 15;
            speedgrowth = 15;
            accuracygrowth = 25;
            attackgrowth = 15;
            defensegrowth = 15;
			spiritgrowth = 10;
        }
    }
}
