using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/SkillScroll", fileName = "SkillScroll")]
    public class SkillScrollBP : ConsumableItemBp
    {
        [SerializeField]public List<SkillBp> skillPool;
        public override Item Create()
        {
            return new SkillScroll(skillPool,name, description, cost, rarity, maxStack, sprite, target);
        }
    }
}
