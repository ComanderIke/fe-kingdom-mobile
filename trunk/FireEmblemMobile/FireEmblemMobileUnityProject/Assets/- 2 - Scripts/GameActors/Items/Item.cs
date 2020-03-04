using System;
using System.Collections.Generic;
using Assets.GameActors.Units.Humans;
using UnityEngine;

namespace Assets.GameActors.Items
{
    [Serializable]
    public abstract class Item : ScriptableObject
    {
        public string Description;
        public List<ItemMixin> Mixins;
        public int NumberOfUses;

        [Header("ItemAttributes")] public Sprite Sprite;

        public virtual void Use(Human character)
        {
            foreach (var mixin in Mixins) mixin.Use(character);
        }
    }
}