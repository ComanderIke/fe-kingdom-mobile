using Assets.Scripts.GameStates;


namespace Assets.Scripts.Characters
{
    public class UnitTurnState
    {
        Unit unit;
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
                if(Unit.onUnitWaiting!=null)
                    Unit.onUnitWaiting(unit, isWaiting);
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
                if(Unit.onUnitCanMove!=null)
                    Unit.onUnitCanMove(unit, !hasMoved);
                if(unit.Player.IsHumanPlayer)
                    Unit.onUnitShowActiveEffect(unit, !hasMoved, false);
                if(hasMoved)
                    UISystem.onHideCursor();
            }
        }
        public bool HasAttacked { get; set; }
        public bool Selected { get; set; }

        public UnitTurnState(Unit unit)
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
           // MainScript.GetInstance().GetSystem<UnitSelectionManager>().DeselectActiveCharacter();
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
            return !IsWaiting && unit.IsAlive() && unit.Player.ID == MainScript.instance.GetSystem<TurnSystem>().ActivePlayerNumber;
        }
    }
}
