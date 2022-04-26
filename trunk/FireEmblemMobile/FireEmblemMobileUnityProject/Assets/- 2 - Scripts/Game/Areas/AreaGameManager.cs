using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Areas;
using Effects;
using Game.GameActors.Players;
using Game.Systems;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;

public class AreaGameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private Area_ActionSystem actionSystem;
    public GameObject attackOptionPrefab;
    public GameObject moveOptionPrefab;
    public EncounterUIController uiCOntroller;
    public UIPartyCharacterCircleController uiPartyController;
    public Party playerStartParty;
    public ColumnManager ColumnManager;
    // Start is called before the first frame update
    public Transform spawnParent;
    private List<GameObject> moveOptions=new List<GameObject>();
    public float offsetBetweenCharacters = 0.25f;
    public int hour = 6;
    public TimeCircleUI circleUI;
    public DynamicAmbientLight lightController;
    void Start()
    {
        cursor = FindObjectOfType<EncounterCursorController>();
        //Debug.Log("WHY IS START NOT CALLED?");
        actionSystem = new Area_ActionSystem();
        if(PlayerPrefs.HasKey("CameraX")&&PlayerPrefs.HasKey("CameraY"))
        {
            WorldMapCameraController camera = FindObjectOfType<WorldMapCameraController>();
            camera.transform.position = new Vector3(PlayerPrefs.GetFloat("CameraX"), PlayerPrefs.GetFloat("CameraY"),
                camera.transform.position.z);
        }

        if (LoadedSaveData())
        {
            Debug.Log("Use party saveData");
            Player.Instance.Party.Initialize();
            SaveData.currentSaveData.playerData.partyData.LoadEncounterAreaData(Player.Instance.Party, EncounterTree.Instance.columns);
            
            
        }
        else
        {
            //Debug.Log("Player Null");
            Player.Instance.Party = Instantiate(playerStartParty, spawnParent);
            Player.Instance.Party.Initialize();
            Player.Instance.Party.EncounterNode = EncounterTree.Instance.startNode;
            Player.Instance.Party.MovedEncounters.Add(EncounterTree.Instance.startNode);
        }


        
            //Debug.Log("Player Node Null");
           
            
      
        SpawnPartyMembers();
        uiCOntroller.Init(Player.Instance.Party);
       
        
        ResetMoveOptions();
  
        ShowMoveOptions();
        uiPartyController.Show(Player.Instance.Party);
        lightController.UpdateHour(hour);
    }
    private bool LoadedSaveData()
    {
        return SaveData.currentSaveData != null && SaveData.currentSaveData.playerData != null;
    }
  
    private List<EncounterPlayerUnitController> partyGameObjects;
    void SpawnPartyMembers()
    {
        int cnt = 1;
        var partyGo = new GameObject("Partytest");
        partyGo.transform.SetParent(spawnParent);
        partyGo.transform.position = Player.Instance.Party.EncounterNode.gameObject.transform.position;
        partyGameObjects = new List<EncounterPlayerUnitController>();
        //Spawn ActiveUnit first
        var activeUnit = Player.Instance.Party.members[Player.Instance.Party.ActiveUnitIndex];
        var go = Instantiate(activeUnit.visuals.Prefabs.EncounterAnimatedSprite, partyGo.transform, false);
        go.transform.localPosition = new Vector3(0,0,0);
        go.GetComponent<EncounterPlayerUnitController>().SetUnit(activeUnit);
        ShowActiveUnit(go.transform.position);
        go.GetComponent<EncounterPlayerUnitController>().Show();
        go.GetComponent<EncounterPlayerUnitController>().SetTarget(null);
        go.GetComponent<EncounterPlayerUnitController>().SetSortOrder(Player.Instance.Party.members.Count);
        activeMemberGo = go;

        partyGameObjects.Add( go.GetComponent<EncounterPlayerUnitController>());
        foreach (var member in Player.Instance.Party.members)
        {
            if (member == activeUnit)
                continue;
            
            go = Instantiate(member.visuals.Prefabs.EncounterAnimatedSprite, partyGo.transform, false);
            go.transform.localPosition = new Vector3(-offsetBetweenCharacters*cnt,0,0);
            go.GetComponent<EncounterPlayerUnitController>().Hide();
            go.GetComponent<EncounterPlayerUnitController>().SetUnit(member);
            go.GetComponent<EncounterPlayerUnitController>().SetTarget(activeMemberGo.transform);
            go.GetComponent<EncounterPlayerUnitController>().SetOffsetCount(cnt);
            go.GetComponent<EncounterPlayerUnitController>().SetSortOrder(Player.Instance.Party.members.Count-cnt);
            cnt++;
            partyGameObjects.Add(go.GetComponent<EncounterPlayerUnitController>());
        }
        Player.Instance.Party.GameObject = partyGo;
    }

    private GameObject activeMemberGo;
    public void UpdatePartyGameObjects()
    {
        Debug.Log("UpdatePartyMembes!");
        var activeUnit = Player.Instance.Party.members[Player.Instance.Party.ActiveUnitIndex];
        int cnt = 1;

        foreach (var unitController in partyGameObjects)
        {
            if (unitController.Unit == activeUnit)
            {
                activeMemberGo = unitController.gameObject;
            }
        }

        foreach (var member in Player.Instance.Party.members)
        {
            if (member == activeUnit)
            {
                foreach (var unitController in partyGameObjects)
                {
                    if (unitController.Unit == member)
                    {
                        unitController.transform.localPosition = new Vector3(0,0,0);
                        ShowActiveUnit(unitController.transform.position);
                        unitController.Show();
                        unitController.SetTarget(null);
                        unitController.SetSortOrder(Player.Instance.Party.members.Count);
                    }
                }
               
            }
            else
            {
                foreach (var unitController in partyGameObjects)
                {
                    if (unitController.Unit == member)
                    {
                        unitController.transform.localPosition = new Vector3(-offsetBetweenCharacters*cnt,0,0);
                        unitController.Hide();
                        unitController.SetTarget(activeMemberGo.transform);
                        unitController.SetOffsetCount(cnt);
                        Debug.Log("SetOffset!");
                        unitController.SetSortOrder(Player.Instance.Party.members.Count-cnt);
                    }
                }
            }

            cnt++;

        }
    }

    private void ResetMoveOptions()
    {
        for (int i = moveOptions.Count - 1; i >= 0; i--)
        {
            Destroy(moveOptions[i]);

            
        }
        foreach (var child in Player.Instance.Party.EncounterNode.children)
        {
            child.moveable = false;
        }
    }

    public GameObject activeUnitEffectPrefab;
    private GameObject activeUnitEffect;
    public void ShowActiveUnit(Vector3 position)
    {
        if (activeUnitEffect == null)
        {
            activeUnitEffect = Instantiate(activeUnitEffectPrefab, spawnParent, false);
            
        }
        activeUnitEffect.transform.position = position;
    }
    public void ShowMoveOptions()
    {
        moveOptions = new List<GameObject>();
        foreach (var child in Player.Instance.Party.EncounterNode.children)
        {
            child.moveable = true;
            GameObject go = null;
            if (child is BattleEncounterNode)
            {
                go = Instantiate(attackOptionPrefab, spawnParent, false);
            }
            else
            {
                go = Instantiate(moveOptionPrefab, spawnParent, false);
            }

            go.transform.position = child.gameObject.transform.position;
            moveOptions.Add(go);
        }
    }

  
    // Update is called once per frame
    void Update()
    {
        
    }

    private EncounterCursorController cursor;
    public void NodeClicked(EncounterNode encounterNode)
    {
        Debug.Log("Node Clicked: "+encounterNode);
        Debug.Log("Node Clicked GO: "+encounterNode.gameObject);
        cursor.SetPosition(encounterNode.gameObject.transform.position);
        if (encounterNode.moveable)
        {

            StartCoroutine(MovementAnimation(encounterNode));
            circleUI.Rotate();
            hour += 6;
            if (hour >= 24)
                hour = 0;
            lightController.UpdateHour(hour);
            // ResetMoveOptions();
            // ShowMoveOptions();
        }
    }
    IEnumerator MovementAnimation(EncounterNode target)
    {
        Vector3 targetPos = target.gameObject.transform.position;
        Vector3 startPos = activeMemberGo.transform.position;
        float distance = Vector3.Distance(targetPos, startPos);
        float time = 0;
        float speed = 3;
        while ( activeMemberGo.transform.position!=targetPos)
        {
            time += Time.deltaTime *speed/distance;
            activeMemberGo.transform.position = Vector3.Lerp(startPos, targetPos, time);
            yield return null;

        }
        actionSystem.Move(target);
        ShowMovedRoads();
        Debug.Log("Before DelayAction!");
        StartCoroutine( DelayAction(()=>target.Activate(Player.Instance.Party), 1.0f));
       
        
    }

    private void ShowMovedRoads()
    {
        for( int i= 0; i < Player.Instance.Party.MovedEncounters.Count-1; i++)
        {
            Road road = Player.Instance.Party.MovedEncounters[i].GetRoad(Player.Instance.Party.MovedEncounters[i + 1]);
            road.SetMovedVisual();
        }
    }

    public void Continue()
    {
        ResetMoveOptions();
        ShowMoveOptions();
    }

    private IEnumerator DelayAction(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

   
}
