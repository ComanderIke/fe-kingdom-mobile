using System;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/HealthPotion", fileName = "HealthPotion")]
    public class HealthpotionBP : ConsumableItemBp
    {
        public int strength;
        public GameObject healEffect;
        public bool percentage;

        public override Item Create()
        {
            return new HealthPotion(name, description, cost, rarity, maxStack,sprite, target, strength, healEffect, percentage);

        }
    }
}