using System;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/SkillScroll", fileName = "SkillScroll")]
    public class SkillScrollBP : ConsumableItemBp
    {
        
        public override Item Create()
        {
            return new SkillScroll(name, description, cost, rarity,maxStack,sprite, target);
        }
    }
}