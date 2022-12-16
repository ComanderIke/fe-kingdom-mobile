using System;
using Game.GameActors.Units.Numbers;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/Holy Water", fileName = "Holy Water")]
    public class HolyWaterBP : ConsumableItemBp
    {
        public override Item Create()
        {
            return new HolyWater(name, description, cost, rarity,sprite, target);

        }
    }
}