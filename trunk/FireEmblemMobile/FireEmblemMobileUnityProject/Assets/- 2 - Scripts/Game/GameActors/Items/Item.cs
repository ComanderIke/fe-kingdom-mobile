using System;
using System.Collections.Generic;
using Game.GameActors.Units.Humans;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Items
{
    public enum ItemTarget
    {
        Self,
        Ally,
        Enemy
    }
    [Serializable]
    public abstract class Item : ScriptableObject, ICloneable, ITargetableObject
    {
        public string Name;
        public string Description;
        public List<ItemMixin> Mixins;
        public int NumberOfUses;
        public int cost;

        [Header("ItemAttributes")] public Sprite Sprite;
       
        public ItemTarget target;

        public override string ToString()
        {
            return Name;
        }
        public string GetName()
        {
            return Name;
        }
        public string GetDescription()
        {
            return Description;
        }
        public Sprite GetIcon()
        {
            return Sprite;
        }
        public virtual void Use(Human character)
        {
            foreach (var mixin in Mixins) mixin.Use(character);
        }

        public object Clone()
        {
            var clone = (Item)MemberwiseClone();
            HandleCloned(clone);
            return clone;
        }
        protected virtual void HandleCloned(Item clone)
        {
            //Nothing particular in the base class, but maybe useful for children.
            //Not abstract so children may not implement this if they don't need to.
        }
    }
}