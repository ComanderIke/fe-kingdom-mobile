using System.Collections.Generic;
using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills.Base;
using Game.Manager;
using Game.Systems;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class SkillScroll : ConsumableItem
    {
        private List<SkillBp> skillPool;
        public SkillScroll(List<SkillBp> skillPool,string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
            this.skillPool = skillPool;
            
        }

        public override void Use(Unit character, Party convoy)
        {
            if(skillPool.Count==0)
                ServiceProvider.Instance.GetSystem<UnitProgressSystem>().LearnNewSkill(character);
            else
                ServiceProvider.Instance.GetSystem<UnitProgressSystem>().LearnNewSkill(character, skillPool);
            //character.SkillManager.LearnSkill(skill);
            //ServiceProvider.Instance.GetSystem<UnitProgressSystem>().LearnNewSkill(character);
            base.Use(character, convoy);
        }
    }
}