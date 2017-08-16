using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Characters.Classes;
using Assets.Scripts.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters
{
    class Hellebardier : CharClass
    {
        public Hellebardier()
        {
			base.weaponType.Add( WeaponScript.lance);

            //skills.Add(new Counter());
            //2 aktive und 2 passive Skills
            stats = new Stats(20, 6, 6, 4, 6, 2);
            //moveRange
            AttackRanges.Add(1);
           // AttackRanges.Add(2);
            hpgrowth = 20;
            speedgrowth = 15;
            accuracygrowth = 15;
            attackgrowth = 15;
            defensegrowth = 15;
			spiritgrowth = 15;

        }
    }
}
