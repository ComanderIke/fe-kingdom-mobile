using System;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/ExpOrb", fileName = "ExpOrb")]
    public class ExpOrbBP : ConsumableItemBp
    {
        public int expvalue;
        public override Item Create()
        {
            return new ExpOrb(name, description, cost, rarity,maxStack,sprite, target, expvalue);

        }
    }
}