using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEngine;

namespace LostGrace
{
    public class MenuActionController : MonoBehaviour
    {
        public static void StartGame()
        {
            SceneTransferData.Instance.Reset();
            //SceneTransferData.Instance.TutorialBattle1 = true;
            SceneController.LoadSceneAsync(Scenes.EncounterArea, false);
        }
    }
}
