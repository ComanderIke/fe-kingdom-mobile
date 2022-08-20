using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public abstract class EquipableItem : Item
    {
        public EquipmentSlotType EquipmentSlotType;

        public abstract int GetUpgradeCost();

        public abstract int GetUpgradeSmithingStoneCost();
    }

    public enum EquipmentSlotType
    {
        Relic,
        Weapon
    }
}