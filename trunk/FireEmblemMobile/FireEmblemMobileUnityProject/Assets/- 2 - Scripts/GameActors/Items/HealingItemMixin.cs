using System;
using Assets.GameActors.Units;
using UnityEngine;

namespace Assets.GameActors.Items
{
    [CreateAssetMenu(menuName = "GameData/Items/Mixins/Healing", fileName = "Healing")]
    public class HealingItemMixin : ItemMixin
    {
        public override void Use(Unit character)
        {
            throw new NotImplementedException();
        }
    }
}