using System.Collections;
using System.Collections.Generic;
using Game.Grid;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Systems;
using TMPro;
using UnityEngine;

public class PlayerPhaseUI : MonoBehaviour, IPlayerPhaseUI
{
    public OKCancelDialogController OkCancelDialogController;
    public TextMeshProUGUI turnText;
    public TileInfoPanel TileInfoPanel;
    public GameObject BackButton;
    public void ShowTileInfo(Tile selectedTile)
    {
        TileInfoPanel.Show(selectedTile);
    }
    public void HideTileInfo()
    {
        TileInfoPanel.Hide();
    }
    public void Show(int turnCount)
    {
        GetComponent<Canvas>().enabled = true;
        turnText.SetText("Turn "+turnCount);
        
    }

    public void ShowBackButton()
    {
        BackButton.SetActive(true);
    }
    public void HideBackButton()
    {
        BackButton.SetActive(false);
    }

    public void Hide()
    {
        GetComponent<Canvas>().enabled = false;
       
    }

    public void BackClicked()
    {
       HideBackButton();
    }
   
    public void EndTurnClicked()
    {
        OkCancelDialogController.Show("Do you want to end the turn?", () =>
        {
            //Debug.Log("Ending Turn with UI!");
            if(GridGameManager.Instance!=null)
                GridGameManager.Instance.GetSystem<TurnSystem>().OnTriggerEndTurn();
            else
            {
                WorldMapGameManager.Instance.GetSystem<TurnSystem>().OnTriggerEndTurn();
            }
        });
    }
}
