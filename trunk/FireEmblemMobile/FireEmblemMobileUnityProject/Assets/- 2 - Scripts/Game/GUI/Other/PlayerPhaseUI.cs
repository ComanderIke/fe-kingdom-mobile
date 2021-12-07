using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Systems;
using TMPro;
using UnityEngine;

public class PlayerPhaseUI : MonoBehaviour, IPlayerPhaseUI
{
    public SelectionUI selectionUI;
    public OKCancelDialogController OkCancelDialogController;
    public TextMeshProUGUI turnText;
    public TileInfoPanel TileInfoPanel;
    public static Action OnBackClicked;
    public void ShowTileInfo(Tile selectedTile)
    {
        TileInfoPanel.Show(selectedTile);
    }
    public void HideTileInfo()
    {
        TileInfoPanel.Hide();
    }

    public void SubscribeOnBackClicked(Action action)
    {
        //Debug.Log("Subscribe BACK!");
        OnBackClicked += action;
    }

    

    public void UnsubscribeOnBackClicked(Action action)
    {
        OnBackClicked -= action;
    }
    public void Show(int turnCount)
    {
        GetComponent<Canvas>().enabled = true;
        turnText.SetText("Turn "+turnCount);
        
    }

    // public void ShowBackButton()
    // {
    //     BackButton.SetActive(true);
    // }
    // public void HideBackButton()
    // {
    //     BackButton.SetActive(false);
    // }

    public void Hide()
    {
        Debug.Log("Hide");
        GetComponent<Canvas>().enabled = false;
        selectionUI.HideUndo();

    }

    public void BackClicked()
    {
        Debug.Log("BACK Clicked!");
        
        OnBackClicked?.Invoke();
        Invoke(nameof(HideUndo),0.05f);//Invoke after small time so the raycast of the button click doesnt go to the grid....
        
       
    }

    private void HideUndo()
    {
        selectionUI.HideUndo();
        
    }
   
    public void EndTurnClicked()
    {
        OkCancelDialogController.Show("Do you want to end the turn?", () =>
        {
            Debug.Log("Ending Turn clicked with UI!");
            if (GridGameManager.Instance != null)
            {
                Debug.Log("Ending Turn");
                GridGameManager.Instance.GetSystem<TurnSystem>().OnTriggerEndTurn();
            }
            else
            {
                Debug.Log("World Map Ending Turn with UI!");
                WorldMapGameManager.Instance.GetSystem<TurnSystem>().OnTriggerEndTurn();
            }
        });
    }
}
