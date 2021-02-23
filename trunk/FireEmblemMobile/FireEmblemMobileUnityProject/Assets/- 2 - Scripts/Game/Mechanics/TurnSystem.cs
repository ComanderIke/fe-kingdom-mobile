using System;
using Game.GameInput;
using Game.GUI.Text;
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

        public IPhaseRenderer phaseRenderer;

        public void Init()
        {
            gridGameManager = GridGameManager.Instance;
            OnTriggerEndTurn += EndPhase;
            factionManager = gridGameManager.FactionManager;
            InitPlayers();
        }


        private void InitPlayers()
        {
            factionManager.ActivePlayerNumber = 0;
            factionManager.ActiveFaction = gridGameManager.FactionManager.Factions[factionManager.ActivePlayerNumber];
        }

        public void StartPhase()
        {
            GridInputSystem.SetActive(false);
            phaseRenderer.Show(factionManager.ActiveFaction.Id, ReadyPhase);
        }

        private void ReadyPhase()
        {
            if (!factionManager.ActiveFaction.IsPlayerControlled)
            {
                //Debug.Log("AITurn");
                GridInputSystem.SetActive(false);
                gridGameManager.GameStateManager.Feed(NextStateTrigger.StartEnemyPhase);
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
            StartPhase();
        }

        private void OnDestroy()
        {
            OnTriggerEndTurn = null;
            OnStartTurn = null;
        }
    }
}