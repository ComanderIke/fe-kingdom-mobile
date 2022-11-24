using System;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Items
{
    [Serializable]
    public abstract class ItemBP : ScriptableObject
    {
        [FormerlySerializedAs("Name")] public new string name;
        [FormerlySerializedAs("Description")] public string description;
        public int cost;

        [FormerlySerializedAs("Sprite")] [Header("ItemAttributes")] public Sprite sprite;
        
        public abstract Item Create();
    }
}