using System;
using System.Collections.Generic;
using Game.GameActors.Units.Humans;
using UnityEngine;

namespace Game.GameActors.Items
{
    [Serializable]
    public abstract class Item : ScriptableObject, ICloneable
    {
        public string Description;
        public List<ItemMixin> Mixins;
        public int NumberOfUses;
        public int cost;

        [Header("ItemAttributes")] public Sprite Sprite;

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