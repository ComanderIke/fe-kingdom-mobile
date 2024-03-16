using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class ResurrectScroll : ConsumableItem, IEquipableCombatItem
    {
        private Skill skill;
        public ResurrectScroll(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target, Skill skill) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
            this.skill = skill;
        }

        public override void Use(Unit character, Party convoy)
        {
            ((SelfTargetSkillMixin)skill.FirstActiveMixin).Activate(character);
           
            base.Use(character, convoy);
        }
    }
}