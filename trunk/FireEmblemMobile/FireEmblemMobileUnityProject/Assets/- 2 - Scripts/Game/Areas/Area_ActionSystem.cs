using System;
using Game.GameActors.Players;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Serialization;
using GameEngine;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    public class Area_ActionSystem:IEngineSystem
    {
        private void AttackFinished()
        {
            // var action = new AttackFinishedAction(Player.Instance.Party, EncounterNode, SceneTransferData.Instance.BattleOutCome);
            // action.PerformAction();
            // action.Save(SaveData.currentSaveData);
        }

        public void Move(EncounterNode location)
        {  
           // Debug.Log("Move Party");
            var action = new MoveAction(location);
                action.PerformAction();
                action.Save(SaveData.currentSaveData);
                

        }

        public void Init()
        {
            
        }

        public void Deactivate()
        {
            
        }

        public void Activate()
        {
            
        }
    }
}