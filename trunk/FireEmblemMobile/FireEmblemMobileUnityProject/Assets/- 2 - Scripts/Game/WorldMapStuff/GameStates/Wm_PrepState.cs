using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.Manager;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Serialization;
using Game.WorldMapStuff.Systems;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.WorldMapStuff.GameStates
{
    public class Wm_PrepState: GameState<NextStateTrigger>
    {
        public override void Enter()
        {
            if(SceneTransferData.Instance.BattleOutCome!=BattleOutcome.None)
                AfterBattleEvents();
            PrepFinished();
        }

        private void AfterBattleEvents()
        {
            var selectionSystem = WorldMapGameManager.Instance.GetSystem<WM_PartySelectionSystem>();
            WM_Faction  playerFaction = (WM_Faction)WorldMapGameManager.Instance.FactionManager.GetPlayerControlledFaction();
            Party lastSelectedParty =
                playerFaction.Parties.FirstOrDefault(p => p.name == SceneTransferData.Instance.PartyID);
            LocationController locationOfEnemy = WorldMapGameManager.Instance.World.Locations.FirstOrDefault(l=> l.UniqueId==SceneTransferData.Instance.LocationData.LocationId);
           
            var action = new AttackFinishedAction(lastSelectedParty, (Party) locationOfEnemy.Actor,selectionSystem, SceneTransferData.Instance.BattleOutCome);
            action.PerformAction();
            action.Save(SaveData.currentSaveData);
            
     
        }

        private void PrepFinished()
        {
            WorldMapGameManager.Instance.GameStateManager.Feed(NextStateTrigger.FinishedPreparation);
        }
        public override void Exit()
        {
        }

        public override GameState<NextStateTrigger> Update()
        {
            return null;

        }
    }
}