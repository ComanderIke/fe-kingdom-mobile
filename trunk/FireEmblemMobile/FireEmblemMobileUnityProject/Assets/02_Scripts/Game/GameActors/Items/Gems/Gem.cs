using UnityEngine;

namespace Game.GameActors.Items.Gems
{
    public class Gem : Item
    { 
        private GemType gemType;
        public Gem(string name, string description, int cost, Sprite sprite, int rarity, GemType gemType) : base(name, description, cost, rarity,sprite)
        {
            
            this.gemType = gemType;
        }
    }
}