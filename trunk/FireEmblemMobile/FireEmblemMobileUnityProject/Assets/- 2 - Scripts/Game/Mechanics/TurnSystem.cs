using System;
using Game.GameInput;
using Game.Manager;
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
            OnTriggerEndTurn += EndTurn;
            factionManager = gridGameManager.FactionManager;
            InitPlayers();
        }
 

        private void InitPlayers()
        {
            factionManager.ActivePlayerNumber = 0;
            factionManager.ActiveFaction = gridGameManager.FactionManager.Factions[factionManager.ActivePlayerNumber];
        }

        public void StartTurn()
        {
            if (!factionManager.ActiveFaction.IsPlayerControlled)
            {
                //Debug.Log("AITurn");
                GridInputSystem.SetActive(false);
                gridGameManager.GameStateManager.Feed(NextStateTrigger.StartAITurn);
            }
            else
            {
                //Debug.Log("PlayerTurn");
                GridInputSystem.SetActive(true);
            }
            OnStartTurn?.Invoke();
            foreach (var c in factionManager.ActiveFaction.Units)
            {
                c.TurnStateManager.UpdateTurn();
            }
        }

        public void EndTurn()
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
            StartTurn();
        }

        private void OnDestroy()
        {
            OnTriggerEndTurn = null;
            OnStartTurn = null;
        }
    }
}