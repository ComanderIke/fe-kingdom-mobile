using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using __2___Scripts.Game.Areas;
using Audio;
using Effects;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameResources;
using Game.GUI;
using Game.Manager;
using Game.Mechanics;
using Game.States;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using GameCamera;
using GameEngine;
using LostGrace;
using Menu;
using TMPro;
using UnityEngine;
using IServiceProvider = Game.Manager.IServiceProvider;

public class AreaGameManager : MonoBehaviour, IServiceProvider
{
    public static AreaGameManager Instance;
    private List<IEngineSystem> Systems { get; set; }
    public static event Action<int> OnAreaCompleted;
    public static event Action<AreaData> OnAreaStarted;


    private Area_ActionSystem actionSystem;
    public EncounterUIController uiCOntroller;
    public UIPartyCharacterCircleController uiPartyController;
    private ActiveUnitGroundFX activeUnitGroundGO;
    [SerializeField] private GameObject activeUnitGroundPrefab;
    public UICharacterViewController uiCharacterView;
    // Start is called before the first frame update
    public Transform spawnParent;


    [SerializeField]private float moveToNodeHoldTime = 1.5f;
    [SerializeField] private TextMeshProUGUI areaText;
    
    public TimeOfDayManager timeOfDayManager;
    [SerializeField] private bool startFreshSave = false;
    [SerializeField] private float delayBeforeLoadingNewScene=2f;
    private static bool startFreshSaveFirstTime;
    private List<EncounterNodeClickController> nodeClickControllers;
    void Awake()
    {
        startReady = false;
        MyDebug.LogTest("Subscribe SceneReady");
        SceneController.OnSceneReady -= SceneReady;
        SceneController.OnSceneReady += SceneReady;

        Instance = this;
       
        Debug.Log(Player.Instance.Party);
        if (startFreshSave&&startFreshSaveFirstTime==false)//Just for Testing when starting from encounterArea
        {
            startFreshSaveFirstTime = true;
            startFreshSave = false;
            MyDebug.LogEngine("Started game from EncounterArea => creating new game savedata");
            SaveGameManager.NewGame(0, "DebugEditorSave", GameConfig.Instance.ConfigProfile.chosenDifficulty.name);
        }
        else
        {
            MyDebug.LogPersistance("Load game Slot 0:");
            SaveGameManager.Load(0); //Trigger load at start of scenes to trigger all persistance objects observers
        }
        Debug.Log(Player.Instance.Party);
        
       
    }

    private bool areaStart = false;
    private bool startReady = false;
    private void Start()
    {
       
        cursor = FindObjectOfType<EncounterCursorController>();
        //Debug.Log("WHY IS START NOT CALLED?");
        actionSystem = new Area_ActionSystem();
        nodeClickControllers = new List<EncounterNodeClickController>();
       
        if (Player.Instance.Party!=null)
        {
            MyDebug.LogLogic("Using existing party data");
            Player.Instance.Party.Initialize();
            LoadEncounterAreaData(Player.Instance.Party, EncounterTree.Instance);
            
            
        }


        
        if (Player.Instance.Party == null || Player.Instance.Party.members.Count == 0){
            MyDebug.LogLogic("No existing party! Creating new party with default units");
           
            if(Player.Instance.Party==null)
                Player.Instance.Party = new Party();
            var demoUnits = GameObject.FindObjectOfType<DemoUnits>().GetUnits();
            Player.Instance.Party.members = demoUnits;
            Player.Instance.Party.Initialize();
            Player.Instance.Party.EncounterComponent.EncounterNode = EncounterTree.Instance.startNode;
            Player.Instance.Party.EncounterComponent.AddMovedEncounter(EncounterTree.Instance.startNode);
        }
           
        AddSystems();
      
        SpawnPartyMembers();
        uiCOntroller.Init(Player.Instance.Party);
       
      
        ResetMoveOptions();

        uiPartyController.Show(Player.Instance.Party);
        timeOfDayManager.InitHour(6);
        this.CallWithDelay(ShowMovedRoads,0.1f);//Some other scripts not started yet thtas why

 
        ShowAllInactiveNodes();
        if (!Player.Instance.Party.EncounterComponent.activatedEncounter)
        {
            Player.Instance.Party.EncounterComponent.EncounterNode.Activate(Player.Instance.Party);
        }
        else
        {       
            ShowMoveOptions();
        }

        FindObjectOfType<CameraSystem>().GetMixin<FocusCameraMixin>().SetTargets(Player.Instance.Party.EncounterComponent.EncounterNode.gameObject);
        ShowActiveUnitGroundFX();
        MyDebug.LogLogic("Enter Area: "+ (Player.Instance.Party.AreaIndex+1));
        areaText.SetText("Area <size=120%>"+(Player.Instance.Party.AreaIndex+1));
        startReady = true;
    }

    void SceneReady()
    {
        MyDebug.LogTest("SCENE IS READY");
        if (startReady)
        {
            if(areaStart)
                OnAreaStarted?.Invoke(GameBPData.Instance.AreaDataList[Player.Instance.Party.AreaIndex]);
            if (SceneTransferData.Instance.IsBoss)
            {
                AreaCompleted();
           
            }
        }
        else
        {
            Debug.LogError("START WAS NOT READY WHEN SCENE WAS READY!!!!!");
            MyDebug.LogTODO("START WAS NOT READY WHEN SCENE WAS READY!!!!!");
                MonoUtility.DelayFunction(() =>
                {
                    if(areaStart)
                        OnAreaStarted?.Invoke( GameBPData.Instance.AreaDataList[Player.Instance.Party.AreaIndex]);
                    if (SceneTransferData.Instance.IsBoss)
                    {
                        AreaCompleted();
           
                    }
                },.05f);
        }
        
    }
    private void AreaCompleted()
    {
        OnAreaCompleted?.Invoke(Player.Instance.Party.AreaIndex);
        MyDebug.LogTODO("Wait for ContinueClickedOnLoadingScreen");
        SceneTransferData.Instance.Reset();
        Player.Instance.Party.AreaIndex++;
        MyDebug.LogTest("AREAINDEX: "+ Player.Instance.Party.AreaIndex);
        Player.Instance.Party.EncounterComponent.Reset();
        MonoUtility.DelayFunction(() =>
        {
            ResetLoadArea();
        },delayBeforeLoadingNewScene);
    }
    private void ResetLoadArea()
    {
        Player.Instance.Party.ResetFoodBuffs();
        SaveGameManager.Save();
        GridGameManager.Instance.CleanUp();
        SceneController.LoadSceneAsync(Scenes.EncounterArea, false);
    }

    
    public void LoadEncounterAreaData(Party party, EncounterTree tree){
        MyDebug.LogTest("EncounterNodeId: "+party.EncounterComponent.EncounterNodeId);
        party.EncounterComponent.EncounterNode = tree.GetEncounterNodeById(party.EncounterComponent.EncounterNodeId);
        if(party.EncounterComponent.EncounterNode!=null)
            MyDebug.LogTest("EncounterNode: " +party.EncounterComponent.EncounterNode );
        // for (int i = 0; i < party.EncounterComponent.MovedEncounterIds.Count; i++)
        // {
        //     party.EncounterComponent.AddMovedEncounter(tree.GetEncounterNodeById(party.EncounterComponent.MovedEncounterIds[i]));
        // }
    }
    private void AddSystems()
    {
        Systems = new List<IEngineSystem>
        {
            new BattleSystem(),
            new UnitProgressSystem(Player.Instance.Party),
            new SkillSystem(GameBPData.Instance.SkillGenerationConfig,FindObjectsOfType<MonoBehaviour>().OfType<ISkillUIRenderer>().First()),
        };
        InjectDependencies();
        foreach (var system in Systems)
        {
            system.Init();
            system.Activate();
        }
    }

    private void InjectDependencies()
    {
        GetSystem<BattleSystem>().BattleAnimation = FindObjectsOfType<MonoBehaviour>().OfType<IBattleAnimation>().First();
        GetSystem<BattleSystem>().BattleAnimation.Hide();
        GetSystem<UnitProgressSystem>().levelUpRenderer = FindObjectsOfType<MonoBehaviour>().OfType<ILevelUpRenderer>().First();
        GetSystem<UnitProgressSystem>().expRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IExpRenderer>().First();
        var expBars = FindObjectsOfType<MonoBehaviour>().OfType<ExpBarController>();
        foreach (var expBar in expBars)
        {
            if (expBar.CompareTag("MainExpBar"))
                GetSystem<UnitProgressSystem>().ExpBarController = expBar;
        }
       
    }
    private bool LoadedSaveData()
    {
        return SaveGameManager.currentSaveData != null && SaveGameManager.currentSaveData.playerData != null;
    }

    private GameObject partyGo;
    private List<EncounterPlayerUnitController> partyGameObjects;
    void SpawnPartyMembers()
    {
       //Debug.Log("Party Count: "+Player.Instance.Party.members.Count);
        int cnt = 1;
        if (Player.Instance.Party.EncounterComponent.EncounterNode == null)
        {
            Player.Instance.Party.EncounterComponent.EncounterNode = EncounterTree.Instance.startNode;
            Player.Instance.Party.EncounterComponent.AddMovedEncounter(EncounterTree.Instance.startNode);
            areaStart = true;
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
        Player.Instance.Party.onActiveUnitChanged-=UpdatePartyGameObjects;
        Player.Instance.Party.onActiveUnitChanged+=UpdatePartyGameObjects;
        
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
            if (unitController.unit.Equals(activeUnit))
            {
                activeMemberGo = unitController.gameObject;
            }
        }

        foreach (var member in Player.Instance.Party.members)
        {
            if (member.Equals(activeUnit))
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
                    if (unitController.unit.Equals(member))
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
        foreach (var member in Player.Instance.Party.deadMembers)
        {
            foreach (var unitController in partyGameObjects)
            {
                if (unitController.unit.Equals(member))
                {
                    unitController.Hide();
                }
            }
        }
        MonoUtility.InvokeNextFrame(()=>uiPartyController.Show(Player.Instance.Party));//Otherwise mouse click will go through UI
        uiCharacterView.UpdateUnit(Player.Instance.Party.ActiveUnit);
    }
   
    private void ResetMoveOptions()
    {
       

        EncounterTree.Instance.SetAllNodesMoveable(false);
        // foreach (var child in Player.Instance.Party.EncounterComponent.EncounterNode.children)
        // {
        //     child.SetMoveable(false);
        //
        // }
    }

   // public GameObject activeUnitEffectPrefab;
    private GameObject activeUnitEffect;
    public void ShowActiveUnit(Vector3 position)
    {
        // if (activeUnitEffect == null)
        // {
        //     activeUnitEffect = Instantiate(activeUnitEffectPrefab, spawnParent, false);
        //     
        // }
        // activeUnitEffect.transform.position = position;
    }
    public void ShowMoveOptions()
    {
        
        foreach (var child in Player.Instance.Party.EncounterComponent.EncounterNode.children)
        {
            if (child != Player.Instance.Party.EncounterComponent.EncounterNode)
            {
                child.SetMoveable(true);
                child.SetActive(true);
                foreach (var nestedChild in child.children)
                {
                    nestedChild.SetActive(true);
                }
                foreach (var nestedRoad in child.roads)
                {
                    nestedRoad.SetMovedVisual();
                }
            }

//            Debug.Log("Move Option: "+child+" "+child.gameObject.transform.position);
        
        }
        foreach (var road in Player.Instance.Party.EncounterComponent.EncounterNode.roads)
        {
            road.SetMoveable(true);
           
        }
      
    }

  
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<CameraSystem>().GetMixin<FocusCameraMixin>().SetTargets(Player.Instance.Party.EncounterComponent.EncounterNode.gameObject);
        }
        if (Input.GetMouseButtonUp(0))
        {
           
           
            foreach (var clickController in nodeClickControllers)
            {
          
                clickController.encounterNode.renderer.IncreaseScale(0);
                clickController.encounterNode.renderer.AmplifyRotationSpeedMultiplier(0);
            }
        }

        if (!Input.GetMouseButton(0)&&!Input.GetMouseButtonDown(0))
        {
            cursor.DecreaseFill();
        }
    }

    private EncounterCursorController cursor;
    public void Deactivate()
    {
        SceneController.OnSceneReady -= SceneReady;
        Player.Instance.Party.onActiveUnitChanged-=UpdatePartyGameObjects;
        foreach (var system in Systems)
        {
            system.Deactivate();
        }
        
    }
    private void OnDestroy()
    {
        Deactivate();
    }

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
        foreach (var road in Player.Instance.Party.EncounterComponent.EncounterNode.roads)
        {
            road.end.Grow();
        }
    }

    public enum NodeType
    {
        Battle,
        Heal,
        Random,
        Standard,
        Boss,
        Elite
    }
    public void NodeClicked(EncounterNode encounterNode)
    {
        MyDebug.LogInput("Node Clicked: "+encounterNode);
      
        cursor.Show();
        cursor.SetPosition(encounterNode.gameObject.transform.position);
        cursor.SetSprite(encounterNode.renderer.moveOptionSprite);
       // cursor.SetColor(encounterNode.renderer.typeColor);
        cursor.SetScale(1.0f);
        if (encounterNode is BattleEncounterNode battleEncounterNode)
        {
            if(battleEncounterNode.BattleType==BattleType.Boss)
                cursor.SetScale(1.8f);
        }
       
        if (encounterNode== Player.Instance.Party.EncounterComponent.EncounterNode)
        {
            MovementHint();
            //FindObjectOfType<UICharacterViewController>().Show(Player.Instance.Party.ActiveUnit);
            
            return;
        }
        if (encounterNode.moveable)
        {
 
            //ToolTipSystem.ShowEncounter(encounterNode, encounterNode.gameObject.transform.position+new Vector3(2,0,0), true, MoveClicked);
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
         
            //ToolTipSystem.ShowEncounter(encounterNode, encounterNode.gameObject.transform.position+new Vector3(2,0,0), false, null);
        }
    }

    void HideMoveOptions()
    {
        foreach (var road in Player.Instance.Party.EncounterComponent.EncounterNode.roads)
        {
            road.SetMoveable(false);
        }
    }

    public void MoveClicked(int nodeIndex)
    {
        MoveClicked(nodeClickControllers[nodeIndex].encounterNode);
    }
    public void MoveClicked(EncounterNode node)
    {
        HideMoveOptions();
        activeUnitGroundGO.FadeOut();
        SetAllEncountersNotMovable();
        // foreach (var road in Player.Instance.Party.EncounterComponent.EncounterNode.roads)
        // {
        //         road.NodeDeselected();
        // }
        // SetAllEncountersNotMovable();
        StartCoroutine(MovementAnimation(node));
       
        cursor.Hide();
        
        timeOfDayManager.ElapseTimeStep();
     
    }

   
    IEnumerator MovementAnimation(EncounterNode target)
    {
        Vector3 targetPos = target.gameObject.transform.position;
        Vector3 startPos = partyGo.transform.position;
        float distance = Vector3.Distance(targetPos, startPos);
        float time = 0;
        float speed = 3;
        actionSystem.Move(target);
        // Debug.Log("Reset Moveoptions!");
        ResetMoveOptions();
        ShowMovedRoads();
       
        while ( partyGo.transform.position!=targetPos)
        {
            time += Time.deltaTime *speed/distance;
            partyGo.transform.position = Vector3.Lerp(startPos, targetPos, time);
            yield return null;

        }

        ShowActiveUnitGroundFX();
        ShowInactiveNodes();
 
        this.CallWithDelay(()=>target.Activate(Player.Instance.Party), 1.0f);
       
        
    }

 
    private void ShowActiveUnitGroundFX()
    {
        
        if (activeUnitGroundGO == null)
        {
            activeUnitGroundGO = Instantiate(activeUnitGroundPrefab, partyGo.transform, false)
                .GetComponent<ActiveUnitGroundFX>();
        }
        else
        {
           
            activeUnitGroundGO.FadeIn();
        }
        
    }

    private void ShowAllInactiveNodes()
    {
        for (int i = 0; i <= Player.Instance.Party.EncounterComponent.EncounterNode.depth; i++)
        {


            foreach (var child in EncounterTree.Instance.columns[i].children)
            {
                if (!Player.Instance.Party.EncounterComponent.MovedEncounterIds.Contains(child.GetId()))
                    child.renderer.SetInactive();
            }
        }
    }

    void ShowInactiveNodes()
    {

         var currentNode = Player.Instance.Party.EncounterComponent.EncounterNode;
        
        // var lastNode =
        //     Player.Instance.Party.EncounterComponent.MovedEncounters[
        //         Player.Instance.Party.EncounterComponent.MovedEncounters.Count - 2];
        // foreach (var child in lastNode.children)
        // {
        //     if(child!=currentNode&& !(child is StartEncounterNode))
        //         child.renderer.SetInactive();
        // }
        var parentId = Player.Instance.Party.EncounterComponent.MovedEncounterIds[Player.Instance.Party.EncounterComponent.MovedEncounterIds.Count-2];
        // Debug.Log("Parent ID Node: "+parentId);
        // Debug.Log("CurrentNode: "+currentNode.gameObject.name);
        foreach (var child in EncounterTree.Instance.columns[currentNode.depth-1].children)
        {
            if (child.GetId() == parentId)
            {
                foreach (var road in child.roads)
                {
                    if (road.end != currentNode)
                    {
                        // Debug.Log("Set Missed: "+road.end.gameObject.name);
                        road.SetMissedVisual();
                    }
                }
            }
        }
        // Debug.Log("Show Inactive Nodes of: "+currentNode);
        foreach (var child in EncounterTree.Instance.columns[currentNode.depth].children)
        {
            if (!Player.Instance.Party.EncounterComponent.MovedEncounterIds.Contains(child.GetId()))
            {
                child.renderer.SetInactive();
                foreach (var nestedchild in child.children)
                {
                    nestedchild.renderer.SetInactive();
                }
                foreach (var road in child.roads)
                {
                    road.SetMissedVisual();
                }
            }
        }
    }
    EncounterNode GetEncounterNodeById(string id)
    {
        return EncounterTree.Instance.GetEncounterNodeById(id);
    }
    private void ShowMovedRoads()
    {
        
        for( int i= Player.Instance.Party.EncounterComponent.MovedEncounterIds.Count-2; i >=0; i--)
        {
            var node = EncounterTree.Instance.GetEncounterNodeById(Player.Instance.Party.EncounterComponent
                .MovedEncounterIds[i]);
            var nextNode=EncounterTree.Instance.GetEncounterNodeById(Player.Instance.Party.EncounterComponent
                .MovedEncounterIds[i+1]);
            Road road = node.GetRoad(nextNode);
            if (road==null||road.gameObject == null)
            {
              //  Player.Instance.Party.EncounterComponent.RemoveMovedEncounterAt(i);
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
        Player.Instance.Party.EncounterTick();
         SetAllEncountersNotMovable();
        
         ShowMoveOptions();
        
    }


    public T GetSystem<T>()
    {
        foreach (var s in Systems.OfType<T>())
            return (T) Convert.ChangeType(s, typeof(T));
        return default;
    }

    public Coroutine StartChildCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void StopChildCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }


    public void NodeHolding(EncounterNode encounterNode, float holdTime)
    {
        if (!encounterNode.moveable)
            return;
        
        //cursor.SetScale(1.0f+holdTime);
        cursor.SetFill(1-holdTime/moveToNodeHoldTime);
    
        encounterNode.renderer.IncreaseScale(holdTime/2.5f);
        encounterNode.renderer.AmplifyRotationSpeedMultiplier(holdTime*200);
        if (holdTime >= moveToNodeHoldTime)
        {
            MoveClicked(encounterNode);
        }
    }

    public void SubscribeNodeClickController(EncounterNodeClickController encounterNodeClickController)
    {
        nodeClickControllers.Add(encounterNodeClickController);
    }

    public void CleanUp()
    {
        MyDebug.LogTODO("TODO here and in gridgamemanager cleanup happens multiple times from ondestroy/ondisable and cleanup");
        Deactivate();
    }
}