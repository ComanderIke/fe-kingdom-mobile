using Assets.Scripts.Players;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameStates
{
    internal class GameOverState : GameState
    {
        Player loser;
        public GameOverState(Player loser)
        {
            this.loser = loser;
        }
        public override void Enter()
        {
            Debug.Log("Player " + loser.Name + " lost");
        }

        public override void Exit()
        {
        }

        public override void Update()
        {

        }
    }
}