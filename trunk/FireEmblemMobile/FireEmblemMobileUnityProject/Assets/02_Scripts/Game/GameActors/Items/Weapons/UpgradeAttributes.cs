using System;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class UpgradeAttributes:WeaponAttributes
    {
        public int weightCostPower;
        public int weightCostAccuracy;
        public int weightCostCrit;
        public int weightCostSpecial;
    }
}