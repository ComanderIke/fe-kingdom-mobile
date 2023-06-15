using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GUI;
using Game.Manager;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using LostGrace;
using Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

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
