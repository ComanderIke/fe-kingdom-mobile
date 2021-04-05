using System;
using Game.GameInput;
using Game.GUI.Text;
using Game.Manager;
using Game.States;
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

        private GridGameManager gridGameManager;
        private FactionManager factionManager;

        public int TurnCount { get; set; }

   

        public void Init()
        {
            gridGameManager = GridGameManager.Instance;
            OnTriggerEndTurn += EndPhase;
            factionManager = gridGameManager.FactionManager;
            InitPlayers();
            TurnCount = 1;
        }


        private void InitPlayers()
        {
            factionManager.ActivePlayerNumber = 0;
            factionManager.ActiveFaction = gridGameManager.FactionManager.Factions[factionManager.ActivePlayerNumber];
        }

        public void StartPhase()
        {
            if (!factionManager.ActiveFaction.IsPlayerControlled)
            {
                //Debug.Log("AITurn");
                gridGameManager.GameStateManager.Feed(NextStateTrigger.Transition);
                //gridGameManager.GameStateManager.Feed(NextStateTrigger.StartEnemyPhase);
            }
            else
            {
                gridGameManager.GameStateManager.Feed(NextStateTrigger.Transition);
                //Debug.Log("PlayerTurn");
            }
           
            
        }

        private void ReadyPhase()
        {
          
            OnStartTurn?.Invoke();
            foreach (var c in factionManager.ActiveFaction.Units)
            {
                c.TurnStateManager.UpdateTurn();
            }
        }

        public void EndPhase()
        {
           
            //Debug.Log("EndTurn!");
            foreach (var c in factionManager.ActiveFaction.Units)
            {
                c.TurnStateManager.EndTurn();
                //c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
            }

            factionManager.ActivePlayerNumber++;
            
            gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter = null;
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