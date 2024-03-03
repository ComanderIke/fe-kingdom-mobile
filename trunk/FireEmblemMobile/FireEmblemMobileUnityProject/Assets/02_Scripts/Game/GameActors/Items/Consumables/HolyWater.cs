using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class HolyWater : ConsumableItem, IEquipableCombatItem
    {
        public HolyWater(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
       
        }

        public static bool RemoveAll { get; set; }


        public override void Use(Unit character, Party convoy)
        {
            character.RemoveCurse(character.Curses.GetRandomElement());
            if(RemoveAll)
            {
                for (int i=character.Curses.Count;i>=0; i--)
                    character.RemoveCurse(character.Curses[i]);
            }
           
            base.Use(character, convoy);
        }
    }
}