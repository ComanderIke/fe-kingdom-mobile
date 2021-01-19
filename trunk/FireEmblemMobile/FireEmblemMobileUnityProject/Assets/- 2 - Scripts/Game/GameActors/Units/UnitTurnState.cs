using System;
using Game.Manager;

namespace Game.GameActors.Units
{
    public class UnitTurnState
    {
        public event Action<bool> OnHasAttacked;
        public event Action<bool> OnSelected;
        private readonly Unit unit;
        private bool hasMoved;
        private bool isWaiting;
        private bool hasAttacked;
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
               
            }
        }

        public bool HasAttacked { get => hasAttacked; set
            {
                hasAttacked = value;
                OnHasAttacked?.Invoke(hasAttacked);
            }
        }

        private bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                OnSelected?.Invoke(selected);
            }
        }

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