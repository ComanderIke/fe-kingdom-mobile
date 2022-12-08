using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class BuffPotion : ConsumableItem
    {
        public EncounterBasedBuff buff;
        public BuffPotion(string name, string description, int cost, int rarity,Sprite sprite, ItemTarget target, EncounterBasedBuff buff) : base(name, description, cost, rarity,sprite, target)
        {
            this.buff = buff;
        }

        public override void Use(Unit character, Convoy convoy)
        {
            character.ApplyEncounterBuff(buff);
            base.Use(character, convoy);
        }
    }
}