using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    class Battle
    {
        public const int BonusDamage = 2;
        public const int BonusHit = 10;
        public static Boolean IsEffectiveAgainst(WeaponCategory weapon1, WeaponCategory weapon2)
        {
           // if (weapon1.effectiveAgainst.Contains(weapon2.type))
            //    return true;
            return false;
        }
    }
}
