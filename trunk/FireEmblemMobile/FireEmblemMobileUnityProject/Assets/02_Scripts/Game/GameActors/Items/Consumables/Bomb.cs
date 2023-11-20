using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Skills;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;

namespace _02_Scripts.Game.GameActors.Items.Consumables
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