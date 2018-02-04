using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Events;
using Assets.Scripts.Players;
using System;
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

        private void StartTurn()
        {
            Debug.Log("StartTurn!");
            if (!ActivePlayer.IsHumanPlayer)
            {

                GameObject.FindObjectOfType<AudioManager>().ChangeMusic("EnemyPhase", "PlayerPhase", true);
                mainScript.SwitchState(new AIState(ActivePlayer));
                GameObject go = GameObject.Instantiate(mainScript.AITurnAnimation, new Vector3(), Quaternion.identity) as GameObject;
                go.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }
            else
            {
                GameObject.FindObjectOfType<AudioManager>().ChangeMusic("PlayerPhase", "EnemyPhase", true);
                GameObject go = GameObject.Instantiate(mainScript.PlayerTurnAnimation, new Vector3(), Quaternion.identity) as GameObject;
                go.transform.localPosition = new Vector3();
                go.transform.SetParent(GameObject.Find("Canvas").transform, false);

            }
            if (ActivePlayer.ID == 1)
            {
                TurnCount++;
                foreach (Player p in Players)
                {
                    foreach (LivingObject liv in p.Units)
                    {
                        liv.UpdateOnWholeTurn();
                    }
                }
                Debug.Log("Turn: " + TurnCount);
            }
            foreach (LivingObject c in ActivePlayer.Units)
            {
                c.UpdateTurn();
            }
        }

        public void EndTurn()
        {
            foreach (LivingObject c in ActivePlayer.Units)
            {
                c.EndTurn();
                //c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
            }
            ActivePlayerNumber++;
            ActivePlayer = Players[ActivePlayerNumber];
            Debug.Log(ActivePlayer.Units[0].Name);
            mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter = null;
            Debug.Log("EndTurn" + ActivePlayerNumber);
            StartTurn();
        }
    }
}
