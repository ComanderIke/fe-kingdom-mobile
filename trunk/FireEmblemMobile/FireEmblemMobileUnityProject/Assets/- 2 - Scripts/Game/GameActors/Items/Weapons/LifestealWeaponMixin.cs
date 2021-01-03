using System;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
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