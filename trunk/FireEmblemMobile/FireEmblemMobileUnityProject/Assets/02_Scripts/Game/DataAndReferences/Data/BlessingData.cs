using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameResources
{
    [Serializable]
    public class BlessingData : IBlessingData
    {
        [SerializeField] BlessingBP[] allBlessings;
        [SerializeField] BlessingBP[]  tier0Blessings;
        [SerializeField] BlessingBP[]  tier1Blessings;
        [SerializeField] BlessingBP[]  tier2Blessings;
        [SerializeField] BlessingBP[]  tier3Blessings;
   
        
        public BlessingBP[]  GetBlessingPool(int tier)
        {
            switch (tier)
            {
                case 0: return tier0Blessings;
                case 1: return tier1Blessings;
                case 2: return tier2Blessings;
                case 3: return tier3Blessings;
                default: return null;
            }
        }

        public void Validate()
        {
            allBlessings = GameData.GetAllInstances<BlessingBP>();
            tier0Blessings = Array.FindAll(allBlessings,a => a.tier == 0);
            tier1Blessings = Array.FindAll(allBlessings,a => a.tier == 1);
            tier2Blessings = Array.FindAll(allBlessings,a => a.tier == 2);
            tier3Blessings = Array.FindAll(allBlessings,a => a.tier == 3);
        }
    }
}