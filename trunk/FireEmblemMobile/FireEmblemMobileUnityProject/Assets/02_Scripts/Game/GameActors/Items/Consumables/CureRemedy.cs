using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class CureRemedy : ConsumableItem, IEquipableCombatItem
    {
        public CureRemedy(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
       
        }

        public override void Use(Unit character, Party convoy)
        {
            character.RemoveDebuffs();
            base.Use(character, convoy);
        }
    }
}