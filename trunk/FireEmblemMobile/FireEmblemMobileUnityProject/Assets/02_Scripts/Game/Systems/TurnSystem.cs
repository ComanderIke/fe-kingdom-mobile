using System;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI.Text;
using Game.Manager;
using Game.States;
using Game.WorldMapStuff.Model;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class TurnSystem : IEngineSystem
    {
        #region Events


        public Action OnTriggerEndTurn;

        public event Action OnEndTurn;
        public event Action OnStartTurn;

        // public static event Action OnStartTurn;

        #endregion

        public GameStateManager gameStateManager{ get; set; }
        public FactionManager factionManager{ get; set; }

        private int turnCount = 1;
        public int TurnCount
        {
            get
            {
                return turnCount;
            }
            set
            {
                turnCount = value;
            }
        }



        public void Init()
        {
            InitPlayers();
        }

        public void Deactivate()
        {
            //OnStartTurn = null;
            OnTriggerEndTurn = null;
           // Debug.Log("Deactivate TurnSystem!");
        }

        public void Activate()
        {
           // Debug.Log("Activate TurnSystem!");
            OnTriggerEndTurn = null;
            //OnStartTurn = null;
            OnTriggerEndTurn += EndPhase;
        }

        private void InitPlayers()
        {
            factionManager.ActivePlayerNumber = 0;
            factionManager.ActiveFaction = factionManager.Factions[factionManager.ActivePlayerNumber];
        }

        private void StartPhase()
        {
           
            if (!factionManager.ActiveFaction.IsPlayerControlled)
            {
                gameStateManager.Feed(NextStateTrigger.Transition);
            }
            else
            {
                gameStateManager.Feed(NextStateTrigger.Transition);
            }
        }

        private void EndPhase()
        {
            if (factionManager.ActiveFaction.Units != null && factionManager.ActiveFaction.Units.Count != 0)
            {
                foreach (var c in factionManager.ActiveFaction.Units)
                {
                    c.TurnStateManager.EndTurn();
                }
            }

            factionManager.ActivePlayerNumber++;
            OnEndTurn?.Invoke();
            foreach (var c in factionManager.ActiveFaction.Units)
            {
                c.StatusEffectManager.UpdateTurn();
            }
            if (factionManager.ActivePlayerNumber == 0){
                TurnCount++;
                Debug.Log("NEW TURN: "+TurnCount);
              
                
                
                OnStartTurn?.Invoke();

            }
            StartPhase();
        }


       
    }
}