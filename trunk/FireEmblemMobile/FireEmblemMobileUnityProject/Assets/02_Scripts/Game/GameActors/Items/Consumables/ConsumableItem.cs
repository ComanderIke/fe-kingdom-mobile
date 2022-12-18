using System;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items
{
    [Serializable]
    public abstract class ConsumableItem:Item{
        public ItemTarget target;
        
        public virtual void Use(Unit character, Convoy convoy)
        {
            convoy.RemoveItem(this);
        }

        protected ConsumableItem(string name, string description, int cost,int rarity,int maxStack, Sprite sprite, ItemTarget target) : base(name, description, cost,rarity, maxStack,sprite)
        {
            this.target = target;
        }
    }
}