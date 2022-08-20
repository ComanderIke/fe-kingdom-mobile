using System;
using System.Linq;
using Game.GameActors.Units.Humans;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Relic", fileName = "Relic")]
    public class Relic : EquipableItem
    {
        [Header("RelicAttributes")] public UpgradeAttributes[] Attributes;
        public int Level = 1;

        public int maxLevel = 5;
        //public int Weight;
        //public int armor;

        //public List<WeaponMixin> WeaponMixins;


        public string GetAttributeDescription()
        {
            if(Attributes[Level -1].effect!=null)
                return Attributes[Level -1].effect.GetDescription();
            return "No Description?!";
        }

        public string GetUpgradeAttributeDescription()
        {
            if (Level + 1 <= maxLevel)
            {
                return Attributes[Level].effect.GetDescription();
            }
            else
                return null;
        }
        public override int GetUpgradeCost()
        {
            return Attributes[Level-1].upgradeGoldCost;
        }
        public override int GetUpgradeSmithingStoneCost()
        {
            return Attributes[Level-1].upgradeSmithingStoneCost;
        }

      
    }
}