using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
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
