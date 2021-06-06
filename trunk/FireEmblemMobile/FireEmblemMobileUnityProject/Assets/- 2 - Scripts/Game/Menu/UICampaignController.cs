using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEngine;

public class UICampaignController : UIMenu
{

   

    public void StartCampaign(int selected)
    {
        Campaign.Instance.LoadConfig(GameData.Instance.campaigns[selected]);
       
        SceneController.SwitchScene(GameData.Instance.campaigns[selected].scene);
    }
}
