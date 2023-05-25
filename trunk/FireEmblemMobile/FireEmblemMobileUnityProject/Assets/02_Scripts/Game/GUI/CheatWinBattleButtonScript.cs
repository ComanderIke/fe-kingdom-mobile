using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace LostGrace
{
    public class CheatWinBattleButtonScript : MonoBehaviour
    {
       
        void Start()
        {
        
        }

        void Update()
        {
        
        }

        public void Clicked()
        {
            Player.Instance.LastBattleOutcome = BattleOutcome.Victory;
            GameSceneController.Instance.LoadWorldMapAfterBattle(true);
        }
    }
}
