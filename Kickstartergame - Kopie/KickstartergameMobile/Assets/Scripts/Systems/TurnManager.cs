using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Events;
using Assets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class TurnManager : EngineSystem
    {

        private MainScript mainScript;
        private int activePlayerNumber;
        public int TurnCount { get; set; }
        public Player ActivePlayer { get; set; }
        public List<Player> Players { get; set; }

        public TurnManager()
        {
            mainScript = MainScript.GetInstance();
            EventContainer.endTurn += EndTurn;
            EventContainer.startTurn += StartTurn;

            InitPlayers();
        }
       
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

        IEnumerator DelayTooglePlayerInput(bool active, float delay)
        {
            yield return new WaitForSeconds(delay);
            UnitController.lockInput = !active;
            mainScript.GetSystem<MouseManager>().active = active;
            if (active)
                mainScript.GetController<UIController>().ShowAllActiveUnitEffects();
            else
                mainScript.GetController<UIController>().HideAllActiveUnitEffects();
            CameraMovement.locked = !active;
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
            foreach (LivingObject c in ActivePlayer.Units)
            {
                c.UpdateTurn();
            }
        }
        private void StartTurn()
        {
            Debug.Log("StartTurn");
            if (!ActivePlayer.IsHumanPlayer)
            {

                Debug.Log("AITurn");
                GameObject.FindObjectOfType<AudioManager>().ChangeMusic("EnemyPhase", "PlayerPhase", true);
               
                mainScript.GetController<UIController>().EnemyTurnAnimation();
                mainScript.StartCoroutine(DelayTooglePlayerInput(false,0));
                mainScript.StartCoroutine(DelayAIPhase(PlayerTurnTextAnimation.duration + 0.25f));
            }
            else
            {
                Debug.Log("PlayerTurn");
                GameObject.FindObjectOfType<AudioManager>().ChangeMusic("PlayerPhase", "EnemyPhase", true);
                mainScript.GetController<UIController>().PlayerTurnAnimation();
                mainScript.StartCoroutine(DelayTooglePlayerInput(true, PlayerTurnTextAnimation.duration+0.25f));
            }
            mainScript.StartCoroutine(DelayTurnUpdate( PlayerTurnTextAnimation.duration + 0.25f));

        }

        public void EndTurn()
        {
            Debug.Log("EndTurn!");
            foreach (LivingObject c in ActivePlayer.Units)
            {
                c.EndTurn();
                //c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
            }
            ActivePlayerNumber++;
            ActivePlayer = Players[ActivePlayerNumber];
            mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter = null;
            StartTurn();
        }
    }
}
