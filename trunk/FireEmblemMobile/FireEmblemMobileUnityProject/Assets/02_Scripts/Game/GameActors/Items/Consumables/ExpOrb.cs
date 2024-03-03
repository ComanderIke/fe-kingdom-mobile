using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class ExpOrb : ConsumableItem
    {
        public int expvalue;
        public ExpOrb(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target, int expvalue) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
            this.expvalue = expvalue;
        }

        public override void Use(Unit character, Party convoy)
        {
            character.ExperienceManager.AddExp(expvalue);
            base.Use(character, convoy);
        }
    }
}