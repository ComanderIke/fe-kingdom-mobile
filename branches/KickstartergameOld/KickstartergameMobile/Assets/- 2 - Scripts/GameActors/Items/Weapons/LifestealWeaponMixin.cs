using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.Items.Weapons
{
    [CreateAssetMenu(menuName = "GameData/Weapons/Mixins/LifeSteal", fileName = "Lifesteal")]
    public class LifestealWeaponMixin : WeaponMixin
    {
        public override void OnAttack(Unit attacker, Unit defender)
        {
            throw new NotImplementedException();
        }
    }
}
