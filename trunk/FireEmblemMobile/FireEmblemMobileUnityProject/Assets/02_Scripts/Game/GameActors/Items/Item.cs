using System;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Items
{
    

    [Serializable]
    public abstract class Item : ScriptableObject, ICloneable, ITargetableObject
    {
        public string Name;
        public string Description;
        public int cost;

        [Header("ItemAttributes")] public Sprite Sprite;
        

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