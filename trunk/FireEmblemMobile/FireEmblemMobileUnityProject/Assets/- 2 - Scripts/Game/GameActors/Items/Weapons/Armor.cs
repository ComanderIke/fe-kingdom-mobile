using System;
using Game.GameActors.Units.Humans;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Armor", fileName = "Armor")]
    public class Armor : EquipableItem
    {

        public int Weight;
        public int armor;

        //public List<WeaponMixin> WeaponMixins;


        public override void Use(Human character)
        {
            base.Use(character);
        }
        
       
    }
}