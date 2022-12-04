using Game.GameActors.Items.Weapons;
using UnityEngine;

namespace Game.GameActors.Items.Gems
{
    [System.Serializable]
    public class Gem : Item
    { 
        [SerializeField]
        private GemType gemType;

        private bool inserted = false;
        public Gem(string name, string description, int cost, Sprite sprite, int rarity, GemType gemType) : base(name, description, cost, rarity,sprite)
        {
            
            this.gemType = gemType;
        }

        public void Insert()
        {
            inserted = true;
        }
        public void Remove()
        {
            inserted = false;
        }
        public bool IsInserted()
        {
            return inserted;
        }
    }
}