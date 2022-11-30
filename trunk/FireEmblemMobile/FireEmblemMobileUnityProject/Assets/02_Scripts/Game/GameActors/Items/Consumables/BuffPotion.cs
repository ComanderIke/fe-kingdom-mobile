using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class BuffPotion : ConsumableItem
    {
        public Buff buff;
        public BuffPotion(string name, string description, int cost, int rarity,Sprite sprite, ItemTarget target, Buff buff) : base(name, description, cost, rarity,sprite, target)
        {
            this.buff = buff;
        }
    }
}