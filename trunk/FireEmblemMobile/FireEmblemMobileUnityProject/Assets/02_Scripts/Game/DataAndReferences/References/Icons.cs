using System;
using System.Collections.Generic;
using Game.GameActors.Units;
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
        public Sprite BrawnIcon;
        public Sprite TechniqueIcon;
        public Sprite IntellectIcon;

        
        public Sprite GetRandomMetaUpgradeIcon()
        {
            return MetaUpgrades[UnityEngine.Random.Range(0, MetaUpgrades.Count)];
        }

        public Sprite GetPowerTriangleIcon(PowerTriangleType type)
        {
            switch (type)
            {
                case PowerTriangleType.Brawn: return BrawnIcon;
                case PowerTriangleType.Technique: return TechniqueIcon;
                case PowerTriangleType.Intellect: return IntellectIcon;
            }

            return null;
        }
    }
}