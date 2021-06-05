using System;
using Game.Manager;
using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    public class UnitTurnState
    {
        [SerializeField]
        public bool hasMoved;
        [SerializeField]
        public bool isWaiting;
        [SerializeField]
        public bool hasAttacked;
        [SerializeField]
        public bool isSelected;
       // public bool isActive;
        
    }
}