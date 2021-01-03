using System;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items
{
    [Serializable]
    public abstract class ItemMixin : ScriptableObject
    {
        public string Name;
        public abstract void Use(Unit character);
    }
}