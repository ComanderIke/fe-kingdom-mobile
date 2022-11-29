using System;
using UnityEngine;

namespace Game.GameActors.Items
{
    [Serializable]
    public class Item : ICloneable, ITargetableObject
    {
        public string Name;
        public string Description;
        public int cost;

        [Header("ItemAttributes")] public Sprite Sprite;

        public Item(string name, string description, int cost, Sprite sprite)
        {
            this.Name = name;
            this.Description = description;
            this.cost = cost;
            this.Sprite = sprite;
            
        }

       
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            Debug.Log("Compare");
            if (obj is Item item)
            {
                Debug.Log("Compare Items"+item.Name +" "+Name);
                if (item.Name == Name)
                    return true;
                Debug.Log("Not Equal");
            }
            
            return base.Equals(obj);
        }

        protected bool Equals(Item other)
        {
            return Name == other.Name && Description == other.Description && cost == other.cost && Equals(Sprite, other.Sprite);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ cost;
                hashCode = (hashCode * 397) ^ (Sprite != null ? Sprite.GetHashCode() : 0);
                return hashCode;
            }
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