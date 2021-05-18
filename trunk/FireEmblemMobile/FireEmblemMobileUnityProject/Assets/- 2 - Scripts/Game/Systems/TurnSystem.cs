using System;
using Game.GameInput;
using Game.GUI.Text;
using Game.Manager;
using Game.States;
using Game.WorldMapStuff.Model;
using GameEngine;
using UnityEngine;

namespace Game.Mechanics
{
    public class TurnSystem : MonoBehaviour, IEngineSystem
    {
        #region Events


        public static Action OnTriggerEndTurn;

        public static event Action OnEndTurn;

        public static event Action OnStartTurn;

        #endregion

        public GameStateManager gameStateManager{ get; set; }
        public FactionManager factionManager{ get; set; }

        public int TurnCount { get; set; }

   

        public void Init()
        {
           // var gameManager = TurnBasedGameManager.Instance;
            OnTriggerEndTurn += EndPhase;
            // factionManager = gameManager.FactionManager;
            // gameStateManager = gameManager.GameStateManager;
            InitPlayers();
            TurnCount = 1;
        }

        public void Deactivate()
        {
            
        }

        public void Activate()
        {

        }

        private void InitPlayers()
        {
            factionManager.ActivePlayerNumber = 0;
            factionManager.ActiveFaction = factionManager.Factions[factionManager.ActivePlayerNumber];
        }

        public void StartPhase()
        {
            if (!factionManager.ActiveFaction.IsPlayerControlled)
            {
                //Debug.Log("AITurn");
                gameStateManager.Feed(NextStateTrigger.Transition);
                //gridGameManager.GameStateManager.Feed(NextStateTrigger.StartEnemyPhase);
            }
            else
            {
                gameStateManager.Feed(NextStateTrigger.Transition);
                //Debug.Log("PlayerTurn");
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

        public void EndPhase()
        {
           
            //Debug.Log("EndTurn!");
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
                    Debug.Log(wmFaction.Parties.Count);
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
            if (factionManager.ActivePlayerNumber == 0)
                TurnCount++;
            StartPhase();
        }

        private void OnDestroy()
        {
            OnTriggerEndTurn = null;
            OnStartTurn = null;
        }
    }
}