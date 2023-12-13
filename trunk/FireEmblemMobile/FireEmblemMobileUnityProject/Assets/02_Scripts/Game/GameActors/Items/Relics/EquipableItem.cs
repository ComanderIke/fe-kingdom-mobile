using System;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public abstract class EquipableItem : Item
    {
   
        public EquipableItem(string name, string description, int cost,int rarity,int maxStack, Sprite sprite, Skill skill) : base(name, description, cost,rarity, maxStack,sprite)
        {
            this.Skill = skill;
        }

        public Skill Skill { get; set; }
        
    }
}