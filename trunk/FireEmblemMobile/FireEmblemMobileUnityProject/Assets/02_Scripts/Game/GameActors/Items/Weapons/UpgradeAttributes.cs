using System;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class UpgradeAttributes
    {

        public int upgradeGoldCost = 50;
        public int upgradeSmithingStoneCost = 1;
        public EffectMixin effect;
    }
}