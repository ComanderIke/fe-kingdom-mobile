using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.GameStates;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Players;
using Assets.Scripts.Events;

public class MainScript : MonoBehaviour {
	
    #region EVENTS
    public delegate void WinLevelEvent();
    public static WinLevelEvent winLevelEvent;

	public delegate void MoveCharacterEvent(LivingObject character,int x, int z);
	public static MoveCharacterEvent moveCharacterEvent;
	public delegate void EndOfMoveCharacterEvent();
	public static EndOfMoveCharacterEvent endOfMoveCharacterEvent;

	public delegate void FightEvent(LivingObject attacker, LivingObject defender);
	public static FightEvent fightEvent;
	public delegate void EndOfFightEvent();
	public static EndOfFightEvent endOfFightEvent;

	public delegate void MoveCamEvent(Vector3 position);
	public static MoveCamEvent moveCamEvent;
	public delegate void EndOfMoveCam();
	public static EndOfMoveCam endOfMoveCam;

    #endregion

    private const float TIME_LIMIT = 60;
    private const double START_TURN_DELAY = 0.3f;

    #region fields

    [HideInInspector]
    public GameState gameState;
	[HideInInspector]
	public Vector2 oldPosition;

    bool init = false;
	[HideInInspector]
    public int AttackRangeFromPath;
    #endregion
    public List<EngineSystem> Systems { get; set; }

    private static MainScript instance;
    
    void Awake()
    {
        EventContainer.ResetEvents();
        instance = this;
        gameState = new GameplayState();
    }

    void Start () {
        Debug.Log("Initialize");
        
        Systems = new List<EngineSystem>();
        Systems.Add(FindObjectOfType<UISystem>());
        Systems.Add(FindObjectOfType<GridSystem>());
        Systems.Add(FindObjectOfType<CameraSystem>());
        Systems.Add(FindObjectOfType<AudioSystem>());
        Systems.Add(FindObjectOfType<SpeechBubbleSystem>());
        Systems.Add(FindObjectOfType<PopUpTextSystem>());
        Systems.Add(FindObjectOfType<UnitActionSystem>());
        Systems.Add(FindObjectOfType<InputSystem>());
        Systems.Add(FindObjectOfType<UnitsSystem>());
        Systems.Add(FindObjectOfType<TurnSystem>());
        Systems.Add(new UnitSelectionSystem());
    }
    
    public static MainScript GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("Instance null!");
            
        }
        else if (instance.gameObject == null)
        {
            Debug.Log("GameObject null!");
        }
            
        return instance;
    }

    private void Initialize()
    {
        
        InitPlayers();
        InitCharacters();
        
        gameState.Enter();
        Debug.Log("InitializeSystems");
        EventContainer.startTurn();
    }

    void Update () {
        if (!init)
        {
            Initialize();
            init = true;
        }
        gameState.Update();
    }

    public void SwitchState(GameState state)
    {
        gameState.Exit();
        gameState = state;
        state.Enter();
    }
    private void InitPlayers()
    {
        TurnSystem turnManager = GetSystem<TurnSystem>();
        foreach (Player p in turnManager.Players)
        {
            p.Init();
        }
    }
    private void InitCharacters()
    {
        TurnSystem turnManager = GetSystem<TurnSystem>();
        Player p = turnManager.Players[0];
        StartPosition[] startPositions = GameObject.FindObjectsOfType<StartPosition>();
        UnitInstantiater cc = GameObject.FindObjectOfType<UnitInstantiater>();
        RessourceScript ss = GameObject.FindObjectOfType<RessourceScript>();
        if (FindObjectOfType<GameData>() != null)
        {
            foreach (LivingObject u in FindObjectOfType<GameData>().player.Units)
            {
                p.AddUnit(u);
            }
           
            cc.PlaceCharacter(0, p.Units[0], startPositions[0].GetXOnGrid(), startPositions[0].GetYOnGrid());
        }
        else
        {
            Human filler = null;
            Human filler2 = null;
            Human filler3 = null;
            Human filler4 = null;
            
            filler = new Human("Leila", ss.sprites.GetCharacterOnMapSprites(0));
            filler2 = new Human("Flora", ss.sprites.GetCharacterOnMapSprites(2));
            filler3 = new Human("Eldric", ss.sprites.GetCharacterOnMapSprites(1));
            filler4 = new Human("Hector", ss.sprites.GetCharacterOnMapSprites(3));
            filler.Inventory.AddItem(GameObject.FindObjectOfType<DataScript>().weapons.woodenSword);
            filler.Inventory.UseItem(filler.Inventory.items[0]);
            filler2.Inventory.AddItem(GameObject.FindObjectOfType<DataScript>().weapons.woodenAxe);
            filler2.Inventory.UseItem(filler2.Inventory.items[0]);
            filler3.Inventory.AddItem(GameObject.FindObjectOfType<DataScript>().weapons.basicBow);
            filler3.Inventory.UseItem(filler3.Inventory.items[0]);
            filler4.Inventory.AddItem(GameObject.FindObjectOfType<DataScript>().weapons.woodenSpear);
            filler4.Inventory.UseItem(filler4.Inventory.items[0]);

            p.AddUnit(filler);
            cc.PlaceCharacter(0, filler, startPositions[0].GetXOnGrid(), startPositions[0].GetYOnGrid());
            p.AddUnit(filler2);
            p.AddUnit(filler3);
            p.AddUnit(filler4);
            cc.PlaceCharacter(0, filler2, startPositions[1].GetXOnGrid(), startPositions[1].GetYOnGrid());
            cc.PlaceCharacter(0, filler3, startPositions[2].GetXOnGrid(), startPositions[2].GetYOnGrid());
            cc.PlaceCharacter(0, filler4, startPositions[3].GetXOnGrid(), startPositions[3].GetYOnGrid());
        }

        //cc.PlaceCharacter(0, filler, startPositions[0].GetXOnGrid(), startPositions[0].GetYOnGrid());
        //cc.PlaceCharacter(0, filler2, startPositions[1].GetXOnGrid(), startPositions[1].GetYOnGrid());
        //cc.PlaceCharacter(0, filler3, startPositions[2].GetXOnGrid(), startPositions[2].GetYOnGrid());
        //cc.PlaceCharacter(0, filler4, startPositions[3].GetXOnGrid(), startPositions[3].GetYOnGrid());
        //Monster monster = new Monster("Mammoth", MonsterType.Mammoth, ss.sprites.GetMonsterOnMapSprites(0));
        Monster saber = new Monster("Sabertooth", MonsterType.Sabertooth, ss.sprites.GetMonsterOnMapSprites(1));
        //turnManager.Players[1].AddUnit(monster);
        turnManager.Players[1].AddUnit(saber);
        
        //cc.PlaceCharacter(1, monster, 4, 2);
        cc.PlaceCharacter(1, saber, 5, 2);
        saber.GridPosition.FacingLeft = true;
        //gridManager.ShowSightRange(saber);
    }

    public T GetSystem<T>()
    {
        foreach(EngineSystem s in Systems)
        {
            if (s is T)
            {
                return (T)Convert.ChangeType(s, typeof(T));
            }
        }
        return default(T);
    }





}
