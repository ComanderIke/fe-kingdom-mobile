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
        private Gem upgradeTo;
        public Gem(string name, string description, int cost, int maxStack,Sprite sprite, int rarity, GemType gemType, Gem upgradeTo) : base(name, description, cost, rarity,maxStack,sprite)
        {
            
            this.gemType = gemType;
            this.upgradeTo = upgradeTo;
        }

        public GemType GetGemType(GemType gemType)
        {
            return gemType;
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

        public Gem GetUpgradedGem()
        {
            return upgradeTo;
        }

        public bool HasUpgrade()
        {
            return upgradeTo != null;
        }
    }
}