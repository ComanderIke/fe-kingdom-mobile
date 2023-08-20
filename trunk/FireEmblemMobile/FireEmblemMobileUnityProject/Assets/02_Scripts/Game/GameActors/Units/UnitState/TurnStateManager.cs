using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills.Passive;
using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    public class TurnStateManager
    {
        public delegate void OnActorWaiting(bool waiting);

        public OnActorWaiting UnitWaiting;

        public delegate void OnActorCanMove(bool canMove);
        public delegate void OnSelected(bool selected);

        public OnSelected onSelected;
   
        public OnActorCanMove UnitCanMove;
        [SerializeField] private UnitTurnState UnitTurnState;
        private Dictionary<TurnStateManager.TurnStateEvent, List<ITurnStateListener>> turnStateEvents;

        public TurnStateManager()
        {
            UnitTurnState = new UnitTurnState();
            turnStateEvents = new Dictionary<TurnStateEvent, List<ITurnStateListener>>();
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

        public bool HasCantoed { get; set; }

        public void UnitTurnFinished()
        {
           // IsActive = false;
            HasMoved = true;
            HasAttacked = true;
            IsWaiting = true;
            HasCantoed = true;
        }
        public void EndTurn()
        {
            Reset();
        }

        public void UpdateTurn()
        {
            // if(actor is Unit unit)
            //     unit.StatusEffectManager.Update();
          
            Reset();
        }

        public void Reset()
        {
            HasMoved = false;
            HasAttacked = false;
            IsWaiting = false;
            IsSelected = false;
            HasCantoed = false;
            // IsActive = false;
        }



        public void Wait()
        {
            IsWaiting = true;
            IsSelected = false;
            HasMoved = true;
            HasCantoed = true;
        }

        public enum TurnStateEvent
        {
            Wait,
            Move,
            Attacked,
            Selected
        }
       
        public void AddListener(TurnStateEvent turnStateEvent, ITurnStateListener listener)
        {
            if (!turnStateEvents.ContainsKey(turnStateEvent))
            {
                turnStateEvents.Add(turnStateEvent, new List<ITurnStateListener>(){listener});
            }
            else
            {
                turnStateEvents[turnStateEvent].Add(listener);
            }
        }

        public void RemoveListener(TurnStateEvent turnStateEvent, OnWaitEffectSkillMixin listener)
        {
            if (turnStateEvents.ContainsKey(turnStateEvent))
            {
                turnStateEvents[turnStateEvent].Remove(listener);
            }
        }
    }
}