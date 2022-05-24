using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.WorldMapStuff.Model;

namespace Game.GameActors.Items
{
    public enum ItemTarget
    {
        Self,
        Ally,
        Enemy
    }
    [Serializable]
    public abstract class ConsumableItem:Item{
        public List<ItemMixin> Mixins;
        public int NumberOfUses;
        public ItemTarget target;
        
        public virtual void Use(Unit character, Convoy convoy)
        {
            foreach (var mixin in Mixins) mixin.Use(character);
            convoy.RemoveItem(this);
        }
    }
}