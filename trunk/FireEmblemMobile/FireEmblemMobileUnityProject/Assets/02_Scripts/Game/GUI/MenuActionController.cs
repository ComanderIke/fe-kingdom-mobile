using Game.EncounterAreas.Model;
using Game.Menu;
using Game.SerializedData;
using UnityEngine;

namespace Game.GUI
{
    public class MenuActionController : MonoBehaviour
    {
        public static void StartGame()
        {
            SaveGameManager.Save();
            SceneTransferData.Instance.Reset();
            //SceneTransferData.Instance.TutorialBattle1 = true;
            SceneController.LoadSceneAsync(Scenes.EncounterArea, false);
        }
    }
}
