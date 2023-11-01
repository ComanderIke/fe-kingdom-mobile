using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class HealthPotion:ConsumableItem, IEquipableCombatItem
    {
        public int strength;
        public GameObject healEffect;
        public bool percentage;

        public HealthPotion(string name, string description, int cost, int rarity, int maxStack,Sprite sprite, ItemTarget target, int strength, GameObject healEffect, bool percentage):base (name, description, cost, rarity, maxStack,sprite, target)
        {
            this.strength = strength;
            this.healEffect = healEffect;
            this.percentage = percentage;
        }
        public override void Use(Unit character, Party convoy)
        {
            Debug.Log("Use Healthpotion!");
            if (percentage)
            {
                character.Heal(strength, true);
            }
            else
                character.Heal(strength);
            //Different Animation in GridBattle
            if(character.GameTransformManager.GameObject!=null)
                GameObject.Instantiate(healEffect, character.GameTransformManager.GetCenterPosition(), Quaternion.identity,null);
            
            base.Use(character, convoy);
        }
    }
}