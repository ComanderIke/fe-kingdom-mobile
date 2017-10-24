
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.GameStates;
using Assets.Scripts.Characters.Skills;
using Assets.Scripts.Characters;

public class MainScript : MonoBehaviour {
	const float START_FADE = 3.0f;

	[HideInInspector]
	public static GameObject itemDescription;
	[HideInInspector]
	public static GameObject weaponDescriptionShort;
	[HideInInspector]
	public static GameObject weaponDescriptionLong;
   
    public delegate void AudioStart();
    public static AudioStart audioStart;

    public delegate void FieldHoveredEvent(int x, int z);
	public static FieldHoveredEvent fieldHoveredEvent;

 
	public delegate void WinGameEvent();
	public static WinGameEvent winGameEvent;

    public delegate void AfterAttackInfoBox();
	public static AfterAttackInfoBox afterAttackInfoBox;
	public delegate void EndOfAfterAttackInfoBox();
	public static EndOfAfterAttackInfoBox endOfAfterAttackInfoBox;
	public delegate void AttackInfoBox();
	public static AttackInfoBox attackInfoBox;
	public delegate void ClickedOnField(int x, int z);
	public static ClickedOnField clickedOnField;
	public delegate void FightClickedEvent();
	public static FightClickedEvent fightClickedEvent;
	public delegate void EndOfIntroductionEvent();
	public static EndOfIntroductionEvent endOfIntroductionEvent;
	public delegate void FallThroughHoleEvent(Character character, Vector3 translation);
	public static FallThroughHoleEvent fallThroughHoleEvent;
	public delegate void EndOfFallThroughHoleEvent();
	public static EndOfFallThroughHoleEvent endOfFallThroughHoleEvent;
    public delegate void CharacterKneelingEvent(Character character);
    public static CharacterKneelingEvent characterKneelingEvent;
	
    public delegate void GloboFadeEvent();
	public static GloboFadeEvent globoFadeEvent;
	public delegate void EndOfGloboFadeEvent();
	public static EndOfGloboFadeEvent endOfGloboFadeEvent;
	public delegate void MoveCharacterEvent(Character character,int x, int z);
	public static MoveCharacterEvent moveCharacterEvent;
	public delegate void EndOfMoveCharacterEvent();
	public static EndOfMoveCharacterEvent endOfMoveCharacterEvent;
	public delegate void FightEvent(Character attacker, Character defender);
	public static FightEvent fightEvent;
	public delegate void EndOfFightEvent();
	public static EndOfFightEvent endOfFightEvent;
	public delegate void CharGroundMovementAnimation(Character character);
	public static CharGroundMovementAnimation charGroundMovementAnimation;
	public delegate void EndOfCharGroundMovementAnimation();
	public static EndOfCharGroundMovementAnimation endOfCharGroundMovementAnimation;
	public delegate void EndOfcharStandUpAnimationEvent();
	public static EndOfcharStandUpAnimationEvent endOfcharStandUpAnimationEvent;
	public delegate void CharStandUpAnimationEvent(Character character);
	public static CharStandUpAnimationEvent charStandUpAnimationEvent;
	public delegate void EndOfhowToHealInfoEvent();
	public static EndOfhowToHealInfoEvent endOfhowToHealInfoEvent;
	public delegate void HowToHealInfoEvent();
	public static HowToHealInfoEvent howToHealInfoEvent;
	public delegate void HealthPotionUsedEvent();
	public static HealthPotionUsedEvent healthPotionUsedEvent;
	public delegate void MoveCamEvent(Vector3 position);
	public static MoveCamEvent moveCamEvent;
	public delegate void EndOfMoveCam();
	public static EndOfMoveCam endOfMoveCam;
	public delegate void DisableEverythingExceptUseOfHealthPotionEvent(Character character);
	public static DisableEverythingExceptUseOfHealthPotionEvent disableEverythingExceptUseOfHealthPotionEvent;
	public delegate void EndOfDisableEverythingExceptUseOfHealthPotionEvent();
	public static EndOfDisableEverythingExceptUseOfHealthPotionEvent endOfDisableEverythingExceptUseOfHealthPotionEvent;
    public delegate void CharacterClickedEvent(LivingObject c);
    public static CharacterClickedEvent characterClickedEvent;

    #region dialoge scene 1 
    public delegate void IntroStoryTextEvent(float duration);
    public static IntroStoryTextEvent introStoryTextEvent;
    public delegate void EndOfIntroStoryTextEvent();
    public static EndOfIntroStoryTextEvent endOfIntroStoryTextEvent;

    public delegate void IntroDustParticleEvent(float duration);
    public static IntroDustParticleEvent introDustParticleEvent;
    #endregion

    #region const
    private const float TIME_LIMIT = 60;
    private const double START_TURN_DELAY = 0.3f;
    private const String DATA_GO_NAME = "TransferenemToNextScene";
    private const String CAMERA_NAME = "Main Camera";
    public const String CURSOR_NAME = "Cursor";
    public const String PREFAB_CONTAINER = "PrefabObject";
    public const String FIGHT_CANVAS = "FightCanvas";
    private const String ATTACK_ANIMATION = "Attack1";
    private const String ATTACK_ANIMATION2 = "Attack2";
    public const String FIGHT_PANEL = "FightPanel";
    private const String ATTACK_MISS_TEXT = "missed!";
    private const String ATTACK_CRIT_TEXT = "Crit: ";
    public const String MAIN_GAME_OBJ = "Game";
    public const String CONTROL_IMG = "Controls";
    public const String XBUTTON_IMAGE = "XButtonImage";
    public const String YBUTTON_IMAGE = "YButtonImage";
    public const String ABUTTON_IMAGE = "AButtonImage";
    public const String BBUTTON_IMAGE = "BButtonImage";
    private const float DOUBLE_ATTACK_THRESHOLD = 1.3f;
    private const float delay = 0.2f;
    const float START_TIME = 2.5f;
    public const float CURSOROFFSET = 0.2f;
	public LivingObject lastClickedCharacter;
    #endregion

    #region fields
    float alpha = 0;
    float fadetime = 0;
  
    [HideInInspector]
    public int turncount = 0;
    public GameObject pauseMenu;
    public Canvas FightCanvas;
    [HideInInspector]
    public GameState gameState;
    public GameObject skillPreviewText;
    float CameraHeight = 5.0f;
    float CameraDegrees = 35f;//Diablo3 90-35
    float CameraDegrees2 = 65f;//Diablo3
    private static int activePlayerNumber;
    public GameObject CharacterView;
    public GameObject gameOverScreen;
    public GameObject CharacterScreenPrefab;
    public GameObject PlayerTurnAnimation;
    public GameObject AITurnAnimation;
	[HideInInspector]
    public bool canEndTurn = false;
    public Text turnTime;
    private float turn_Time = 0;
    public Image moveSpriteImage;
    public GameObject LevelUpDialog;
	[HideInInspector]
	public LivingObject activeCharacter = null;
    private LivingObject attackTarget = null;
	[HideInInspector]
	public List<Character> characterList;
	[HideInInspector]
    public Image faceSprite;
	public GridScript gridScript;
	public Camera myCamera;
    public CombatCamera combatCamera;
    [HideInInspector]
	public bool isAnimation = false;
	[HideInInspector]
	public Vector3 ClickedPoint;
	public static List<Player> players;
	[HideInInspector]
	public Player activePlayer;
	[HideInInspector]
    public List<Vector2> Startpositions;
	[HideInInspector]
    public List<Vector2> Startpositions2;
   
	//[HideInInspector]
	//public bool hasMoved = false;
	private bool fightRotationSetup = true;
	private bool isFighting = false;
    private bool isFightingAgainstWall = false;
    public UIController uiController;
	[HideInInspector]
	public Character fightCharacter = null;
	[HideInInspector]
	public Vector2 oldPosition;
	private ArrayList attackOrderList;
	private bool animationFlag = false;
    bool attackRangesOn = false;
	private int cnt = 0;
    bool gameStarted = false;
    bool startingTurn = false;
    double startingTurnTime = 0.0;
    bool isJumpAnimation = false;
    bool init = false;
    int CharacterNumber = 0;
    float time = 0.0f;
	[HideInInspector]
    public float cameraSpeed = 3f;
	//[HideInInspector]
    //public float speedincr = 1.0f;
    bool camIsFlipped = false;
    bool init2 = false;
	[HideInInspector]
    public int AttackRangeFromPath;
    Vector3 jumpPosition;
    Character jumpCharacter;
    bool flag = false;
    bool flag2 = false;
    float jumpdeltax;
    float jumpdeltaz;
	[HideInInspector]
    public bool isUsingSkill = false;
    Vector2 OldCursorPosition;
    Image image;
	[HideInInspector]
    public List<Vector3> skillPositionTargets;
	[HideInInspector]
    public List<Vector3> jumpPositionTargets;
    float offset = 0.3f;
    int charCounter = 0;
	public int levelnumber;
    Character skillTarget;
	[HideInInspector]
    public Character skillUser;
    Skill activeSkill;
    Vector3 skillPositionTarget;
    bool isChargeAnimation = false;
    Vector3 ChargePosition;
    List<Character> skillTargets;
    float bbuttontimepressed = 0;
    readonly String[] mNames = new String[26] { "Fritz", "Lanner", "Keppi", "Mazz", "Kosti", "Thü", "Haasi", "Lukas", "Philipp", "Manuel", "Can", "Bader", "AndiBa", "Boolean", "Wuwu", "Lacki", "Höbling", "Fly", "Klemens", "Artelsmair", "Jan", "Patrick", "Rene", "Cheng", "Hochi", "Lankes" };
    readonly String[] wNames = new String[10] { "Elli", "Eva", "Steffi", "Mini", "Kathi", "Nici", "Nora", "Meli", "Vali", "Bea" };
    bool flag3 = false;
    bool attack2 = false;
    #endregion
    
    public static int ActivePlayerNumber
    {
        get
        {
            return activePlayerNumber;
        }
        set
        {
            if (value >= players.Count)
                ActivePlayerNumber = 0;
            else
                activePlayerNumber = value;
        }
    }
    
    void Awake()
    {
        gameState = new GameplayState();
    }

    void Start () {     
        characterList = new List<Character> ();
        GameObject cam = GameObject.Find(CAMERA_NAME);
        players = new List<Player> ();
		PlayerScript[] transform = GetComponentsInChildren<PlayerScript> ();
		for (int i = 0; i < transform.Length; i++) {
			players.Add (transform [i].player);
		}
		activePlayer = players[activePlayerNumber];
		clickedOnField += FieldClicked;
		fightClickedEvent += Dummy;
		winGameEvent += Dummy;
		healthPotionUsedEvent += Dummy;
        characterClickedEvent += CharacterClicked;
		moveCharacterEvent += Dummy;
        audioStart += Dummy;
    }

    #region Dummys For Delegates
    void Dummy(Character c,int x, int z)
    {

    }
    void Dummy(Character c, Vector3 Pos){

	}
	void Dummy(Character c){

	}
	void Dummy(float f){
	}
	void Dummy(){
	}
	void Dummy(int x, int z){
	}
    #endregion

    void CharacterClicked(LivingObject c)
    {
        SetActiveCharacter(c, false);
    }
  
    public void MoveCharacterTo(LivingObject c, int x, int y,bool drag, GameState targetState)
    {
        if(gameState is GameplayState)
        {
            ((GameplayState)gameState).MoveCharacter(c, x, y,drag, targetState);
        }
    }
    public void MoveCharacterTo(LivingObject c, int x, int y,List<Vector2> path, bool drag, GameState targetState)
    {
        if (gameState is GameplayState)
        {
            List<Vector3> movePath = new List<Vector3>();
            for (int i = 0; i < path.Count; i++)
            {
                movePath.Add(new Vector2(path[i].x, path[i].y));
                Debug.Log(movePath[i]);
            }
            ((GameplayState)gameState).MoveCharacter(c, x, y,movePath, drag, targetState);
        }
    }

 
	public static MainScript GetInstance(){

		return GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ();
	}

    public void PlaceCharacterOnField(int x, int y, Character character, Player player)
    {
        GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(player.number, character, x, y);
        player.addUnit(character);
        characterList.Add(character);
        gridScript.fields[x, y].character = character;
        character.x = x;
        character.y = y;
    }
  

    private void Initialize()
    {
        gridScript = FindObjectOfType<GridScript>();

        CreateFillerCharacters();
        activeCharacter = null;
        audioStart();

		gameState.enter ();
    }

    void Update () {
        if (!init)
        {
            Initialize();
            init = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
        gameState.update();
        if (!gameStarted)
        {
            gameStarted = true;
            StartTurn();
        }
    }

    private void CreateFillerCharacters()
    {
        Player p = players[0];
        Character filler = null;
        Character filler2 = null;
        Character filler3 = null;
        Character filler4 = null;
        filler = new Character("Leila");
        filler2 = new Character("Flora");
        filler3 = new Character("Eldric");
        filler4 = new Character("Hector");
        WeaponScript ws = FindObjectOfType<WeaponScript>();
        filler.addItem(ws.bastardSword);
        filler.EquipedWeapon = ws.bastardSword;
        filler2.addItem(ws.warAxe);
        filler2.EquipedWeapon = ws.warAxe;
        filler3.addItem(ws.steelLance);
        filler3.EquipedWeapon = ws.steelLance;
        filler4.addItem(ws.recurveBow);
        filler4.EquipedWeapon = ws.recurveBow;
        SpriteScript ss = FindObjectOfType<SpriteScript>();
        filler.activeSpriteObject = ss.swordActiveSprite;
        filler2.activeSpriteObject = ss.axeActiveSprite;
        filler3.activeSpriteObject = ss.archerActiveSprite;
        filler4.activeSpriteObject = ss.lancerActiveSprite;

        p.addUnit(filler);
        p.addUnit(filler2);
        p.addUnit(filler3);
        p.addUnit(filler4);
        StartPosition[] startPositions = FindObjectsOfType<StartPosition>();
        CreateCharacter cc = FindObjectOfType<CreateCharacter>();
        cc.placeCharacter(0, filler, startPositions[0].GetX(), startPositions[0].GetY());
        cc.placeCharacter(0, filler2, startPositions[1].GetX(), startPositions[1].GetY());
        cc.placeCharacter(0, filler3, startPositions[2].GetX(), startPositions[2].GetY());
        cc.placeCharacter(0, filler4, startPositions[3].GetX(), startPositions[3].GetY());
        EnemyPosition[] enemyPosition = FindObjectsOfType<EnemyPosition>();
        Monster monster = new Monster("Mammoth",MonsterType.Mammoth);
        players[1].addUnit(monster);
        cc.placeCharacter(1, monster, enemyPosition[0].GetX(), enemyPosition[0].GetY());
    }

    public void SwitchState(GameState state)
    {
        // Debug.Log("from " + gameState + " to " + state);
        if (state != null)
        {
            gameState.exit();
            gameState = state;
            state.enter();
        }
        else
        {
            Debug.Log("SWITCHSTATENULL");
        }
    }



    void StartTurn() {
        if (!activePlayer.isPlayerControlled)
            SwitchState(new AIState(activePlayer));
        init2 = true;
        foreach (LivingObject c in activePlayer.getCharacters())
        {
           GameObject.Instantiate(FindObjectOfType<UXRessources>().activeUnitField, c.gameObject.transform.position,Quaternion.identity,c.gameObject.transform);
        }
        
	}

    public void GoToEnemy(LivingObject a, LivingObject b, bool drag)
    {
        if(gameState is GameplayState)
            ((GameplayState)gameState).GoToEnemy(a, b, drag);
    }
   

    public bool CheckAttackField(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < gridScript.grid.width && y < gridScript.grid.height)
        {
                return true;
        }
        return false;
    }


    public void UpdateCharacters()
    {

    }
    
    #region GetSurroundingTargets
   
    public void GetAttackableCharacters(LivingObject character, int x, int y, int range, List<LivingObject> characters, List<int> direction)
    {
        if (range <= 0)
        {
            LivingObject c = gridScript.fields[x, y].character;
         
            if (c != null && c.team != character.team && c.isAlive)
            {
                bool contains = false;
                foreach (LivingObject a in characters)
                {
                    if (a == c)
                    {
                        contains = true;

                    }
                }
                if (!contains)
                {
                    characters.Add(c);
                }
            }
            return;
        }
        if (!direction.Contains(2))
        {
            if (CheckAttackField(x + 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                GetAttackableCharacters(character, x + 1, y, range - 1, characters, newdirection);
            }
        }
        if (!direction.Contains(1))
        {
            if (CheckAttackField(x - 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                GetAttackableCharacters(character, x - 1, y, range - 1, characters, newdirection);
            }
        }
        if (!direction.Contains(4))
        {
            if (CheckAttackField(x, y + 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                GetAttackableCharacters(character, x, y + 1, range - 1, characters, newdirection);
            }
        }
        if (!direction.Contains(3))
        {
            if (CheckAttackField(x, y - 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                GetAttackableCharacters(character, x, y - 1, range - 1, characters, newdirection);
            }
        }

    }
    
    public LivingObject GetCharacterAtLocation(Vector3 location)
    {
        return gridScript.fields[(int)location.x, (int)location.z].character;
    }
 
    public List<LivingObject> GetAttackableTargetsAtLocation(Vector3 location, LivingObject character)
    {
        List<LivingObject> attackTargets = new List<LivingObject>();
        int x = (int)location.x;
        int z = (int)location.z;
        foreach (int range in character.AttackRanges)
        {
            GetAttackableCharacters(character, x, z, range, attackTargets, new List<int>());
        }
        return attackTargets;
    }
    
    public void ShowAttackRanges(int x, int y, int range, List<int> direction)
    {
        if (range <= 0)
        {
            LivingObject c = gridScript.fields[x, y].character;
            MeshRenderer m = gridScript.fields[x, y].gameObject.GetComponent<MeshRenderer>();
            m.material.mainTexture = gridScript.AttackTexture;
            return;
        }
        if (!direction.Contains(2))
        {
            if (CheckAttackField(x + 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                ShowAttackRanges(x + 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(1))
        {
            if (CheckAttackField(x - 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                ShowAttackRanges(x - 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(4))
        {
            if (CheckAttackField(x, y + 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                ShowAttackRanges(x, y + 1, range - 1, newdirection);
            }
        }
        if (!direction.Contains(3))
        {
            if (CheckAttackField(x, y - 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                ShowAttackRanges(x, y - 1, range - 1, newdirection);
            }
        }

    }

    public void ShowAttackRange(LivingObject c)
    {
        List<LivingObject> characters = new List<LivingObject>();
        int x = (int)c.GetPositionOnGrid().x;
        int z = (int)c.GetPositionOnGrid().y;
        foreach (int range in c.AttackRanges)
        {
            ShowAttackRanges(x, z, range, new List<int>());
        }
    }

    public List<LivingObject> GetAttackTargets(LivingObject c){
		int x = (int)c.GetPositionOnGrid().x;
        int z = (int)c.GetPositionOnGrid().y;
        List<LivingObject> characters = new List<LivingObject>();
        foreach (int range in c.AttackRanges)
        {
            GetAttackableCharacters(c, x, z, range, characters,new List<int>());
        }
        return characters;
	}

    #endregion
   
    #region RedirectingMethods

    public void ShowMovementAndAttack(LivingObject c)
    {
        gridScript.ShowMovement(c);
        gridScript.ShowAttack(c, new List<int>(c.AttackRanges), false);
    }

    public void EndTurn()
    {
        if(gameState is GameplayState)
        {
            ((GameplayState)gameState).EndTurn();
        }
    }

    public void DeselectActiveCharacter()
    {
        if(activeCharacter!=null)
            activeCharacter.Selected = false;
        MouseManager.ResetMousePath();
        activeCharacter = null;
        gridScript.HideMovement();
        uiController.HideTopUI();
        uiController.ShowBottomUI();
    }

    public void SetActiveCharacter(LivingObject c, bool switchChar){
		lastClickedCharacter = c;
		if (activePlayerNumber == 0&& c.team==0&&activeCharacter==null)
        {
            gridScript.HideMovement();
        }
        if(gameState is GameplayState)
        {
            UXRessources ux = GameObject.FindObjectOfType<UXRessources>();
            foreach (LivingObject chara in activePlayer.getCharacters())
            {
                if (!chara.hasMoved && chara.gameObject.GetComponentInChildren<ActiveUnitEffect>() == null)
                {
                    GameObject.Instantiate(ux.activeUnitField, chara.gameObject.transform.position, Quaternion.identity, chara.gameObject.transform);
                }
            }
            if (activeCharacter == c)
            {
                DeselectActiveCharacter();
            }
            else
            {
                ((GameplayState)gameState).SetActiveCharacter(c, switchChar);
            }
        }
        
    }

    void FieldClicked(int x, int z)
    {
        if (gameState is GameplayState)
        {
            ((GameplayState)gameState).FieldClicked(new Vector2(x,z));
        }
    }
    #endregion

    public void ActiveCharWait(){
        if (activeCharacter != null && !activeCharacter.isWaiting)
        {
            gridScript.HideMovement();
            activeCharacter.isWaiting = true;
            activeCharacter.Selected = false;
            activeCharacter = null;
            uiController.HideTopUI();
            uiController.ShowBottomUI();
        }
	}
}
