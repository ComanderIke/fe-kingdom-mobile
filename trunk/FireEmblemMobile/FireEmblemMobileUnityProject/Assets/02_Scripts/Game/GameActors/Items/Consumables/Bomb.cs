using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class Bomb : ConsumableItem, IEquipableCombatItem
    {
        public Skill skill;
        
        public Bomb(Skill skill, string name, string description, int cost, int rarity,int maxStack,Sprite sprite) : base(name, description, cost,rarity, maxStack,sprite, ItemTarget.Position)
        {
            this.skill = skill;
            
        }
    }
}