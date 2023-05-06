using System;
using System.Collections;
using Game.GameActors.Players;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Serialization;
using GameEngine;
using LostGrace;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    public class Area_ActionSystem
    {
        public void Move(EncounterNode location)
        {  
           // Debug.Log("Move Party");
            var action = new MoveAction(location);
                action.PerformAction();
                SaveGameManager.Save();
        }
    }
}