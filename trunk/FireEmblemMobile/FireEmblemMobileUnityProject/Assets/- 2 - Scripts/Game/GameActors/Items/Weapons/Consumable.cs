using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public enum ItemEffect
    {
        Heal,
        Damage
    }
   
    public abstract class Consumable : EquipableItem
    {

        
        //public int Weight;
        //public int armor;

        //public List<WeaponMixin> WeaponMixins;


        public virtual void Use(Unit character, Convoy convoy)
        {
            convoy.RemoveItem(this);
        }
    }
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumable", fileName = "HealthPotion")]
    public class Healthpotion : Consumable
    {
        public int strength;
        public override void Use(Unit character, Convoy convoy)
        {
            character.Heal(strength);
            base.Use(character, convoy);
        }
    }
    
}