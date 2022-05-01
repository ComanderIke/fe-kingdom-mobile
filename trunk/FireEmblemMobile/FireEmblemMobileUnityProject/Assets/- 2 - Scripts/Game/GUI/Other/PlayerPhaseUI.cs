using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerPhaseUI : MonoBehaviour, IPlayerPhaseUI
{
    public SelectionUI selectionUI;
    public OKCancelDialogController OkCancelDialogController;
    public TextMeshProUGUI turnText;
    public TileInfoPanel TileInfoPanel;

    public static Action<Unit> OnUnitCircleClicked;
    public void ShowTileInfo(Tile selectedTile)
    {
        //Debug.Log("ShowTileInfo!");
        TileInfoPanel.Show(selectedTile);
    }
    public void HideTileInfo()
    {
        //Debug.Log("HideTileInfo!");
        TileInfoPanel.Hide();
    }

    public void SubscribeOnBackClicked(Action action)
    {
        //Debug.Log("Subscribe BACK!");
        SelectionUI.OnBackClicked += action;
    }

    

    public void UnsubscribeOnBackClicked(Action action)
    {
        SelectionUI.OnBackClicked -= action;
    }

    public void SubscribeOnCharacterCircleClicked(Action<Unit> action)
    {
        OnUnitCircleClicked += action;
    }

    public void UnsubscribeOnCharacterCircleClicked(Action<Unit> action)
    {
        OnUnitCircleClicked -= action;
    }

    public void ViewUnit(Unit unit)
    {
        FindObjectOfType<UICharacterViewController>().Show(unit);
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
        //Debug.Log("Hide");
        GetComponent<Canvas>().enabled = false;
        selectionUI.HideUndo();

    }

    public void UnitCircleClicked(Unit u)
    {
        OnUnitCircleClicked?.Invoke(u);
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