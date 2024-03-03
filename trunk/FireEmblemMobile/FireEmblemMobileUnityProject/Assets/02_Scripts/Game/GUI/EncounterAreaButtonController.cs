using Game.EncounterAreas.Model;
using Game.Manager;
using Game.Menu;
using Game.SerializedData;
using UnityEngine;

namespace Game.GUI
{
    public class EncounterAreaButtonController : MonoBehaviour
    {
        public void Clicked()
        {
            SaveGameManager.Save();
            GridGameManager.Instance.CleanUp();
            SceneController.LoadSceneAsync(Scenes.EncounterArea, false);
        }
    }
}
