using System;
using System.Collections.Generic;
using Game.GameActors.Items.Gems;
using UnityEditor.Graphs;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class GemSlot
    {
        public Gem gem;

        public bool IsEmpty()
        {
            return gem == null;
        }
    }
    
    [Serializable]
    public class Relic:EquipableItem
    {
        [Header("RelicAttributes")] public UpgradeAttributes[] Attributes;
        public int Level = 1;

        public int maxLevel = 5;

        public int slotCount = 0;
        public List<GemSlot> slots;
        public Relic(string name, string description, int cost, int rarity, Sprite sprite, EquipmentSlotType slotType, int level, int maxLevel,int slotCount) : base(name, description, cost,rarity, sprite, slotType)
        {
            this.Level = level;
            this.maxLevel = maxLevel;
            this.slotCount = slotCount;
            slots = new List<GemSlot>();
        }

        public void InsertGem(Gem gem, int slotindex)
        {
            slots[slotindex].gem = gem;

        }
        public Gem RemoveGem(int slotindex)
        {
            var gem = slots[slotindex].gem;
            slots[slotindex].gem = null;
            return gem;

        }
        public bool HasEmptySlot()
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmpty())
                    return true;
            }

            return false;
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

        public int GetSlotCount()
        {
            return slotCount;
        }

        public Gem GetGem(int index)
        {
            if (slots.Count < index)
                return slots[index].gem;
            return null;
        }
    }
}