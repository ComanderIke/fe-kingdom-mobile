using System;
using Game.GameActors.Units.Skills.Base;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    public abstract class ConsumableItemBp:ItemBP{
        public ItemTarget target;
        public SkillBp skill;
    }
}