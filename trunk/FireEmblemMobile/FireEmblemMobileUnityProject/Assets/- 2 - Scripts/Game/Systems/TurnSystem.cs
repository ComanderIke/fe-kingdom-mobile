using System;
using Game.GameInput;
using Game.GUI.Text;
using Game.Manager;
using Game.States;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class TurnSystem : IEngineSystem
    {
        #region Events


        public static Action OnTriggerEndTurn;

        public static event Action OnEndTurn;

        // public static event Action OnStartTurn;

        #endregion

        public GameStateManager gameStateManager{ get; set; }
        public FactionManager factionManager{ get; set; }

        private int turnCount;
        public int TurnCount
        {
            get
            {
                return turnCount;
            }
            set
            {
                turnCount = value;
                if(gameStateManager is WM_GameStateManager)
                    Campaign.Instance.turnCount = turnCount;
                    
            }
        }



        public void Init()
        {
           // var gameManager = TurnBasedGameManager.Instance;
           
            // factionManager = gameManager.FactionManager;
            // gameStateManager = gameManager.GameStateManager;
            InitPlayers();
            TurnCount = Campaign.Instance.turnCount;
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

                if (factionManager.ActiveFaction.Units != null && factionManager.ActiveFaction.Units.Count != 0)
                {
                    foreach (var c in factionManager.ActiveFaction.Units)
                    {

                        c.SpBars++;
                    }
                }

               // Debug.Log("AITurn");
                gameStateManager.Feed(NextStateTrigger.Transition);
                //gridGameManager.GameStateManager.Feed(NextStateTrigger.StartEnemyPhase);
            }
            else
            {
                if (factionManager.ActiveFaction.Units != null && factionManager.ActiveFaction.Units.Count != 0)
                {
                    foreach (var c in factionManager.ActiveFaction.Units)
                    {

                        c.SpBars++;
                    }
                }

                //Debug.Log("PlayerTurn");
                gameStateManager.Feed(NextStateTrigger.Transition);
                

            }
           
            
        }

        // private void ReadyPhase()
        // {
        //   
        //     OnStartTurn?.Invoke();
        //     foreach (var c in factionManager.ActiveFaction.Units)
        //     {
        //         c.TurnStateManager.UpdateTurn();
        //     }
        // }

        private void EndPhase()
        {
           
            //Debug.Log("EndTurn! BUT WHY?");
            if (factionManager.ActiveFaction.Units != null && factionManager.ActiveFaction.Units.Count != 0)
            {
                foreach (var c in factionManager.ActiveFaction.Units)
                {
                    c.TurnStateManager.EndTurn();
                    
                    //c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
                }
            }

            if (factionManager.ActiveFaction is WM_Faction wmFaction)
            {

                if (wmFaction != null && wmFaction.Parties.Count != 0)
                {
                    foreach (var c in  wmFaction.Parties)
                    {

                        c.TurnStateManager.EndTurn();
                        //c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
                    }
                }
            }

            factionManager.ActivePlayerNumber++;
            
            //gameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter = null;
            OnEndTurn?.Invoke();
            if (factionManager.ActivePlayerNumber == 0){
                TurnCount++;
                //Debug.Log("Update SP Bars: "+ factionManager.ActiveFaction.Id);
                
            }
            //Debug.Log("Calling Start Phase From EndPhase");
            StartPhase();
        }

 
    }
}