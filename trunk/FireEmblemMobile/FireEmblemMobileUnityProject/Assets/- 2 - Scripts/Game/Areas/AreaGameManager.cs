using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;

public class AreaGameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private Area_ActionSystem actionSystem;
    public GameObject moveOptionPrefab;
    public EncounterUIController uiCOntroller;
    public UIPartyCharacterCircleController uiPartyController;
    public Party playerStartParty;
    public ColumnManager ColumnManager;
    // Start is called before the first frame update
    public Transform spawnParent;
    private List<GameObject> moveOptions=new List<GameObject>();
    public float offsetBetweenCharacters = 0.25f;
    void Start()
    {
        //Debug.Log("WHY IS START NOT CALLED?");
        actionSystem = new Area_ActionSystem();
        if(PlayerPrefs.HasKey("CameraX")&&PlayerPrefs.HasKey("CameraY"))
        {
            WorldMapCameraController camera = FindObjectOfType<WorldMapCameraController>();
            camera.transform.position = new Vector3(PlayerPrefs.GetFloat("CameraX"), PlayerPrefs.GetFloat("CameraY"),
                camera.transform.position.z);
        }


        if (Player.Instance.Party == null)
        {
            //Debug.Log("Player Null");
            Player.Instance.Party = Instantiate(playerStartParty);
            Player.Instance.Party.Initialize();
            

        }
        else
        {
            
        }
    
        
       
        if (Player.Instance.Party.EncounterNode == null)
        {
            //Debug.Log("Player Node Null");
           
            Player.Instance.Party.EncounterNode = EncounterTree.Instance.startNode;
        }
        SpawnPartyMembers();
        uiCOntroller.Init(Player.Instance.Party);
       

       Debug.Log(Player.Instance.Party.EncounterNode);
        ResetMoveOptions();
  
        ShowMoveOptions();
        uiPartyController.Show(Player.Instance.Party);
        
    }

  
    private List<EncounterPlayerUnitController> partyGameObjects;
    void SpawnPartyMembers()
    {
        int cnt = 1;
        var partyGo = new GameObject("Party");
        partyGo=Instantiate(partyGo, spawnParent);
        partyGo.transform.position = Player.Instance.Party.EncounterNode.gameObject.transform.position;
        partyGameObjects = new List<EncounterPlayerUnitController>();
        //Spawn ActiveUnit first
        var activeUnit = Player.Instance.Party.members[Player.Instance.Party.ActiveUnitIndex];
        var go = Instantiate(activeUnit.visuals.Prefabs.EncounterAnimatedSprite, partyGo.transform, false);
        go.transform.localPosition = new Vector3(0,0,0);
        go.GetComponent<EncounterPlayerUnitController>().SetUnit(activeUnit);
        go.GetComponent<EncounterPlayerUnitController>().SetActiveUnit(true);

        go.GetComponent<EncounterPlayerUnitController>().SetSortOrder(Player.Instance.Party.members.Count);
        partyGameObjects.Add( go.GetComponent<EncounterPlayerUnitController>());
        foreach (var member in Player.Instance.Party.members)
        {
            if (member == activeUnit)
                continue;
            
            go = Instantiate(member.visuals.Prefabs.EncounterAnimatedSprite, partyGo.transform, false);
            go.transform.localPosition = new Vector3(-offsetBetweenCharacters*cnt,0,0);
            go.GetComponent<EncounterPlayerUnitController>().SetUnit(member);
            go.GetComponent<EncounterPlayerUnitController>().SetActiveUnit(false);

            go.GetComponent<EncounterPlayerUnitController>().SetSortOrder(Player.Instance.Party.members.Count-cnt);
            cnt++;
            partyGameObjects.Add(go.GetComponent<EncounterPlayerUnitController>());
        }
        Player.Instance.Party.GameObject = partyGo;
    }
    public void UpdatePartyGameObjects()
    {
        Debug.Log("UpdatePartyMembes!");
        var activeUnit = Player.Instance.Party.members[Player.Instance.Party.ActiveUnitIndex];
        int cnt = 1;
        foreach (var member in Player.Instance.Party.members)
        {
            if (member == activeUnit)
            {
                foreach (var unitController in partyGameObjects)
                {
                    if (unitController.Unit == member)
                    {
                        unitController.transform.localPosition = new Vector3(0,0,0);
                        unitController.SetActiveUnit(true);
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
                        unitController.SetActiveUnit(false);
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
    public void ShowMoveOptions()
    {
        moveOptions = new List<GameObject>();
        foreach (var child in Player.Instance.Party.EncounterNode.children)
        {
            child.moveable = true;
            var go = Instantiate(moveOptionPrefab, spawnParent, false);
            go.transform.position = child.gameObject.transform.position;
            moveOptions.Add(go);
        }
    }

  
    // Update is called once per frame
    void Update()
    {
        
    }

    public void NodeClicked(EncounterNode encounterNode)
    {
        FindObjectOfType<EncounterCursorController>().SetPosition(encounterNode.gameObject.transform.position);
        if (encounterNode.moveable)
        {
            
           
            actionSystem.Move(encounterNode);
            StartCoroutine( DelayAction(()=>encounterNode.Activate(Player.Instance.Party), 1.0f));
            // ResetMoveOptions();
            // ShowMoveOptions();
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
