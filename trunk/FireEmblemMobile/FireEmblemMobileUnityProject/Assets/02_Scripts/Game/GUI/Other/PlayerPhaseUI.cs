using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using Game.WorldMapStuff.Systems;
using LostGrace;
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
    [SerializeField] private TextMeshProUGUI endTurnButtonText;
    [SerializeField] private BossUI bossUI;
    private static PlayerPhaseUI instance;
   // [SerializeField] private UICharacterViewController enemyView;
   private void Start()
   {
       instance = this;
   }

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
        // else
        //     enemyView.Show(unit);
    }

    public void ShowBossUI(Unit unit)
    {
        bossUI.Show(unit);
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

    public static void StaticHide()
    {
        if(instance!=null && instance.gameObject!=null)
            instance.Hide();
    }

    public void UnitCircleClicked(Unit u)
    {
        OnUnitCircleClicked?.Invoke(u);
    }


    private bool buttonAnimated = false;
    //TODO PERFORMANCE? only check on any unit waited?
    private void Update()
    {
        bool allWaiting = true;
        foreach (var unit in GridGameManager.Instance.FactionManager.ActiveFaction.FieldedUnits)
        {
            if (!unit.TurnStateManager.IsWaiting)
                allWaiting = false;
        }

        if (allWaiting)
        {
            if(!buttonAnimated)
                endTurnButtonText.text = "<bounce><shinier>End Turn";
            buttonAnimated = true;
        }
        else
        {
           
            if(buttonAnimated)
                endTurnButtonText.text = "</bounce></shinier>End Turn";
            buttonAnimated = false;
        }
       
    }


    public void ToggleDangerArea()
    {
        GridGameManager.Instance.GetSystem<GridSystem>().ToggleDangerArea();
    }
    public void EndTurnClicked()
    {
        OkCancelDialogController.Show("Do you want to end the turn?", () =>
        {
           
            
                MyDebug.LogLogic("Ending Turn");
                GridGameManager.Instance.GetSystem<TurnSystem>().OnTriggerEndTurn();
           
        });
    }

    
}