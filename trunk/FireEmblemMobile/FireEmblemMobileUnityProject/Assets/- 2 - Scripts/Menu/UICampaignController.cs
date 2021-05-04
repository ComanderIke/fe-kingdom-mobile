using System.Collections;
using System.Collections.Generic;
using Menu;
using UnityEngine;

public class UICampaignController : UIMenu
{

    public void StartClicked()
    {
        SceneController.SwitchScene("Base");
    }
}
