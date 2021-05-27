using System;
using System.Collections;
using System.Collections.Generic;
using Game.Manager;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEngine;

public class WorldMapSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public static WorldMapSceneController Instance;
    public GameObject DisableObject;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadInside(Party playerParty)
    {
        SceneController.OnSceneReady += Hide;
        BattleTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
        SceneController.SwitchScene("InsideLocation");
    }
    public void LoadBattleLevel(Party playerParty, Party enemyParty)
    {
        BattleTransferData.Instance.PlayerName = playerParty.Faction.name;
        BattleTransferData.Instance.UnitsGoingIntoBattle = playerParty.members;
        BattleTransferData.Instance.EnemyUnits = enemyParty.members;
        SceneController.OnSceneReady += Hide;
        SceneController.SwitchScene("Level2");
    }

    public void LoadWorldMap()
    {
        Show();
        SceneController.SwitchScene("WorldMap");
    }

    private bool lastBattleVictory = false;
    public void FinishedBattle(bool victory)
    {
        GridGameManager.Instance.Deactivate();
        Show();
        lastBattleVictory = victory;
        SceneController.OnSceneReady += InvokeBattleFinished;
        SceneController.SwitchScene("WorldMap");
       
    }

    private void InvokeBattleFinished()
    {
        SceneController.OnSceneReady -= InvokeBattleFinished;
        OnBattleFinished?.Invoke(lastBattleVictory);
    }
    private void Hide()
    {
        WorldMapGameManager.Instance.Deactivate();
        SceneController.OnSceneReady -= Hide;
        DisableObject.SetActive(false);
        
    }
    
    private void Show()
    {
        WorldMapGameManager.Instance.Activate();
        DisableObject.SetActive(true);
      
    }

    public event Action<bool> OnBattleFinished;
}
