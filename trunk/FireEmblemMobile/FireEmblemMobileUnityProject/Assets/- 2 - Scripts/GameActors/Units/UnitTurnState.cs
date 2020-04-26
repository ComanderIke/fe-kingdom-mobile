using Assets.Core;
using Assets.GUI;
using UnityEngine;

namespace Assets.GameActors.Units
{
    public class UnitTurnState
    {
        private readonly Unit unit;
        private bool hasMoved;
        private bool isWaiting;

        public UnitTurnState(Unit unit)
        {
            this.unit = unit;
        }

        public bool IsActive { get; set; }

        public bool IsWaiting
        {
            get => isWaiting;
            set
            {
                isWaiting = value;
                Unit.UnitWaiting?.Invoke(unit, isWaiting);
            }
        }

        public bool HasMoved
        {
            get => hasMoved;
            set
            {
                hasMoved = value;
                unit.UnitCanMove?.Invoke(unit, !hasMoved);
                
                if (hasMoved)
                    UiSystem.OnHideCursor?.Invoke();
            }
        }

        public bool HasAttacked { get; set; }
        public bool Selected { get; set; }

        public void Reset()
        {
            HasMoved = false;
            HasAttacked = false;
            IsWaiting = false;
            Selected = false;
            IsActive = false;
        }

        public void UnitTurnFinished()
        {
            IsActive = false;
            HasMoved = true;
            HasAttacked = true;
            IsWaiting = true;
        }

        public bool IsDragable()
        {
            return !IsWaiting && unit.IsAlive() &&
                   unit.Faction.Id == GridGameManager.Instance.FactionManager.ActivePlayerNumber;
        }
    }
}