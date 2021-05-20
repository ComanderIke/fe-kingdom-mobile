using System.Collections;
using System.Collections.Generic;
using Game.Mechanics;
using Game.WorldMapStuff.Input;
using Game.WorldMapStuff.Systems;
using TMPro;
using UnityEngine;

public class PlayerPhaseUI : MonoBehaviour, IPlayerPhaseUI, IPartyActionRenderer
{
    public OKCancelDialogController OkCancelDialogController;
    public TextMeshProUGUI turnText;
    public GameObject splitButton;
    public GameObject joinButton;
    public void Show(int turnCount)
    {
        GetComponent<Canvas>().enabled = true;
        turnText.SetText("Turn "+turnCount);
    }

    public void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void ShowJoinButton()
    {
        joinButton.SetActive(true);
    }
    public void HideJoinButton()
    {
        joinButton.SetActive(false);
    }
    public void ShowSplitButton()
    {
        splitButton.SetActive(true);
    }
    public void HideSplitButton()
    {
        splitButton.SetActive(false);
    }
    public void SplitClicked()
    {
        Debug.Log("Split clicked!");
        WM_PartyActionSystem.OnSplitClicked?.Invoke();
    }
    public void JoinClicked()
    {
        Debug.Log("Join Clicked!");
        WM_PartyActionSystem.OnJoinClicked?.Invoke();
    }
    public void EndTurnClicked()
    {
        OkCancelDialogController.Show("End the Turn?", () =>
        {
            Debug.Log("Ending Turn with UI!");TurnSystem.OnTriggerEndTurn(); });
    }
}
