using System;
using Game.Manager;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.WorldMapStuff.Controller
{
    public class WorldMapSceneController : MonoBehaviour
    {
        // Start is called before the first frame update
        public static WorldMapSceneController Instance;
       // public GameObject DisableObject;
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

      
        public void LoadInside(Party playerParty)
        {
            // SceneController.OnBeforeSceneReady += Hide;
            BattleTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
            SceneController.UnloadScene(Scenes.WM_Gameplay).completed+= (AsyncOperation) =>
            {
                Debug.Log("Unloading Gameplay Complete");
                SceneController.UnloadScene(Scenes.Campaign1).completed += (AsyncOperation) =>
                {
                    Debug.Log("Unloading Campaign1 Complete");
                    SceneController.LoadSceneAsync(Scenes.InsideLocation,true);
                };
            };
            
           
            
        }
        public void LoadBattleLevel(Party playerParty, Party enemyParty)
        {
            BattleTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
            BattleTransferData.Instance.EnemyUnits = enemyParty.members;
            // SceneController.OnBeforeSceneReady += Hide;

            SceneController.LoadSceneAsync(Scenes.Level2, true);
        }

        public void LoadWorldMap()
        {
            // SceneController.OnSceneReady+=Show;
            SceneController.LoadSceneAsync(Scenes.Campaign1,true);
        }

        private bool lastBattleVictory = false;
        public void FinishedBattle(bool victory)
        {
            GridGameManager.Instance.Deactivate();
            // SceneController.OnSceneReady+=Show;
            lastBattleVictory = victory;
            SceneController.OnBeforeSceneReady += InvokeBattleFinished;
            SceneController.LoadSceneAsync(Scenes.Campaign1, true);
       
        }

        private void InvokeBattleFinished()
        {
            SceneController.OnBeforeSceneReady -= InvokeBattleFinished;
            OnBattleFinished?.Invoke(lastBattleVictory);
        }
        // private void Hide()
        // {
        //     WorldMapGameManager.Instance.Deactivate();
        //     SceneController.OnBeforeSceneReady -= Hide;
        //    // DisableObject.SetActive(false);
        //
        // }
    
        // private void Show()
        // {
        //     SceneController.OnSceneReady-=Show;
        //     WorldMapGameManager.Instance.Activate();
        //    // DisableObject.SetActive(true);
        //
        // }

        public event Action<bool> OnBattleFinished;

        public void LoadOnlyMainMenu()
        {
            SceneController.LoadSceneAsync(Scenes.MainMenu, false);
        }

        public void LoadWorldMapFromInside()
        {
            SceneController.UnloadScene(Scenes.InsideLocation);
            SceneController.LoadSceneAsync(Scenes.Campaign1, true);
            SceneController.OnSceneCompletelyFinished += LoadGameplayScene;
            
            
        }

        private void LoadGameplayScene()
        {
            SceneController.OnSceneCompletelyFinished -= LoadGameplayScene;
            SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }
    }
}
