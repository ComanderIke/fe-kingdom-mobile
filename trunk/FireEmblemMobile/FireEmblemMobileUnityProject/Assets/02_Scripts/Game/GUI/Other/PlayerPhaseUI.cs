using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Mechanics;
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
    [SerializeField] private UICharacterViewController characterView;
    [SerializeField] private UICharacterViewController enemyView;

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
        if(unit.IsPlayerControlled())
            characterView.Show(unit);
        else
            enemyView.Show(unit);
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

    private void OnDestroy()
    {
        Debug.Log("PLAYERPHASE UI GETS DESTROYED HERE =======================");
    }

    public void Hide()
    {
        //Debug.Log("Hide");
        if (this!=null &&gameObject != null)
        {
            GetComponent<Canvas>().enabled = false;
            if(selectionUI!=null&&selectionUI.gameObject!=null)
                selectionUI.HideUndo();
        }

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
            
                Debug.Log("Ending Turn");
                GridGameManager.Instance.GetSystem<TurnSystem>().OnTriggerEndTurn();
           
        });
    }

    
}