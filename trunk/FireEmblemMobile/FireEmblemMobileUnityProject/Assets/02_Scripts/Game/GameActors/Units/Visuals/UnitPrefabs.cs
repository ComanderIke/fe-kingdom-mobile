using System;
using UnityEngine;

namespace Game.GameActors.Units
{
    [Serializable]
    public class UnitPrefabs
    {
        public GameObject MapAnimatedSprite;
        public GameObject EncounterAnimatedSprite;
        public AnimatorOverrideController UIAnimatorController;

        public UnitPrefabs(UnitPrefabs other)
        {
            MapAnimatedSprite = other.MapAnimatedSprite;
            EncounterAnimatedSprite = other.EncounterAnimatedSprite;
            UIAnimatorController = other.UIAnimatorController;
        }
    }
}