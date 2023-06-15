using System.Collections;
using System.Collections.Generic;
using Game.Manager;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEngine;

namespace LostGrace
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
