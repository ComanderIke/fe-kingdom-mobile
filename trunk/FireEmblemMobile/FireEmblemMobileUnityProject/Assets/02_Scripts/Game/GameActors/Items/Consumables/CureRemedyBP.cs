using System;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/CureRemedy", fileName = "CureRemedy")]
    public class CureRemedyBP : ConsumableItemBp
    {
        public override Item Create()
        {
            return new CureRemedy(name, description, cost, rarity,maxStack,sprite, target);

        }
    }
}