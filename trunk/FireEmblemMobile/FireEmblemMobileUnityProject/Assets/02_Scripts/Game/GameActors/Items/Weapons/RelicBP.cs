using System;
using System.Linq;
using Game.GameActors.Units.Humans;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Relic", fileName = "Relic")]
    public class RelicBP : EquipableItemBP
    {
        [Header("RelicAttributes")] public UpgradeAttributes[] Attributes;
        public int Level = 1;
        public int maxLevel = 5;

        public int slotCount=1;

        public override Item Create()
        {
            return new Relic(name, description, cost, rarity,maxStack,sprite, equipmentSlotType, Level, maxLevel, slotCount);
        }
    }
}