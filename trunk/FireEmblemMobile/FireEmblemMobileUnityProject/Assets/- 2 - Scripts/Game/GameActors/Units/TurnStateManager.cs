using System;

namespace Game.GameActors.Units
{
    public class TurnStateManager
    {
        public delegate void OnUnitWaiting(Unit unit, bool waiting);

        public OnUnitWaiting UnitWaiting;

        public delegate void OnUnitCanMove(Unit unit, bool canMove);

        public OnUnitCanMove UnitCanMove;
        public UnitTurnState UnitTurnState { get; private set; }
        private Unit unit;

        public TurnStateManager(Unit unit)
        {
            UnitTurnState = new UnitTurnState();
            unit = this.unit;
        }

        public bool IsWaiting
        {
            get
            {
                return UnitTurnState.isWaiting;
            }
            set
            {
                UnitTurnState.isWaiting = value;
                UnitWaiting?.Invoke(unit, value);
            }
        }
        public bool HasMoved
        {
            get
            {
                return UnitTurnState.hasMoved;
            }
            set
            {
                UnitTurnState.hasMoved = value;
                UnitCanMove?.Invoke(unit, !value);
            }
        }

        public bool HasAttacked { get => UnitTurnState.hasAttacked; set
            {
                UnitTurnState.hasAttacked = value;
                //OnHasAttacked?.Invoke(value);
            }
        }
        public bool IsActive { get=> UnitTurnState.isActive; set => UnitTurnState.isActive= value;
        }
        
        public bool IsSelected
        {
            get
            {
                return UnitTurnState.isSelected;
            }
            set
            {
                UnitTurnState.isSelected = value;
                OnSelected?.Invoke(value);
            }
        }
        public void UnitTurnFinished()
        {
            IsActive = false;
            HasMoved = true;
            HasAttacked = true;
            IsWaiting = true;
        }
        public void EndTurn()
        {
            Reset();
        }

        public void UpdateTurn()
        {
            unit.StatusEffectManager.Update();
          
            Reset();
        }

        public void Reset()
        {
            HasMoved = false;
            HasAttacked = false;
            IsWaiting = false;
            IsSelected = false;
            IsActive = false;
        }

        public event Action<bool> OnSelected;
    }
}