using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Menu;
using UnityEngine;

public class UICampaignController : UIMenu
{

    public void StartClicked()
    {
        
        SceneController.SwitchScene("WorldMap");
    }
}
