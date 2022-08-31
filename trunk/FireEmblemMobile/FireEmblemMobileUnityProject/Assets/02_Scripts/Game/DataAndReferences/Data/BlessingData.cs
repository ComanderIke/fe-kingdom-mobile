using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameResources
{
    [Serializable]
    public class BlessingData : IBlessingData
    {
        [SerializeField] List<Blessing> tier0Blessings;
        [SerializeField] List<Blessing> tier1Blessings;
        [SerializeField] List<Blessing> tier2Blessings;
        [SerializeField] List<Blessing> tier3Blessings;
        [SerializeField] List<Blessing> tier4Blessings;
        
        public List<Blessing> GetBlessingPool(int tier)
        {
            switch (tier)
            {
                case 0: return tier0Blessings;
                case 1: return tier1Blessings;
                case 2: return tier2Blessings;
                case 3: return tier3Blessings;
                case 4: return tier4Blessings;
                default: return null;
            }
        }
    }
}