using Game.GameActors.Units.Skills;
using UnityEngine;

namespace Game.GameActors.Items.Gems
{
    [System.Serializable]
    public class Stone : Item
    { 
        [SerializeField]
        private GemType gemType;

        private bool inserted = false;
        private Stone upgradeTo;
        public Stone(string name, string description, int cost, int maxStack,Sprite sprite, int rarity,  Stone upgradeTo) : base(name, description, cost, rarity,maxStack,sprite)
        {
           
            this.upgradeTo = upgradeTo;
        }

        // public GemType GetGemType(GemType gemType)
        // {
        //     return gemType;
        // }
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

        public Stone GetUpgradedGem()
        {
            return upgradeTo;
        }

        public bool HasUpgrade()
        {
            return upgradeTo != null;
        }
    }
}