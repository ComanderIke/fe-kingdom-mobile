using Assets.Audio;
using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameInput;
using Assets.GUI;
using Assets.Manager;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Mechanics
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

        private void Start()
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
                Debug.Log("AITurn");
                UnitController.LockInput = true;
                InputSystem.OnSetActive(false);
                gridGameManager.GameStateManager.Feed(NextStateTrigger.StartAITurn);
            }
            else
            {
                Debug.Log("PlayerTurn");
                UnitController.LockInput = !false;
                InputSystem.OnSetActive(true);
            }
            OnStartTurn?.Invoke();
            foreach (var c in factionManager.ActiveFaction.Units)
            {
                c.UpdateTurn();
            }
        }

        public void EndTurn()
        {
            Debug.Log("EndTurn!");
            foreach (var c in factionManager.ActiveFaction.Units)
            {
                c.EndTurn();
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