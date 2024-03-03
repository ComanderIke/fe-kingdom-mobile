using System;
using System.Collections.Generic;
using Game.Dialog;
using Game.GameActors.Items;
using Game.GameActors.Player;
using UnityEngine;

namespace Game.EncounterAreas.Encounters.Event
{
    [CreateAssetMenu(menuName = "GameData/Events/MiniGames/MemoryGameData", fileName="MemoryGameData")]
    public class MemoryGameData: MiniGame
    {
        public int MaxTries = 5;
        public List<ItemBP> items;
        public int columns=5;
        public int hpCost = 0;
        private MemoryMiniGame miniGame;

        public override void StartGame()
        {
            miniGame=GameObject.FindObjectOfType<MemoryMiniGame>();
            miniGame.OnComplete -= Complete;
            miniGame.OnComplete += Complete;
            miniGame.Show(this, Player.Instance.Party);
        
        }

        void Complete()
        {
            OnComplete?.Invoke();
        }
        public override Reward GetRewards()
        {
            return GameObject.FindObjectOfType<MemoryMiniGame>().GetRewards();
        }

        public override event Action OnComplete;
    }
}