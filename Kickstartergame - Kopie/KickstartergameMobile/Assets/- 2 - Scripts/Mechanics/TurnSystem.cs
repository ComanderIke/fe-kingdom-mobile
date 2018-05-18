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
        private int activePlayerNumber;
        public int TurnCount { get; set; }
        public Player ActivePlayer { get; set; }
        public List<Player> Players { get; set; }
        public int ActivePlayerNumber
        {
            get
            {
                return activePlayerNumber;
            }
            set
            {
                if (value >= Players.Count)
                    ActivePlayerNumber = 0;
                else
                    activePlayerNumber = value;
            }
        }
        void Start()
        {
            mainScript = MainScript.instance;
            onEndTurn += EndTurn;
            onStartTurn += StartTurn;

            InitPlayers();
        }
       
       
        public void Init()
        {
            foreach (Player p in Players)
            {
                p.Init();
            }
        }
        private void InitPlayers()
        {
            Players = new List<Player>();
            PlayerConfig transform = GameObject.FindObjectOfType<PlayerConfig>();
            foreach (Player p in transform.players)
            {
                Players.Add(p);
            }
            activePlayerNumber = 0;
            ActivePlayer = Players[activePlayerNumber];
        }
        public Player GetRealPlayer()
        {
            return Players[0];
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
            mainScript.SwitchState(new AIState(ActivePlayer));
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
            foreach (Unit c in ActivePlayer.Units)
            {
                c.UpdateTurn();
            }
        }
        private void StartTurn()
        {
            if (!ActivePlayer.IsHumanPlayer)
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
            foreach (Unit c in ActivePlayer.Units)
            {
                c.EndTurn();
                //c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
            }
            ActivePlayerNumber++;
            ActivePlayer = Players[ActivePlayerNumber];
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
