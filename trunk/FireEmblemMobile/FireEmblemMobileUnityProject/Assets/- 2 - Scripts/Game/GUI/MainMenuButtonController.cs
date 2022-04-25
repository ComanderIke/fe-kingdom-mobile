using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GUI;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Menu;
using SerializedData;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{


    public void MainMenuClicked()
    {
        SaveSystem.SaveGame("AutoSave", new SaveData(Player.Instance, Campaign.Instance));
        GameSceneController.Instance.UnloadAllExceptMainMenu();
        
    }
}
