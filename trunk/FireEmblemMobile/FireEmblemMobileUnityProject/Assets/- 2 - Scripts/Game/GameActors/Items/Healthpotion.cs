using System;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/HealthPotion", fileName = "HealthPotion")]
    public class Healthpotion : ConsumableItem
    {
        public int strength;
        public GameObject healEffect;
        public override void Use(Unit character, Convoy convoy)
        {
            Debug.Log("Use Healthpotion!");
            character.Heal(strength);
            Instantiate(healEffect, character.GameTransformManager.GetCenterPosition(), Quaternion.identity,null);
            
            base.Use(character, convoy);
        }
    }
}