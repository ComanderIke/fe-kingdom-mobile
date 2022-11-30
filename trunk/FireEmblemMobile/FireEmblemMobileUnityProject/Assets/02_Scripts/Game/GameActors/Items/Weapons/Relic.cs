using System;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class Relic:EquipableItem
    {
        [Header("RelicAttributes")] public UpgradeAttributes[] Attributes;
        public int Level = 1;

        public int maxLevel = 5;

        public Relic(string name, string description, int cost, Sprite sprite, EquipmentSlotType slotType, int level, int maxLevel) : base(name, description, cost, sprite, slotType)
        {
            this.Level = level;
            this.maxLevel = maxLevel;
        }


        public string GetAttributeDescription()
        {
            if(Attributes!=null&&Attributes.Length>Level-1)
                if(Attributes[Level -1].effect!=null)
                    return Attributes[Level -1].effect.GetDescription();
            return "No Description?!";
        }

        public string GetUpgradeAttributeDescription()
        {
            if (Attributes == null)
                return null;
            if (Level + 1 <= maxLevel)
            {
                return Attributes[Level].effect.GetDescription();
            }
            else
                return null;
        }
        public override int GetUpgradeCost()
        {
            if (Attributes == null)
                return 0;
            return Attributes[Level-1].upgradeGoldCost;
        }
        public override int GetUpgradeSmithingStoneCost()
        {
            if (Attributes == null)
                return 0;
            return Attributes[Level-1].upgradeSmithingStoneCost;
        }

    }
}