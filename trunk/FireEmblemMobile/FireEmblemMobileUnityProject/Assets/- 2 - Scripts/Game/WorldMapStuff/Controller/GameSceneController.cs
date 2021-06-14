using System;
using Game.Manager;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Menu;

namespace Game.WorldMapStuff.Controller
{
    public class GameSceneController
    {
        private static GameSceneController instance;

        public static GameSceneController Instance => instance ??= new GameSceneController();

        public void LoadInside(Party playerParty)
        {
            SceneTransferData.Instance.Reset();
            SceneTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
            SceneTransferData.Instance.PartyID = playerParty.name;
            SceneTransferData.Instance.LocationId = playerParty.location.UniqueId;
            SceneController.UnLoadSceneAsync(Scenes.WM_Gameplay);
            SceneController.UnLoadSceneAsync(Scenes.Campaign1);
            SceneController.LoadSceneAsync(Scenes.InsideLocation,true);
        }
        public void LoadBattleLevel(Party playerParty, Party enemyParty)
        {
            SceneTransferData.Instance.Reset();
            SceneTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
            SceneTransferData.Instance.EnemyUnits = enemyParty.members;
            SceneTransferData.Instance.PartyID = playerParty.name;
            SceneTransferData.Instance.EnemyPartyID = enemyParty.name;
            SceneTransferData.Instance.LocationId = enemyParty.location.UniqueId;
            SceneController.UnLoadSceneAsync(Scenes.WM_Gameplay);
            SceneController.UnLoadSceneAsync(Scenes.Campaign1);
            SceneController.LoadSceneAsync(Scenes.Level2, true);
        }
        public void LoadWorldMapFromInside()
        {
            SceneTransferData.Instance.Reset();
            SceneController.UnLoadSceneAsync(Scenes.InsideLocation);
            SceneController.LoadSceneAsync(Scenes.Campaign1, true);
            SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }

        public void LoadWorldMapBeforeBattle()
        {
            SceneTransferData.Instance.Reset();
          
            SceneController.UnLoadSceneAsync(Scenes.Level2);
            SceneController.LoadSceneAsync(Scenes.Campaign1, true);
            SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
            SceneController.OnSceneCompletelyFinished += InvokeBattleFinished;
        }

        private void InvokeBattleFinished()
        {
            SceneController.OnSceneCompletelyFinished -= InvokeBattleFinished;
            SceneTransferData.Instance.BattleFinished?.Invoke();
        }
        public void LoadWorldMapAfterBattle(bool victory)
        {
            SceneTransferData.Instance.BattleOutCome = victory?BattleOutcome.Victory:  BattleOutcome.Defeat;
            SceneController.UnLoadSceneAsync(Scenes.Level2);
            SceneController.LoadSceneAsync(Scenes.Campaign1, true);
            SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
            SceneController.OnSceneCompletelyFinished += InvokeBattleFinished;
        }

        public void UnloadAllExceptMainMenu()
        {
            SceneTransferData.Instance.Reset();
            SceneController.UnLoadSceneAsync(Scenes.InsideLocation);
            SceneController.UnLoadSceneAsync(Scenes.Level2);
            SceneController.UnLoadSceneAsync(Scenes.WM_Gameplay);
            SceneController.UnLoadSceneAsync(Scenes.UI);
            SceneController.UnLoadSceneAsync(Scenes.Campaign1);
            SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }
    }
}
