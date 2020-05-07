using System;
using Assets.GameActors.Units;
using UnityEngine;

namespace Assets.GameActors.Items.Weapons
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