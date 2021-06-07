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
            BattleTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
            SceneController.UnLoadSceneAsync(Scenes.WM_Gameplay);
            SceneController.UnLoadSceneAsync(Scenes.Campaign1);
            SceneController.LoadSceneAsync(Scenes.InsideLocation,true);
        }
        public void LoadBattleLevel(Party playerParty, Party enemyParty)
        {
            BattleTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
            BattleTransferData.Instance.EnemyUnits = enemyParty.members;
            SceneController.UnLoadSceneAsync(Scenes.WM_Gameplay);
            SceneController.UnLoadSceneAsync(Scenes.Campaign1);
            SceneController.LoadSceneAsync(Scenes.Level2, true);
        }
        public void LoadWorldMapFromInside()
        {
            SceneController.UnLoadSceneAsync(Scenes.InsideLocation);
            SceneController.LoadSceneAsync(Scenes.Campaign1, true);
            SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }

        public void LoadWorldMapFromBattle()
        {
            SceneController.UnLoadSceneAsync(Scenes.Level2);
            SceneController.LoadSceneAsync(Scenes.Campaign1, true);
            SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }

        public void UnloadAllExceptMainMenu()
        {
            SceneController.UnLoadSceneAsync(Scenes.InsideLocation);
            SceneController.UnLoadSceneAsync(Scenes.Level2);
            SceneController.UnLoadSceneAsync(Scenes.WM_Gameplay);
            SceneController.UnLoadSceneAsync(Scenes.UI);
            SceneController.UnLoadSceneAsync(Scenes.Campaign1);
            SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }
    }
}
