using System;
using UnityEngine;

namespace Game.GameActors.Units
{
    public class TurnStateManager
    {
        public delegate void OnActorWaiting(bool waiting);

        public OnActorWaiting UnitWaiting;

        public delegate void OnActorCanMove(bool canMove);
        public delegate void OnSelected(bool selected);

        public OnSelected onSelected;
   
        public OnActorCanMove UnitCanMove;
        private UnitTurnState UnitTurnState { get; set; }
        private IActor actor;

        public TurnStateManager(IActor actor)
        {
            UnitTurnState = new UnitTurnState();
            this.actor = actor;
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
                UnitWaiting?.Invoke(value);
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
                UnitCanMove?.Invoke(!value);
            }
        }

        public bool HasAttacked { get => UnitTurnState.hasAttacked; set
            {
                UnitTurnState.hasAttacked = value;
                //OnHasAttacked?.Invoke(value);
            }
        }
        //public bool IsActive { get=> UnitTurnState.isActive; set => UnitTurnState.isActive= value;
        //}
        
        public bool IsSelected
        {
            get
            {
                return UnitTurnState.isSelected;
            }
            set
            {
               
                UnitTurnState.isSelected = value;
                onSelected?.Invoke(value);
            }
        }
        public void UnitTurnFinished()
        {
           // IsActive = false;
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
            if(actor is Unit unit)
                unit.StatusEffectManager.Update();
          
            Reset();
        }

        public void Reset()
        {
            HasMoved = false;
            HasAttacked = false;
            IsWaiting = false;
            IsSelected = false;
           // IsActive = false;
        }



        public void Wait()
        {
            actor.TurnStateManager.IsWaiting = true;
            actor.TurnStateManager.IsSelected = false;
            actor.TurnStateManager.HasMoved = true;
        }
    }
}