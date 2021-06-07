using System;
using Game.Manager;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    public class WorldMapSceneController : MonoBehaviour
    {
        // Start is called before the first frame update
        public static WorldMapSceneController Instance;
        public GameObject DisableObject;
        void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
            {
                Instance = this;
                GameManager.Instance.SessionManager.WorldMapLoaded = true;

            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Cleanup()
        {
            WorldMapGameManager.Instance.CleanUp();
        }
        public void LoadInside(Party playerParty)
        {
            SceneController.OnBeforeSceneReady += Hide;
            BattleTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
            Cleanup();
            SceneController.SwitchScene(Scenes.InsideLocation);
        }
        public void LoadBattleLevel(Party playerParty, Party enemyParty)
        {
            BattleTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
            BattleTransferData.Instance.EnemyUnits = enemyParty.members;
            SceneController.OnBeforeSceneReady += Hide;
            Cleanup();
            SceneController.SwitchScene(Scenes.Level2);
        }

        public void LoadWorldMap()
        {
            SceneController.OnSceneReady+=Show;
            SceneController.SwitchScene(Scenes.WorldMap);
        }

        private bool lastBattleVictory = false;
        public void FinishedBattle(bool victory)
        {
            GridGameManager.Instance.Deactivate();
            SceneController.OnSceneReady+=Show;
            lastBattleVictory = victory;
            SceneController.OnBeforeSceneReady += InvokeBattleFinished;
            SceneController.SwitchScene(Scenes.WorldMap);
       
        }

        private void InvokeBattleFinished()
        {
            SceneController.OnBeforeSceneReady -= InvokeBattleFinished;
            OnBattleFinished?.Invoke(lastBattleVictory);
        }
        private void Hide()
        {
            WorldMapGameManager.Instance.Deactivate();
            SceneController.OnBeforeSceneReady -= Hide;
            DisableObject.SetActive(false);
        
        }
    
        private void Show()
        {
            SceneController.OnSceneReady-=Show;
            WorldMapGameManager.Instance.Activate();
            DisableObject.SetActive(true);
      
        }

        public event Action<bool> OnBattleFinished;

        public void LoadMainMenu(bool async = false)
        {
            if (async)
            {
                SceneController.OnSceneReady += Hide;
                DisableObject.SetActive(false);
                SceneController.SwitchScene(Scenes.MainMenu);
                
            }
            else
            {
                Cleanup();
                SceneController.SwitchScene(Scenes.MainMenu);
                Destroy(this.gameObject);
            }
        }
    }
}
