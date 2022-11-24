using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public abstract class EquipableItemBP : ItemBP
    {
        [FormerlySerializedAs("EquipmentSlotType")] public EquipmentSlotType equipmentSlotType;

    }
    
    public enum EquipmentSlotType
    {
        Relic,
        Weapon
    }
}