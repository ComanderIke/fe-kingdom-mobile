using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class UnitTurnState
    {
        LivingObject unit;
        private bool isActive;
        private bool hasMoved;
        private bool isWaiting;
        public bool IsActive {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                if(EventContainer.unitActiveChanged !=null)
                    EventContainer.unitActiveChanged(unit, isActive);
            }
        }
        public bool IsWaiting
        {
            get
            {
                return isWaiting;
            }
            set
            {
                isWaiting = value;
                EventContainer.unitWaiting(unit, isWaiting);
            }
        }
        public bool HasMoved
        {
            get
            {
                return hasMoved;
            }
            set
            {
                hasMoved = value;
                EventContainer.unitCanMove(unit, !value);
            }
        }
        public bool HasAttacked { get; set; }
        public bool Selected { get; set; }

        public UnitTurnState(LivingObject unit)
        {
            this.unit = unit;
        }
        public void Reset()
        {
            HasMoved = false;
            HasAttacked = false;
            IsWaiting = false;
            Selected = false;
            IsActive = false;
            MainScript.GetInstance().GetSystem<UnitSelectionManager>().DeselectActiveCharacter();
        }

        public bool IsDragable()
        {
            return !IsWaiting && unit.IsAlive() && unit.Player.ID == MainScript.GetInstance().GetSystem<TurnManager>().ActivePlayerNumber;
        }
    }
}
