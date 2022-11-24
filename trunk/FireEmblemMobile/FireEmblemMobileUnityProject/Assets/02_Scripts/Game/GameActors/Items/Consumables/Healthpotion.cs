﻿using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class HealthPotion:ConsumableItem
    {
        public int strength;
        public GameObject healEffect;

        public HealthPotion(string name, string description, int cost, Sprite sprite, ItemTarget target, int strength, GameObject healEffect):base (name, description, cost, sprite, target)
        {
            this.strength = strength;
            this.healEffect = healEffect;
        }
        public override void Use(Unit character, Convoy convoy)
        {
            Debug.Log("Use Healthpotion!");
            character.Heal(strength);
            GameObject.Instantiate(healEffect, character.GameTransformManager.GetCenterPosition(), Quaternion.identity,null);
            
            base.Use(character, convoy);
        }
    }
}