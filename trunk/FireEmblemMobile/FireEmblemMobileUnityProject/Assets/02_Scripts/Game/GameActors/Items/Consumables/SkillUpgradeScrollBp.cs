using System;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using UnityEngine;

namespace _02_Scripts.Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/SkillUpgradeScroll", fileName = "SkillUpgradeScroll")]
    public class SkillUpgradeScrollBp : ConsumableItemBp
    {
        [SerializeField] bool random = true;

        public override Item Create()
        {
            return new SkillUpgradeScroll(random, name, description, cost, rarity,maxStack,sprite, target);
        }
    }
}