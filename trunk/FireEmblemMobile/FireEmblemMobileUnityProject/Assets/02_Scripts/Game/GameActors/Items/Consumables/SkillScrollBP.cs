using System;
using Game.GameActors.Items;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/SkillScroll", fileName = "SkillScroll")]
    public class SkillScrollBP : ConsumableItemBp
    {
        [SerializeField]public SkillBp learntSkill;
        public override Item Create()
        {
            return new SkillScroll(learntSkill.Create(),name, description, cost, rarity, maxStack, sprite, target);
        }
    }
}
