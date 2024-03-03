using System;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/Boots", fileName = "Boots")]
    public class BootsBP : ConsumableItemBp
    {
        public int movIncrease;
        public override Item Create()
        {
            return new Boots(name, description, cost, rarity,maxStack,sprite, target, movIncrease);

        }
    }
}