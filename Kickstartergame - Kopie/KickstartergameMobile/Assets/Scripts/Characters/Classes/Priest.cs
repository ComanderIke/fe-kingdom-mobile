using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Characters.Classes;
using Assets.Scripts.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyCSharp;

namespace Assets.Scripts.Characters
{
    class Priest:CharClass
    {
        public Priest()
        {
			base.weaponType.Add(WeaponScript.staff);

            stats = new Stats(16, 3, 4, 2, 5, 6);
            //moveRange
            hpgrowth = 10;
            speedgrowth = 10;
            accuracygrowth = 10;
            attackgrowth = 15;
            defensegrowth = 5;
			spiritgrowth = 25;
        }
    }
}
