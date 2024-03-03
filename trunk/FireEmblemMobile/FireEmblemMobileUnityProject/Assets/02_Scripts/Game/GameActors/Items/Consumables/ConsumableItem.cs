using System;
using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    public abstract class ConsumableItem:Item{
        public ItemTarget target;
        
        public virtual void Use(Unit character, Party party)
        {
            party.RemoveItem(this);
        }

        protected ConsumableItem(string name, string description, int cost,int rarity,int maxStack, Sprite sprite, ItemTarget target) : base(name, description, cost,rarity, maxStack,sprite)
        {
            this.target = target;
        }
    }
}