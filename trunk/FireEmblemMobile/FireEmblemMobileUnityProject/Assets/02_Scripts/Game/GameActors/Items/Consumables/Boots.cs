using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class Boots : ConsumableItem
    {
        public int moveIncrease;
        public Boots(string name, string description, int cost,int rarity, Sprite sprite, ItemTarget target, int moveIncrease) : base(name, description, cost, rarity,sprite, target)
        {
            this.moveIncrease = moveIncrease;
        }

        public override void Use(Unit character, Convoy convoy)
        {
            character.Stats.Mov += moveIncrease;
            base.Use(character, convoy);
        }
    }
}