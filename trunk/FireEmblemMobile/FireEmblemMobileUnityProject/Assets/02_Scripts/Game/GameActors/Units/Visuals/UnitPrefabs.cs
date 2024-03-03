using System;
using UnityEngine;

namespace Game.GameActors.Units.Visuals
{
    [Serializable]
    public class UnitPrefabs
    {
    
        public GameObject EncounterAnimatedSprite;
        public AnimatorOverrideController UIAnimatorController;

        public UnitPrefabs(UnitPrefabs other)
        {
     
            EncounterAnimatedSprite = other.EncounterAnimatedSprite;
            UIAnimatorController = other.UIAnimatorController;
        }
    }
}