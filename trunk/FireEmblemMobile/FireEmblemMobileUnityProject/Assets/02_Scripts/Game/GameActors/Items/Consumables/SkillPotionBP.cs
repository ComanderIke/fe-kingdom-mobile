using System;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/Skillpotion", fileName = "Skillpotion")]
    public class SkillPotionBP : ConsumableItemBp
    {
        public int strength;
        public GameObject effect;

        public override Item Create()
        {
            return new SkillPotion(name, description, cost, rarity, maxStack,sprite, target, strength, effect);

        }
    }
}