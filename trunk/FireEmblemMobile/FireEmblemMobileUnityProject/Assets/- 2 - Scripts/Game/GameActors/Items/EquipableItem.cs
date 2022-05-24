using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/EquipableItem", fileName = "EquipableItem")]
    public class EquipableItem : Item
    {
        public EquipmentSlotType EquipmentSlotType;

    }

    public enum EquipmentSlotType
    {
        Relic,
        Weapon
    }
}