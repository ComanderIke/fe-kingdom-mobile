using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataAndReferences.References
{
    [Serializable]
    public class Icons
    {
        public Sprite Grace;
        public Sprite CorruptedGrace;
        public Sprite DeathStones;
        public List<Sprite> MetaUpgrades;

        public Sprite GetRandomMetaUpgradeIcon()
        {
            return MetaUpgrades[UnityEngine.Random.Range(0, MetaUpgrades.Count)];
        }
    }
}