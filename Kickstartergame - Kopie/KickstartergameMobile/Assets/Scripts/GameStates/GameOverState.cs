using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameStates
{
    internal class GameOverState : GameState
    {
        Player loser;
        GameObject gameOverScreen;
        public GameOverState(Player loser,GameObject gameOverScreen)
        {
            this.loser = loser;
            this.gameOverScreen = gameOverScreen;
        }
        public override void enter()
        {
            Debug.Log("Player " + loser.name + " lost");
            GameObject go = GameObject.Instantiate(gameOverScreen);
            go.transform.SetParent(GameObject.Find("Canvas").transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            //MainScript.players.Remove(loser); TODO only 2 player?
            if (MainScript.players.Count == 1)
            {
                Player winner = MainScript.players[0];
                Debug.Log("Player " + winner.name + " won");
                GameObject.Find("WinnerText").GetComponent<Text>().text = MainScript.players[0].name;
            }
        }

        public override void exit()
        {
            GameObject.Find("TransferToNextScene").GetComponent<TransferData>().player = MainScript.players[0];
            Application.LoadLevel("Levelauswahl");
        }

        public override void update()
        {

        }
    }
}