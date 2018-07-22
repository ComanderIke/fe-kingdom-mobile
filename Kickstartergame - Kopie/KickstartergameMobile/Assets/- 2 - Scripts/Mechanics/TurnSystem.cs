using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class TurnSystem : MonoBehaviour, EngineSystem
    {

        #region Events
        public delegate void OnEndTurn();
        public static OnEndTurn onEndTurn;
        public delegate void OnStartTurn();
        public static OnStartTurn onStartTurn;
        #endregion
        private MainScript mainScript;
        private PlayerManager playerManager;
        
        public int TurnCount { get; set; }
        
        

        
        void Start()
        {
            mainScript = MainScript.instance;
            onEndTurn += EndTurn;
            onStartTurn += StartTurn;
            playerManager = mainScript.PlayerManager;
            InitPlayers();
        }
       
       
 
        private void InitPlayers()
        {
           
            playerManager.ActivePlayerNumber = 0;
            playerManager.ActivePlayer = mainScript.PlayerManager.Players[playerManager.ActivePlayerNumber];
        }
       
        IEnumerator DelayTooglePlayerInput(bool active, float delay)
        {
            yield return new WaitForSeconds(delay);
            UnitController.lockInput = !active;
            mainScript.GetSystem<InputSystem>().active = active;
            if (active)
                mainScript.GetSystem<UISystem>().ShowAllActiveUnitEffects();
            else
                mainScript.GetSystem<UISystem>().HideAllActiveUnitEffects();
        }
        IEnumerator DelayAIPhase( float delay)
        {
            yield return new WaitForSeconds(delay);
            mainScript.GameStateManager.Feed(NextStateTrigger.StartAITurn);
        }
        IEnumerator DelayTurnUpdate(float delay)
        {
            yield return new WaitForSeconds(delay);
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
            foreach (Unit c in playerManager.ActivePlayer.Units)
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
               
                mainScript.GetSystem<UISystem>().EnemyTurnAnimation();
                mainScript.StartCoroutine(DelayTooglePlayerInput(false,0));
                mainScript.StartCoroutine(DelayAIPhase(PlayerTurnTextAnimation.duration + 0.25f));
            }
            else
            {
                Debug.Log("PlayerTurn");
                mainScript.GetSystem<AudioSystem>().ChangeMusic("PlayerTheme", "EnemyTheme", true);
                mainScript.GetSystem<UISystem>().PlayerTurnAnimation();
                mainScript.StartCoroutine(DelayTooglePlayerInput(true, PlayerTurnTextAnimation.duration+0.25f));
            }
            mainScript.StartCoroutine(DelayTurnUpdate( PlayerTurnTextAnimation.duration + 0.25f));

        }

        public void EndTurn()
        {
            Debug.Log("EndTurn!");
            foreach (Unit c in playerManager.ActivePlayer.Units)
            {
                c.EndTurn();
                //c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
            }
            playerManager.ActivePlayerNumber++;
            
            mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter = null;
            StartTurn();
        }
        void OnDestroy()
        {
            onEndTurn = null;
            onStartTurn = null;
        }


    }
}
