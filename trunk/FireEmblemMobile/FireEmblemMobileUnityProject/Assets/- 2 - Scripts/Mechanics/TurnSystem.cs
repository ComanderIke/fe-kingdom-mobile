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

        private MainScript mainScript;
        private PlayerManager playerManager;

        public int TurnCount { get; set; }

        private void Start()
        {
            mainScript = MainScript.Instance;
            OnEndTurn += EndTurn;
            OnStartTurn += StartTurn;
            playerManager = mainScript.PlayerManager;
            InitPlayers();
        }

        private void InitPlayers()
        {
            playerManager.ActivePlayerNumber = 0;
            playerManager.ActivePlayer = mainScript.PlayerManager.Players[playerManager.ActivePlayerNumber];
        }

        private IEnumerator DelayTogglePlayerInput(bool active, float delay)
        {
            yield return new WaitForSeconds(delay);
            UnitController.LockInput = !active;
            mainScript.GetSystem<InputSystem>().Active = active;
            if (active)
                mainScript.GetSystem<UiSystem>().ShowAllActiveUnitEffects();
            else
                mainScript.GetSystem<UiSystem>().HideAllActiveUnitEffects();
        }

        private IEnumerator DelayAIPhase(float delay)
        {
            yield return new WaitForSeconds(delay);
            mainScript.GameStateManager.Feed(NextStateTrigger.StartAITurn);
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
            foreach (var c in playerManager.ActivePlayer.Units)
            {
                c.UpdateTurn();
            }
        }

        private void StartTurn()
        {
            if (!playerManager.ActivePlayer.IsPlayerControlled)
            {
                Debug.Log("AITurn");
                mainScript.GetSystem<AudioSystem>().ChangeMusic("EnemyTheme", "PlayerTheme", true);

                mainScript.GetSystem<UiSystem>().EnemyTurnAnimation();
                mainScript.StartCoroutine(DelayTogglePlayerInput(false, 0));
                mainScript.StartCoroutine(DelayAIPhase(PlayerTurnTextAnimation.Duration + 0.25f));
            }
            else
            {
                Debug.Log("PlayerTurn");
                mainScript.GetSystem<AudioSystem>().ChangeMusic("PlayerTheme", "EnemyTheme", true);
                mainScript.GetSystem<UiSystem>().PlayerTurnAnimation();
                mainScript.StartCoroutine(DelayTogglePlayerInput(true, PlayerTurnTextAnimation.Duration + 0.25f));
            }

            mainScript.StartCoroutine(DelayTurnUpdate(PlayerTurnTextAnimation.Duration + 0.25f));
        }

        public void EndTurn()
        {
            Debug.Log("EndTurn!");
            foreach (var c in playerManager.ActivePlayer.Units)
            {
                c.EndTurn();
                //c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
            }

            playerManager.ActivePlayerNumber++;

            mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter = null;
            StartTurn();
        }

        private void OnDestroy()
        {
            OnEndTurn = null;
            OnStartTurn = null;
        }
    }
}