using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class ResurrectScroll : ConsumableItem, IEquipableCombatItem
    {
        public ResurrectScroll(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
        }

        public override void Use(Unit character, Party convoy)
        {
            Unit unitToRevive =Player.Player.Instance.Party.deadMembers[ Player.Player.Instance.Party.deadMembers.Count-1];
            Player.Player.Instance.Party.ReviveCharacter(unitToRevive);
            base.Use(character, convoy);
        }
    }
}