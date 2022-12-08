using System;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/BuffPotion", fileName = "BuffPotion")]
    public class BuffPotionBP : ConsumableItemBp
    {
        public EncounterBasedBuffBP buff;
        public override Item Create()
        {
            return new BuffPotion(name, description, cost, rarity, sprite, target, buff.Create());

        }
    }
}