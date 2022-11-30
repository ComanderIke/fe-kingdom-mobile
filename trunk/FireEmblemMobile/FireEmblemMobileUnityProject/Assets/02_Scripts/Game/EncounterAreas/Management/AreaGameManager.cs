using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using __2___Scripts.Game.Areas;
using Audio;
using Effects;
using Game.GameActors.Players;
using Game.GUI;
using Game.Manager;
using Game.Mechanics;
using Game.States;
using Game.Systems;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using GameEngine;
using LostGrace;
using Menu;
using UnityEngine;
using IServiceProvider = Game.Manager.IServiceProvider;

public class AreaGameManager : MonoBehaviour, IServiceProvider
{
    public static AreaGameManager Instance;
    private List<IEngineSystem> Systems { get; set; }
    public GameObject playerPrefab;
    private Area_ActionSystem actionSystem;
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
        Instance = this;
       
        cursor = FindObjectOfType<EncounterCursorController>();
        //Debug.Log("WHY IS START NOT CALLED?");
        actionSystem = new Area_ActionSystem();
        if(PlayerPrefs.HasKey("CameraX")&&PlayerPrefs.HasKey("CameraY"))
        {
            EncounterAreaCameraController camera = FindObjectOfType<EncounterAreaCameraController>();
            camera.transform.position = new Vector3(PlayerPrefs.GetFloat("CameraX"), PlayerPrefs.GetFloat("CameraY"),
                camera.transform.position.z);
        }

        if (Player.Instance.Party!=null)
        {
            Debug.Log("Use party saveData");
            Player.Instance.Party.Initialize();
            LoadEncounterAreaData(Player.Instance.Party, EncounterTree.Instance);
            
            
        }

        if (Player.Instance.Party != null)
        {
            Debug.Log("Player GO Name: "+Player.Instance.gameObject.name);
            Debug.Log("party Count:" + Player.Instance.Party.members.Count);
        }

        if (Player.Instance.Party == null || Player.Instance.Party.members.Count == 0){
            Debug.LogWarning("No party Members! Creating DemoUnits");
           
            Player.Instance.Party = new Party();
            var demoUnits = GameObject.FindObjectOfType<DemoUnits>().GetUnits();
            Player.Instance.Party.members = demoUnits;
            Player.Instance.Party.Initialize();
            Player.Instance.Party.EncounterComponent.EncounterNode = EncounterTree.Instance.startNode;
            Player.Instance.Party.EncounterComponent.AddMovedEncounter(EncounterTree.Instance.startNode);
        }

       
        
            //Debug.Log("Player Node Null");
           
            AddSystems();
      
        SpawnPartyMembers();
        uiCOntroller.Init(Player.Instance.Party);
       
      
        ResetMoveOptions();

        uiPartyController.Show(Player.Instance.Party);
        lightController.UpdateHour(hour);
        this.CallWithDelay(ShowMovedRoads,0.1f);//Some other scripts not started yet thtas why
       
        ShowMoveOptions();
    }
    public void LoadEncounterAreaData(Party party, EncounterTree tree){
      
        party.EncounterComponent.EncounterNode = tree.GetEncounterNodeById(party.EncounterComponent.EncounterNodeId);
        for (int i = 0; i < party.EncounterComponent.MovedEncounterIds.Count; i++)
        {
            party.EncounterComponent.AddMovedEncounter(tree.GetEncounterNodeById(party.EncounterComponent.MovedEncounterIds[i]));
        }
    }
    private void AddSystems()
    {
        Systems = new List<IEngineSystem>
        {
            FindObjectOfType<AudioSystem>(),
            new BattleSystem(),
            new UnitProgressSystem(Player.Instance.Party),
        };
        InjectDependencies();
    }

    private void InjectDependencies()
    {
        GetSystem<BattleSystem>().BattleAnimation = FindObjectsOfType<MonoBehaviour>().OfType<IBattleAnimation>().First();
        GetSystem<BattleSystem>().BattleAnimation.Hide();
        GetSystem<UnitProgressSystem>().levelUpRenderer = FindObjectsOfType<MonoBehaviour>().OfType<ILevelUpRenderer>().First();
        GetSystem<UnitProgressSystem>().expRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IExpRenderer>().First();
    }
    private bool LoadedSaveData()
    {
        return SaveData.currentSaveData != null && SaveData.currentSaveData.playerData != null;
    }

    private GameObject partyGo;
    private List<EncounterPlayerUnitController> partyGameObjects;
    void SpawnPartyMembers()
    {
        Debug.Log("Party Count: "+Player.Instance.Party.members.Count);
        int cnt = 1;
        if (Player.Instance.Party.EncounterComponent.EncounterNode == null)
        {
            Player.Instance.Party.EncounterComponent.EncounterNode = EncounterTree.Instance.startNode;
            Player.Instance.Party.EncounterComponent.AddMovedEncounter(EncounterTree.Instance.startNode);
        }

        partyGo = new GameObject("Partytest");
        partyGo.transform.SetParent(spawnParent);
        partyGo.transform.position = Player.Instance.Party.EncounterComponent.EncounterNode.gameObject.transform.position;
        partyGameObjects = new List<EncounterPlayerUnitController>();
        //Spawn ActiveUnit first
        var activeUnit = Player.Instance.Party.members[Player.Instance.Party.ActiveUnitIndex];
        var go = Instantiate(activeUnit.visuals.Prefabs.EncounterAnimatedSprite, partyGo.transform, false);
        go.transform.localPosition = new Vector3(0,0,0);
        var uc =  go.GetComponent<EncounterPlayerUnitController>();
        uc.SetUnit(activeUnit);
        ShowActiveUnit(go.transform.position);
        uc.Show();
        uc.onClicked -= UnitClicked;
        uc.onClicked += UnitClicked;
       // go.GetComponent<EncounterPlayerUnitController>().SetTarget(null);
       // go.GetComponent<EncounterPlayerUnitController>().SetSortOrder(Player.Instance.Party.members.Count);
        activeMemberGo = go;

        partyGameObjects.Add( go.GetComponent<EncounterPlayerUnitController>());
        foreach (var member in Player.Instance.Party.members)
        {
            Debug.Log("Party Member: "+member);
            if (member == activeUnit)
                continue;
            
            go = Instantiate(member.visuals.Prefabs.EncounterAnimatedSprite, partyGo.transform, false);
            go.transform.localPosition = new Vector3(0,0,0);
            uc =  go.GetComponent<EncounterPlayerUnitController>();
            uc.Hide();
            uc.SetUnit(member);
            uc.onClicked -= UnitClicked;
            uc.onClicked += UnitClicked;
          //  go.GetComponent<EncounterPlayerUnitController>().SetTarget(activeMemberGo.transform);
           // go.GetComponent<EncounterPlayerUnitController>().SetOffsetCount(cnt);
            //go.GetComponent<EncounterPlayerUnitController>().SetSortOrder(Player.Instance.Party.members.Count-cnt);
            cnt++;
            partyGameObjects.Add(go.GetComponent<EncounterPlayerUnitController>());
        }
        Player.Instance.Party.GameObject = partyGo;
    }

    private void UnitClicked(EncounterPlayerUnitController clickedUnitController)
    {
       
        MovementHint();
    }
    private GameObject activeMemberGo;
    public void UpdatePartyGameObjects()
    {

        var activeUnit = Player.Instance.Party.members[Player.Instance.Party.ActiveUnitIndex];
        int cnt = 1;

        foreach (var unitController in partyGameObjects)
        {
            if (unitController.unit == activeUnit)
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
                    if (unitController.unit == member)
                    {
                        //unitController.transform.localPosition = new Vector3(0,0,0);
                        ShowActiveUnit(unitController.transform.position);
                        unitController.Show();
                        //unitController.SetTarget(null);
                       // unitController.SetSortOrder(Player.Instance.Party.members.Count);
                    }
                }
               
            }
            else
            {
                foreach (var unitController in partyGameObjects)
                {
                    if (unitController.unit == member)
                    {
                        //unitController.transform.localPosition = new Vector3(-offsetBetweenCharacters*cnt,0,0);
                        unitController.Hide();
                       // unitController.SetTarget(activeMemberGo.transform);
                        //unitController.SetOffsetCount(cnt);
                        //Debug.Log("SetOffset!");
                       // unitController.SetSortOrder(Player.Instance.Party.members.Count-cnt);
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
        foreach (var child in Player.Instance.Party.EncounterComponent.EncounterNode.children)
        {
            child.SetMoveable(false);

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
        
        foreach (var child in Player.Instance.Party.EncounterComponent.EncounterNode.children)
        {
            child.SetMoveable(true);

        }
        foreach (var road in Player.Instance.Party.EncounterComponent.EncounterNode.roads)
        {
            road.SetMoveable(true);
           
        }
      
    }

  
    // Update is called once per frame
    void Update()
    {
        
    }

    private EncounterCursorController cursor;

    private void SetAllEncountersNotMovable()
    {
        foreach (var column in EncounterTree.Instance.columns)
        {
            foreach (var node in column.children)
            {
                node.SetMoveable(false);
            }
        }
    }

    void MovementHint()
    {
        Debug.Log("Show Movement Hint");
        foreach (var road in Player.Instance.Party.EncounterComponent.EncounterNode.roads)
        {
            road.end.Grow();
        }
    }
    public void NodeClicked(EncounterNode encounterNode)
    {
        Debug.Log("Node Clicked: "+encounterNode);
      
        cursor.SetPosition(encounterNode.gameObject.transform.position);
        if (encounterNode== Player.Instance.Party.EncounterComponent.EncounterNode)
        {
            MovementHint();
            //FindObjectOfType<UICharacterViewController>().Show(Player.Instance.Party.ActiveUnit);
            
            return;
        }
        if (encounterNode.moveable)
        {

            ToolTipSystem.ShowEncounter(encounterNode, encounterNode.gameObject.transform.position+new Vector3(2,0,0), true, MoveClicked);
            foreach (var road in Player.Instance.Party.EncounterComponent.EncounterNode.roads)
            {
                if(road.end==encounterNode)
                    road.NodeSelected();
                else
                    road.NodeDeselected();
            }
            // ResetMoveOptions();
            // ShowMoveOptions();
        }
        else
        {
            ToolTipSystem.ShowEncounter(encounterNode, encounterNode.gameObject.transform.position+new Vector3(2,0,0), false, null);
        }
    }

    void HideMoveOptions()
    {
        foreach (var road in Player.Instance.Party.EncounterComponent.EncounterNode.roads)
        {
            road.SetMoveable(false);
        }
    }
    void MoveClicked(EncounterNode node)
    {
        HideMoveOptions();
        SetAllEncountersNotMovable();
        StartCoroutine(MovementAnimation(node));
        circleUI.Rotate();
        hour += 6;
        if (hour >= 24)
            hour = 0;
        lightController.UpdateHour(hour);
    }
    IEnumerator MovementAnimation(EncounterNode target)
    {
        Vector3 targetPos = target.gameObject.transform.position;
        Vector3 startPos = partyGo.transform.position;
        float distance = Vector3.Distance(targetPos, startPos);
        float time = 0;
        float speed = 3;
        while ( partyGo.transform.position!=targetPos)
        {
            time += Time.deltaTime *speed/distance;
            partyGo.transform.position = Vector3.Lerp(startPos, targetPos, time);
            yield return null;

        }
        actionSystem.Move(target);
        ShowMovedRoads();
        Debug.Log("Before DelayAction!");
        this.CallWithDelay(()=>target.Activate(Player.Instance.Party), 1.0f);
       
        
    }

    EncounterNode GetEncounterNodeById(string id)
    {
        return EncounterTree.Instance.GetEncounterNodeById(id);
    }
    private void ShowMovedRoads()
    {
        Debug.Log("============================ShowMovedRoads!");
        for( int i= Player.Instance.Party.EncounterComponent.MovedEncounters.Count-2; i >=0; i--)
        {
            Road road = Player.Instance.Party.EncounterComponent.MovedEncounters[i].GetRoad(Player.Instance.Party.EncounterComponent.MovedEncounters[i + 1]);
            if (road==null||road.gameObject == null)
            {
                Player.Instance.Party.EncounterComponent.RemoveMovedEncounterAt(i);
            }
            else
            {
                road.SetMovedVisual();
            }

        }
    }

    public void Continue()
    {
        // Debug.Log("AutoSaving!");
        // SaveGameManager.Save();// new SaveData(Player.Instance, Campaign.Instance, EncounterTree.Instance));
        SetAllEncountersNotMovable();
        ResetMoveOptions();
        ShowMoveOptions();
        
    }


    public T GetSystem<T>()
    {
        foreach (var s in Systems.OfType<T>())
            return (T) Convert.ChangeType(s, typeof(T));
        return default;
    }

    public void StartChildCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
