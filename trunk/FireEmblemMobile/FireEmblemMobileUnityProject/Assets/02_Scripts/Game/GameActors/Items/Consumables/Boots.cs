using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class Boots : ConsumableItem
    {
        public int moveIncrease;
        public Boots(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target, int moveIncrease) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
            this.moveIncrease = moveIncrease;
        }

        public override void Use(Unit character, Party convoy)
        {
            character.Stats.Mov += moveIncrease;
            base.Use(character, convoy);
        }
    }
}