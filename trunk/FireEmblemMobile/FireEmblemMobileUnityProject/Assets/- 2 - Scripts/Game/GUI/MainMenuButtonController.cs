using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using Game.WorldMapStuff.Controller;
using UnityEngine;

public class MainMenuButtonController : MonoBehaviour
{


    public void MainMenuClicked()
    {
        MainMenuController.Instance.ShowMainMenu();
    }
}
