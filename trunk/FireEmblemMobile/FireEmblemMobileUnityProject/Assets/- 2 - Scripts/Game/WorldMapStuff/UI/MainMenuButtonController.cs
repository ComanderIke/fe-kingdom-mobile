using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Controller;
using UnityEngine;

public class MainMenuButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenuClicked()
    {
        WorldMapSceneController.Instance.LoadMainMenu(true);
    }
}
