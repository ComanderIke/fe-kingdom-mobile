﻿using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class BuffPotion : ConsumableItem, IEquipableCombatItem
    {
        public EncounterBasedBuff buff;
        public BuffPotion(string name, string description, int cost, int rarity,int maxStack,Sprite sprite, ItemTarget target, EncounterBasedBuff buff) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
            this.buff = buff;
        }

        public override void Use(Unit character, Party convoy)
        {
            character.ApplyEncounterBuff(buff);
            base.Use(character, convoy);
        }
    }
}