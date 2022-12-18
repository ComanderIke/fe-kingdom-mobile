using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class SkillScroll : ConsumableItem
    {
        public SkillScroll(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
        }

        public override void Use(Unit character, Convoy convoy)
        {
            Debug.Log("TODO Use SKillScrolls");
            base.Use(character, convoy);
        }
    }
}