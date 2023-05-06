using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Numbers;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class HolyWater : ConsumableItem
    {
        public HolyWater(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
       
        }

        public override void Use(Unit character, Convoy convoy)
        {
            character.RemoveCurse();
            base.Use(character, convoy);
        }
    }
}