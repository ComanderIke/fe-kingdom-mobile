using System;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/MoralityModifier", fileName = "MoralityModifier")]
    public class MoralityPotionBp : ConsumableItemBp
    {
        public int strength;
        public GameObject effect;
        public bool resetMorality;

        public override Item Create()
        {
            return new MoralityPotion(name, description, cost, rarity, maxStack, sprite, target, strength, effect,
                resetMorality);
        }
    }
}