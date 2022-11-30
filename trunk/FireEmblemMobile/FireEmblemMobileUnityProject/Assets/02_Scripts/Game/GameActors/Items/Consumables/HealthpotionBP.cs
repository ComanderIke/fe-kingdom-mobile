using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/HealthPotion", fileName = "HealthPotion")]
    public class HealthpotionBP : ConsumableItemBp
    {
        public int strength;
        public GameObject healEffect;

        public override Item Create()
        {
            return new HealthPotion(name, description, cost, rarity, sprite, target, strength, healEffect);

        }
    }
}