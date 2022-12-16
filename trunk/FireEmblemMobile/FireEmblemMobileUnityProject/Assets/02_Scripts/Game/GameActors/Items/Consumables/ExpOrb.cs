using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class ExpOrb : ConsumableItem
    {
        public int expvalue;
        public ExpOrb(string name, string description, int cost,int rarity, Sprite sprite, ItemTarget target, int expvalue) : base(name, description, cost, rarity,sprite, target)
        {
            this.expvalue = expvalue;
        }

        public override void Use(Unit character, Convoy convoy)
        {
            character.ExperienceManager.AddExp(expvalue);
            base.Use(character, convoy);
        }
    }
}