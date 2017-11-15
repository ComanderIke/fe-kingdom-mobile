using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.GameStates;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;

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

    private static MainScript instance;
    
    void Awake()
    {
        gameState = new GameplayState();
    }

    void Start () {     
        gridManager = FindObjectOfType<GridManager>();
        Systems = new List<EngineSystem>();
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
