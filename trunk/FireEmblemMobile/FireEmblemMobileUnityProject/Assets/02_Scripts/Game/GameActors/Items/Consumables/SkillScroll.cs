using System.Net;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class SkillScroll : ConsumableItem
    {
        private Skill skill;
        public SkillScroll(Skill learntSkill,string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
            this.skill = learntSkill;
        }

        public override void Use(Unit character, Party convoy)
        {
            character.SkillManager.LearnSkill(skill);
            //ServiceProvider.Instance.GetSystem<UnitProgressSystem>().LearnNewSkill(character);
            base.Use(character, convoy);
        }
    }
}