using System;
using Assets.GameActors.Units;
using UnityEngine;

namespace Assets.GameActors.Items
{
    [Serializable]
    public abstract class ItemMixin : ScriptableObject
    {
        public string Name;
        public abstract void Use(Unit character);
    }
}