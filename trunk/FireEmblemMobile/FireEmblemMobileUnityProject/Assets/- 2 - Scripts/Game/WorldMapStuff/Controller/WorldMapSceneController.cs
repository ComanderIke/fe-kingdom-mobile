using System.Collections;
using System.Collections.Generic;
using Game.Manager;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Menu;
using UnityEditor.SearchService;
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

    public void LoadLevel(Party playerParty, Party enemyParty)
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
    private void Hide()
    {
        WorldMapGameManager.Instance.Deactivate();
        SceneController.OnSceneReady -= Hide;
        DisableObject.SetActive(false);
        
    }
    
    private void Show()
    {
        DisableObject.SetActive(true);
        WorldMapGameManager.Instance.Activate();
    }
}
