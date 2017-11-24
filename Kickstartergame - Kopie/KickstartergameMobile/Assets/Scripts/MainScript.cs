using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.GameStates;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Players;

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
    public GameObject PlayerTurnAnimation;
    public GameObject AITurnAnimation;
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
        gameState = new GameplayState();
    }

    void Start () {     
        gridManager = FindObjectOfType<GridManager>();
        Systems = new List<EngineSystem>();
        Controllers = new List<Controller>();
    }

    public static MainScript GetInstance()
    {
        if (instance == null)
            instance = FindObjectOfType<MainScript>();
        return instance;
    }

    private void Initialize()
    {
        Systems.Add(new TurnManager());
        Systems.Add(new UnitSelectionManager());
        Systems.Add(FindObjectOfType<UnitActionManager>());
        Systems.Add(new MouseManager());
        Controllers.Add(FindObjectOfType<UIController>());
        Controllers.Add(FindObjectOfType<UnitsController>());
        InitPlayers();
        InitCharacters();
        gameState.enter();
    }

    void Update () {
        if (!init)
        {
            Initialize();
            init = true;
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
        LivingObject filler = null;
        LivingObject filler2 = null;
        LivingObject filler3 = null;
        LivingObject filler4 = null;
        filler = new Human("Leila");
        filler2 = new Human("Flora");
        filler3 = new Human("Eldric");
        filler4 = new Human("Hector");
        SpriteScript ss = GameObject.FindObjectOfType<SpriteScript>();
        filler.Sprite = ss.swordActiveSprite;
        filler2.Sprite = ss.axeActiveSprite;
        filler3.Sprite = ss.archerActiveSprite;
        filler4.Sprite = ss.lancerActiveSprite;

        p.AddUnit(filler);
        p.AddUnit(filler2);
        p.AddUnit(filler3);
        p.AddUnit(filler4);
        StartPosition[] startPositions = GameObject.FindObjectsOfType<StartPosition>();
        UnitInstantiater cc = GameObject.FindObjectOfType<UnitInstantiater>();
        cc.PlaceCharacter(0, filler, startPositions[0].GetXOnGrid(), startPositions[0].GetYOnGrid());
        cc.PlaceCharacter(0, filler2, startPositions[1].GetXOnGrid(), startPositions[1].GetYOnGrid());
        cc.PlaceCharacter(0, filler3, startPositions[2].GetXOnGrid(), startPositions[2].GetYOnGrid());
        cc.PlaceCharacter(0, filler4, startPositions[3].GetXOnGrid(), startPositions[3].GetYOnGrid());
        Monster monster = new Monster("Mammoth", MonsterType.Mammoth);
        turnManager.Players[1].AddUnit(monster);

        cc.PlaceCharacter(1, monster, 3, 2);
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
