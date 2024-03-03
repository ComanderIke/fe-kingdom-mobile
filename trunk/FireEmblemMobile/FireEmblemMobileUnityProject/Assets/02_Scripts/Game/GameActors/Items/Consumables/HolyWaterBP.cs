using System;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/Holy Water", fileName = "Holy Water")]
    public class HolyWaterBP : ConsumableItemBp
    {
        public override Item Create()
        {
            return new HolyWater(name, description, cost, rarity,maxStack,sprite, target);

        }
    }
}