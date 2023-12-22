﻿using System.Net;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class ResurrectScroll : ConsumableItem, IEquipableCombatItem
    {
        public ResurrectScroll(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
        }

        public override void Use(Unit character, Party convoy)
        {
            Unit unitToRevive =Player.Instance.Party.deadMembers[ Player.Instance.Party.deadMembers.Count-1];
            Player.Instance.Party.ReviveCharacter(unitToRevive);
            base.Use(character, convoy);
        }
    }
}