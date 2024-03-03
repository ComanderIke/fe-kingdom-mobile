using System;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/ResurectScroll", fileName = "ResurectScroll")]
    public class ResurrectScrollBP : ConsumableItemBp
    {
        
        public override Item Create()
        {
            return new ResurrectScroll(name, description, cost, rarity,maxStack,sprite, target);
        }
    }
}