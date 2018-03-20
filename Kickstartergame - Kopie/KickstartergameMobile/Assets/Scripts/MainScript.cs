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

    public GameObject pauseMenu;
    [HideInInspector]
    public GameState gameState;
    public GameObject gameOverScreen;

	public GridManager gridManager;
	public Camera myCamera;
    [HideInInspector]
	public bool isAnimation = false;
	[HideInInspector]
	public Vector3 ClickedPoint;
	[HideInInspector]
    public List<Vector2> Startpositions;
	[HideInInspector]
    public List<Vector2> Startpositions2;
    public UIController uiController;
	[HideInInspector]
	public Vector2 oldPosition;
    bool gameStarted = false;
    bool startingTurn = false;
    bool init = false;
	[HideInInspector]
    public int AttackRangeFromPath;
    #endregion
    public List<EngineSystem> Systems { get; set; }
    public List<Controller> Controllers { get; set; }

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
        Controllers = new List<Controller>();
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
        gridManager = FindObjectOfType<GridManager>();
        Systems.Add(new TurnManager());
        Systems.Add(new UnitSelectionManager());
        Systems.Add(FindObjectOfType<UnitActionManager>());
        Systems.Add(FindObjectOfType<MouseManager>());
        Controllers.Add(FindObjectOfType<UIController>());
        Controllers.Add(FindObjectOfType<UnitsController>());
        InitPlayers();
        InitCharacters();
        
        gameState.enter();
        Debug.Log("InitializeSystems");
        EventContainer.startTurn();
    }

    void Update () {
        if (!init)
        {
            Debug.Log("InitializeStart"+init);
            Initialize();
            init = true;
            Debug.Log("InitializeEnd" + init);
        }
        gameState.update();
    }

    public void SwitchState(GameState state)
    {
        gameState.exit();
        gameState = state;
        state.enter();
    }
    private void InitPlayers()
    {
        TurnManager turnManager = GetSystem<TurnManager>();
        foreach (Player p in turnManager.Players)
        {
            p.Init();
        }
    }
    private void InitCharacters()
    {
        TurnManager turnManager = GetSystem<TurnManager>();
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
            
            filler = new Human("Leila", ss.swordActiveSprite);
            filler2 = new Human("Flora", ss.axeActiveSprite);
            filler3 = new Human("Eldric", ss.archerActiveSprite);
            filler4 = new Human("Hector", ss.lancerActiveSprite);
            filler.Inventory.AddItem(GameObject.FindObjectOfType<WeaponScript>().woodenSword);
            filler.Inventory.UseItem(filler.Inventory.items[0]);
            filler2.Inventory.AddItem(GameObject.FindObjectOfType<WeaponScript>().woodenAxe);
            filler2.Inventory.UseItem(filler2.Inventory.items[0]);
            filler3.Inventory.AddItem(GameObject.FindObjectOfType<WeaponScript>().basicBow);
            filler3.Inventory.UseItem(filler3.Inventory.items[0]);
            filler4.Inventory.AddItem(GameObject.FindObjectOfType<WeaponScript>().woodenSpear);
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
        Monster monster = new Monster("Mammoth", MonsterType.Mammoth, ss.mammothSprite);
        Monster saber = new Monster("Sabertooth", MonsterType.Sabertooth, ss.sabertoothSprite);
        //turnManager.Players[1].AddUnit(monster);
        turnManager.Players[1].AddUnit(saber);

        //cc.PlaceCharacter(1, monster, 4, 2);
        cc.PlaceCharacter(1, saber, 5, 2);
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

    public T GetController<T>()
    {
        foreach (Controller c in Controllers)
        {
            if (c is T)
            {
                return (T)Convert.ChangeType(c, typeof(T));
            }
        }
        return default(T);
    }




}
