using System;
using Game.EncounterAreas.Encounters.Event;
using UnityEngine;

namespace Game.Dialog
{
    [System.Serializable]
    public abstract class MiniGame:ScriptableObject
    {
        public abstract void StartGame();

        public abstract Reward GetRewards();

        public abstract event Action OnComplete;
    }
}