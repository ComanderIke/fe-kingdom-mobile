using System.Collections;
using System.Collections.Generic;
using Game.Mechanics;
using UnityEngine;

public class PlayerPhaseUI : MonoBehaviour, IPlayerPhaseUI
{
    public OKCancelDialogController OkCancelDialogController;

    public void Show()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void EndTurnClicked()
    {
        OkCancelDialogController.Show("End the Turn?", () => { TurnSystem.OnTriggerEndTurn(); });
    }
}
