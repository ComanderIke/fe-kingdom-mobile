using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters
{
    public class UnitTurnState
    {
        LivingObject unit;
        public bool IsActive { get; set; }
        public bool IsWaiting { get; set; }
        public bool HasMoved { get; set; }
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
