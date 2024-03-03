using Game.EncounterAreas.Model;
using Game.Manager;
using Game.Menu;
using Game.SerializedData;
using UnityEngine;

namespace Game.GUI.Controller
{
    public class MainMenuButtonController : MonoBehaviour
    {


        public void MainMenuClicked()
        {
            Debug.Log("MainMenu ClickedS!");
            SaveGameManager.Save();
            GridGameManager.Instance.CleanUp();
            SceneController.LoadSceneAsync(Scenes.MainMenu, false);
        
        }
    }
}
