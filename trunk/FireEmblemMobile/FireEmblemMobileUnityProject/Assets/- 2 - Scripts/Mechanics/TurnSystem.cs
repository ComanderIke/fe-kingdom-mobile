using Assets.Audio;
using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameInput;
using Assets.GUI;
using Assets.Manager;
using System.Collections;
using UnityEngine;

namespace Assets.Mechanics
{
    public class TurnSystem : MonoBehaviour, IEngineSystem
    {
        #region Events

        public delegate void OnEndTurnEvent();

        public static OnEndTurnEvent OnEndTurn;

        public delegate void OnStartTurnEvent();

        public static OnStartTurnEvent OnStartTurn;

        #endregion

        private GridGameManager gridGameManager;
        private FactionManager factionManager;

        public int TurnCount { get; set; }

        private void Start()
        {
            gridGameManager = GridGameManager.Instance;
            OnEndTurn += EndTurn;
            OnStartTurn += StartTurn;
            factionManager = gridGameManager.FactionManager;
            InitPlayers();
        }

        private void InitPlayers()
        {
            factionManager.ActivePlayerNumber = 0;
            factionManager.ActiveFaction = gridGameManager.FactionManager.Factions[factionManager.ActivePlayerNumber];
        }

        private IEnumerator DelayTogglePlayerInput(bool active, float delay)
        {
            yield return new WaitForSeconds(delay);
            UnitController.LockInput = !active;
            gridGameManager.GetSystem<InputSystem>().Active = active;
            if (active)
                gridGameManager.GetSystem<UiSystem>().ShowAllActiveUnitEffects();
            else
                gridGameManager.GetSystem<UiSystem>().HideAllActiveUnitEffects();
        }

        private IEnumerator DelayAIPhase(float delay)
        {
            yield return new WaitForSeconds(delay);
            gridGameManager.GameStateManager.Feed(NextStateTrigger.StartAITurn);
        }

        private IEnumerator DelayTurnUpdate(float delay)
        {
            yield return new WaitForSeconds(delay);
            Debug.Log("Update Turn");
            //if (ActivePlayer.ID == 1)
            //{
            //    TurnCount++;
            //    foreach (Player p in Players)
            //    {
            //        foreach (LivingObject liv in ActivePlayer.Units)
            //        {
            //            liv.UpdateOnWholeTurn();
            //        }
            //    }
            //    Debug.Log("Turn: " + TurnCount);
            //}
            foreach (var c in factionManager.ActiveFaction.Units)
            {
                c.UpdateTurn();
            }
        }

        private void StartTurn()
        {
            if (!factionManager.ActiveFaction.IsPlayerControlled)
            {
                Debug.Log("AITurn");
                gridGameManager.GetSystem<AudioSystem>().ChangeMusic("EnemyTheme", "PlayerTheme", true);

                gridGameManager.GetSystem<UiSystem>().EnemyTurnAnimation();
                gridGameManager.StartCoroutine(DelayTogglePlayerInput(false, 0));
                gridGameManager.StartCoroutine(DelayAIPhase(PlayerTurnTextAnimation.Duration + 0.25f));
            }
            else
            {
                Debug.Log("PlayerTurn");
                gridGameManager.GetSystem<AudioSystem>().ChangeMusic("PlayerTheme", "EnemyTheme", true);
                gridGameManager.GetSystem<UiSystem>().PlayerTurnAnimation();
                gridGameManager.StartCoroutine(DelayTogglePlayerInput(true, PlayerTurnTextAnimation.Duration + 0.25f));
            }

            gridGameManager.StartCoroutine(DelayTurnUpdate(PlayerTurnTextAnimation.Duration + 0.25f));
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
            StartTurn();
        }

        private void OnDestroy()
        {
            OnEndTurn = null;
            OnStartTurn = null;
        }
    }
}