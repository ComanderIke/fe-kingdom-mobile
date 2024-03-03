using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Items
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/item", fileName = "Item")]
    public class ItemBP : ScriptableObject
    {
        [FormerlySerializedAs("Name")] public new string name;
        [FormerlySerializedAs("Description")] public string description;
        public int cost;

        public int maxStack = 1;
        [Range(1,3)]
        public int rarity=1;
        [FormerlySerializedAs("Sprite")] [Header("ItemAttributes")] public Sprite sprite;

        public virtual Item Create()
        {
            return new Item(name, description, cost, rarity, maxStack, sprite);
        }

        public int GetRarity()
        {
            return rarity;
        }
    }
}