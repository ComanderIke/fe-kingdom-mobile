using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Game.Graphics
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