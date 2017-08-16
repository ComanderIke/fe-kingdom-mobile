
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Assets.Scripts.GameStates;
using Assets.Scripts.Characters.Skills;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Classes;
using AssemblyCSharp;
using Assets.Scripts.Items;

[System.Serializable]
public class AttackTarget
{
    public Character character;
    public AttackTarget(Character character)
    {
        this.character = character;
        
    }
}
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
    public delegate void CharacterClickedEvent(Character c);
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
    public const float CURSOROFFSET = 0.2f;
	public Character lastClickedCharacter;
    #endregion

    #region fields
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
	public Character activeCharacter = null;
    private AttackTarget attackTarget = null;
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
    /*
    void Awake()
    {
        gameState = new GameplayState();
    }

    void Start () {     
		
        characterList = new List<Character> ();
        chests = new List<TreasureChest>();
        doors = new List<Door>();
        doorSwitches = new List<DoorSwitch>();
		characterRooms = new Dictionary<string,int> ();
		itemDrops = new List<ItemDrop> ();
		level2events = new Level2Events (this);
		itemDescription = GameObject.Find ("ItemDescription");
		weaponDescriptionShort  = GameObject.Find ("WeaponDescriptionShort");
		weaponDescriptionLong = GameObject.Find ("WeaponDescriptionLong");
		itemDescription.SetActive (false);
		weaponDescriptionShort.SetActive (false);
		weaponDescriptionLong.SetActive (false);
		terraineffects = GameObject.Find (MAIN_GAME_OBJ).GetComponentsInChildren<TerrainEffectPosition> ().ToList<TerrainEffectPosition>();
		crystals = FindObjectsOfType<CrystalScript> ().ToList<CrystalScript>();
		dialogSystem = GetComponentInChildren<DialogSystem> ();
        GameObject cam = GameObject.Find(CAMERA_NAME);
        players = new List<Player> ();
		PlayerScript[] transform = GetComponentsInChildren<PlayerScript> ();
		for (int i = 0; i < transform.Length; i++) {
			players.Add (transform [i].player);
		}
		activePlayer = players[activePlayerNumber];
        levelscript = GetComponentInChildren<LevelUpDialogScript>();
		fieldHoveredEvent += Dummy;
		clickedOnField += FieldClicked;
		fightClickedEvent += Dummy;
		winGameEvent += Dummy;
		healthPotionUsedEvent += Dummy;
		endOfIntroStoryTextEvent += Dummy;
        characterClickedEvent += CharacterClicked;

		attackInfoBox += Dummy;
		endOfDisableEverythingExceptUseOfHealthPotionEvent += Dummy;
		globoFadeEvent += Dummy;
		moveCharacterEvent += Dummy;
        audioStart += Dummy;


        
    }
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
    void CharacterClicked(Character c)
    {
        SetActiveCharacter(c, false);
    }
  
    public void MoveCharacterTo(Character c, int x, int z,bool drag, GameState targetState)
    {
        if(gameState is GameplayState)
        {
            ((GameplayState)gameState).MoveCharacter(c, x, z,drag, targetState);
        }
    }
    public void MoveCharacterTo(Character c, List<Vector2> path, bool drag, GameState targetState)
    {
        if (gameState is GameplayState)
        {
            List<Vector3> movePath = new List<Vector3>();
            for (int i = 0; i < path.Count; i++)
            {
                movePath.Add(new Vector3(path[i].x, gridScript.GetHeight((int)path[i].x, (int)path[i].y), path[i].y));
                Debug.Log(movePath[i]);
            }
            ((GameplayState)gameState).MoveCharacter(c, movePath, drag, targetState);
        }
    }

 
	public static MainScript GetInstance(){

		return GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ();
	}

    #region GUI

	public Character GetCharacterByName(String name){

		foreach (Character c in characterList) {
			if (c.name == name)
				return c;
		}
		return null;
	}
    public void HideCharacterInfo()
    {
        CharacterView.SetActive(false);
    }
    public void UpdateCharacterInfo()
    {
        CharacterView.transform.position = Input.mousePosition;
    }
    

    #endregion

    public void PlaceCharacterOnField(int x, int z, Character character, Player player)
    {
        GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(player.number, character, x, gridScript.fields[x, z].height, z);
        player.addCharacter(character);
        characterList.Add(character);
        gridScript.fields[x, z].character = character;
        character.x = x;
        character.z = z;
    }
    private void PositionCharacters()
    {
        StartPosition[] pos = GetComponent<MapConfig>().GetStartPositions();
        EnemyPosition[] epos = GetComponent<MapConfig>().GetEnemyPositions();
        foreach (Player p in players)
        {
            if (p.number == 0)
            {
                foreach (Character c in p.getCharacters())
                {
                
                    for (int i = 0; i < pos.Length; i++)
                    {
                        if (c.characterClassType == pos[i].charType)
                        {

                            GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(p.number, c, pos[i].GetX(), gridScript.fields[pos[i].GetX(), pos[i].GetZ()].height, pos[i].GetZ());
                            characterList.Add(c);
                            gridScript.fields[pos[i].GetX(), pos[i].GetZ()].character = c;
                            c.x = pos[i].GetX();
                            c.z = pos[i].GetZ();
                            i = pos.Length;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < epos.Length; i++)
                {
                    Character c = new Character("AI" + i, epos[i].charType, epos[i].behaviour);
					c.AutomaticLevelUp (epos [i].GetLevel());
					c.items=epos [i].GetItems ();
					if(c.items!=null&&c.items.Count>0)
						c.EquipedWeapon = (Weapon)c.items [0];

                    GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(p.number, c, epos[i].GetX(), gridScript.fields[epos[i].GetX(), epos[i].GetZ()].height, epos[i].GetZ());
                    p.addCharacter(c);
                    characterList.Add(c);
					gridScript.fields[epos[i].GetX(), epos[i].GetZ()].character = c;
                    c.x = epos[i].GetX();
                    c.z = epos[i].GetZ();
                }
            }

        }
		players [1].getCharacters () [0].stats.attack = 7;
		players [1].getCharacters () [0].stats.accuracy = 4;
		players [1].getCharacters () [0].stats.speed = 4;
		players [1].getCharacters () [0].stats.defense = 5;
		players [1].getCharacters () [0].stats.maxHP = 25;
		players [1].getCharacters () [0].HP = players [1].getCharacters () [0].stats.maxHP;
    }
    public void CharacterSpriteButtonClicked(int number)
    {
		if (activePlayerNumber == 0 && gameState is GameplayState)
        {
			
			clickedCharacter =GetCharactersForHud()[number];
            gridScript.HideMovement();
            //MoveCameraTo(clickedCharacter.x, clickedCharacter.z);
            ((GameplayState)gameState).SetActiveCharacter(activePlayer.getCharacters()[number], true);
        }
    }
	


	public void InventoryButtonClicked(int number)
	{
		if (activePlayerNumber == 0)
		{
			if (activeCharacter != null)
			{
				if (gameState is GameplayState) {
					activeCharacter.items [number].use (activeCharacter);
				}
			}
		}
	}

    public List<Character> GetCharactersForHud()
    {
		List<Character> ret = new List<Character> ();
		foreach (Character c in players[0].getCharacters()) {
			ret.Add (c);
		}
        return ret;
    }
    private void Initialize()
    {




        chests = GetComponentsInChildren<TreasureChest>().ToList<TreasureChest>();
        doors = GetComponentsInChildren<Door>().ToList<Door>();
        doorSwitches = GetComponentsInChildren<DoorSwitch>().ToList<DoorSwitch>();
        gridScript = GetComponentInChildren<GridScript>();
		foreach (CrystalScript crystal in crystals)
		{
			int x = crystal.GetX();
			int z = crystal.GetZ();
			gridScript.fields[x, z].isAccessible = false;
		}
        foreach (TreasureChest chest in chests)
        {
            int x = chest.getX();
            int z = chest.getZ();
            gridScript.fields[x, z].isAccessible = false;
            gridScript.fields[x, z].chest = chest;
        }
        foreach (Door d in doors)
        {
            int x = d.getX();
            int z = d.getZ();
            gridScript.fields[x, z].isAccessible = false;
            gridScript.fields[x, z].door = d;
        }
        foreach (DoorSwitch d in doorSwitches)
        {
            int x = d.getX();
            int z = d.getZ();
        }
        CreateFillerCharacters();
        GameObject.Find(CURSOR_NAME).transform.position = new Vector3(activePlayer.getCharacters()[0].x + 0.5f, gridScript.fields[(int)activePlayer.getCharacters()[0].x, (int)activePlayer.getCharacters()[0].z].height + offset, activePlayer.getCharacters()[0].z + 0.5f);
		PositionCharacters();
       // clickedCharacter = activePlayer.getCharacters()[0];
        //GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(0, players[0].getCharacters()[0], 3, gridScript.fields[3, 2].height, 2);
        //GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(0, players[0].getCharacters()[1], 2, gridScript.fields[2, 2].height, 2);
        //GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(0, players[0].getCharacters()[2], 2, gridScript.fields[2, 3].height, 3);
        //GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(0, players[0].getCharacters()[3], 3, gridScript.fields[3, 3].height, 3);
        //characterList.Add(players[0].getCharacters()[0]);
        //characterList.Add(players[0].getCharacters()[1]);
        //characterList.Add(players[0].getCharacters()[2]);
        //characterList.Add(players[0].getCharacters()[3]);
        //gridScript.fields[3, 2].character = players[0].getCharacters()[0];
        //gridScript.fields[2, 2].character = players[0].getCharacters()[1];
        //gridScript.fields[2, 3].character = players[0].getCharacters()[2];
        //gridScript.fields[3, 3].character = players[0].getCharacters()[3];
        //GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(1, players[1].getCharacters()[0], 5, gridScript.fields[5, 5].height, 5);
        //GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(1, players[1].getCharacters()[1], 6, gridScript.fields[6, 5].height, 5);
        //GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(1, players[1].getCharacters()[2], 5, gridScript.fields[5, 6].height, 6);
        //GameObject.Find(MAIN_GAME_OBJ).GetComponent<CreateCharacter>().placeCharacter(1, players[1].getCharacters()[3], 6, gridScript.fields[6, 6].height, 6);
        //characterList.Add(players[1].getCharacters()[0]);
        //characterList.Add(players[1].getCharacters()[1]);
        //characterList.Add(players[1].getCharacters()[2]);
        //characterList.Add(players[1].getCharacters()[3]);
        //gridScript.fields[5, 5].character = players[1].getCharacters()[0];
        //gridScript.fields[6, 5].character = players[1].getCharacters()[1];
        //gridScript.fields[5, 6].character = players[1].getCharacters()[2];
        //gridScript.fields[6, 6].character = players[1].getCharacters()[3];
        activeCharacter = null;
        audioStart();

			//gameState = new TutorialState ();
		//	tutorial.enter ();
		//else {
			gameState.enter ();
		//}
    }



	float alpha=0;
	float fadetime=0;
	const float START_TIME = 2.5f;
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
        if (!(gameState is FightState) && !(gameState is SkillState))
        {
            if(moveCam&&!movingCam)
            UpdateCamera();
        }
        if (!gameStarted)
        {
            gameStarted = true;
            StartTurn();
        }
        //UpdateCharacters();
		//if (!MainScript.players [1].getCharacters () [9].isAlive)
		//	winGameEvent ();
		//level2events.Update ();
}

    #region Pro4 specific
    private void CreateFillerCharacters()
    {
		ItemSpriteScript iss = GameObject.Find ("RessourceScript").GetComponent<ItemSpriteScript> ();
		WeaponScript ws = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ();
        CharacterClassType type = CharacterClassType.Mage;
        Player p = players[0];
        Character filler = null;
        Character filler2 = null;
        Character filler3 = null;
        Character filler4 = null;
        Character filler5 = null;
        Character filler6 = null;
        filler = new Character(GetNameFromGenerator(true), CharacterClassType.Rogue);
        filler2 = new Character("Flora", CharacterClassType.Mage);
        filler3 = new Character("Eldric", CharacterClassType.Archer);
        filler4 = new Character(GetNameFromGenerator(false), CharacterClassType.Priest);
        filler5 = new Character("Leila", CharacterClassType.Hellebardier);
        filler6 = new Character("Hector", CharacterClassType.SwordFighter);
        //filler2.AutomaticLevelUp(level);
        //filler3.AutomaticLevelUp(level);
        //filler4.AutomaticLevelUp(level);
        //filler5.AutomaticLevelUp(level);
        //filler6.AutomaticLevelUp(level);
        foreach (Skill s in filler2.charclass.skills)
        {
            //if(s is Lightning)
            s.Level = 1;
        }
        foreach (Skill s in filler.charclass.skills)
        {
            //if (s is PassThrough)
                s.Level = 1;
        }
        foreach (Skill s in filler5.charclass.skills)
        {
            //if (s is )
                s.Level = 1;
        }
        foreach (Skill s in filler6.charclass.skills)
        {
            s.Level = 1;
        }
        foreach (Skill s in filler4.charclass.skills)
        {
            s.Level = 1;
        }
        foreach (Skill s in filler3.charclass.skills)
        {
            s.Level = 1;
        }

		//Tank 20, 10, 9, 3, 8, 4, 1
		// erhöht Hp3def1str3skl1magicdefense1
		//filler6.level=4;
		//filler6.stats.maxHP = 31;
		//filler6.HP = filler6.stats.maxHP;
		//filler6.stats.maxMana = 10;
		//filler6.stats.strength = 12;
		//filler6.stats.agility = 5;
		//filler6.stats.skill = 6;
		//filler6.stats.defense = 15;
		//filler6.stats.magicDefense = 2;


		filler5.items.Add (ws.spearPrincess);
		filler5.items.Add (iss.CreateHealthPotion ());
		filler5.EquipedWeapon = (Weapon)filler5.items [0];
		filler2.items.Add (ws.ignis);
		filler2.items.Add (iss.CreateHealthPotion ());
		filler2.EquipedWeapon = (Weapon)filler2.items [0];
		filler6.items.Add (ws.knightSword);
		//filler6.items.Add (ws.warAxe);
		filler6.items.Add (iss.CreateHealthPotion ());
		filler6.EquipedWeapon = (Weapon)filler6.items [0];
		filler3.items.Add (ws.recurveBow);
		filler3.EquipedWeapon = (Weapon)filler3.items [0];
       
        //p.addCharacter(filler4);
        //p.addCharacter(filler);

        p.addCharacter(filler2);
        p.addCharacter(filler5);
        p.addCharacter(filler6);
       // p.addCharacter(filler3);

    }

	public void NewItemDrop(ItemDrop drop){
		itemDrops.Add (drop);
	}
	public void DeleteItemDrop(ItemDrop drop){
		itemDrops.Remove (drop);
	}
    private String GetNameFromGenerator(bool male)
    {
        if (male)
        {
                int rng = UnityEngine.Random.Range(0, mNames.Length-1);
                return mNames[rng];
        }
        else{
                int rng = UnityEngine.Random.Range(0, wNames.Length - 1);
                return wNames[rng];
        }
    }
    #endregion

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
    Vector2 cameraTargetPosition;
	[HideInInspector]
    public bool moveCam = false;

	[HideInInspector]
    public bool movingCam = false;
    void UpdateCamera()
    {
        time += Time.deltaTime;
		Transform cam = GameObject.Find(CAMERA_NAME).transform.parent;
        if(time > delay)
        {
            float deltax = cameraTargetPosition.x - cam.localPosition.x;
            float deltaz = cameraTargetPosition.y - cam.localPosition.z;
            if (deltax == 0 && deltaz == 0)
                return;
            if (deltax > -0.2 && deltax < 0.2 && deltaz > -0.2 && deltaz < 0.2)// && speedincr == 1.0f)
            {
                //speedincr = 2.0f;
            }
            float distance = (Time.deltaTime / 1) * (deltax) * cameraSpeed;// * speedincr;
            float zdistance = (Time.deltaTime / 1) * (deltaz) * cameraSpeed;// * speedincr;
            if (deltax > -0.02 && deltax < 0.02 && deltaz > -0.02 && deltaz < 0.02)
            {
                cam.localPosition = new Vector3(cameraTargetPosition.x, cam.localPosition.y, cameraTargetPosition.y);
                time = 0.0f;
                //speedincr = 1.0f;
            }
            else
            {
                cam.localPosition = new Vector3(cam.localPosition.x + distance, cam.localPosition.y, cam.localPosition.z + zdistance);
            }
        }
    }
 //   void UpdateCamera(){
	//	time += Time.deltaTime;
 //       int offset = 2;
            
	//	GameObject cursor = GameObject.Find (CURSOR_NAME);
	//	GameObject cam = GameObject.Find (CAMERA_NAME);
 //       if (camIsFlipped)
 //       {
 //           offset *= -1;
 //       }
	//	if (time > delay) {
	//		float deltax = cursor.transform.position.x - cam.transform.localPosition.x;
	//		float deltaz = cursor.transform.position.z-offset - cam.transform.localPosition.z;
	//		if (deltax == 0 && deltaz == 0)
	//			return;
	//		if (deltax > -0.2 && deltax < 0.2 && deltaz > -0.2 && deltaz < 0.2&&speedincr==1.0f) {
	//			speedincr = 2.0f;
 //           }
           
 //           float distance = (Time.deltaTime / 1) * (deltax) * cameraSpeed*speedincr;
	//		float zdistance = (Time.deltaTime / 1) * (deltaz) * cameraSpeed*speedincr;
	//		if (deltax > -0.02 && deltax < 0.02 && deltaz > -0.02 && deltaz < 0.02) {
	//			cam.transform.localPosition = new Vector3 (cursor.transform.position.x, cam.transform.localPosition.y, cursor.transform.position.z-offset);
	//			time = 0.0f;
	//			actionMenueScript.CamLocked = true;
	//			speedincr = 1.0f;
 //           } else {
 //               cam.transform.localPosition = new Vector3 (cam.transform.position.x + distance, cam.transform.localPosition.y, cam.transform.localPosition.z + zdistance);
 //               actionMenueScript.CamLocked = false;
	//		}
	//	}
	//}
	[HideInInspector]
    public int turncount = 0;
    void StartTurn() {
        if (!activePlayer.isPlayerControlled)
            SwitchState(new AIState(activePlayer));
        init2 = true;
  //      Vector3 firstCharacterPosition = activePlayer.getCharacters ()[0].gameObject.transform.localPosition;
		//GameObject cursor = GameObject.Find (CURSOR_NAME);
		//cursor.GetComponent<CursorScript>().SetPosition(firstCharacterPosition.x ,firstCharacterPosition.y, firstCharacterPosition.z);
		//int x = (int)(cursor.transform.localPosition.x - 0.5f);
		//int z = (int)(cursor.transform.localPosition.z - 0.5f);
		//gridScript.fields [x, z].gameObject.GetComponent<FieldClicked> ().hovered = false;
        foreach (Character c in activePlayer.getCharacters())
        {
           c.UpdateTurn();
           GameObject.Instantiate(FindObjectOfType<UXRessources>().activeUnitField, c.gameObject.transform.position,Quaternion.identity,c.gameObject.transform);
        }
        
	}


    public void RotateCharacterTo(Character a, Character b)
    {
        a.SetRotation((int)getRotation(a, new AttackTarget(b)));
    }
    public void GoToEnemy(Character a, Character b, bool drag)
    {
        if(gameState is GameplayState)
            ((GameplayState)gameState).GoToEnemy(a, b, drag);
    }
    public float getRotation(global::Character a, AttackTarget b)
    {
       // Debug.Log(a.gameObject.transform.localPosition.x + " " + a.gameObject.transform.localPosition.z + " " + b.character.gameObject.transform.localPosition.x + " " + b.character.gameObject.transform.localPosition.z);
        int xa = (int)a.gameObject.transform.localPosition.x;
        int za = (int)a.gameObject.transform.localPosition.z;
        int xb = 0;
        int zb = 0;
        if (b.character != null)
        {
            xb =(int)b.character.gameObject.transform.localPosition.x;
            zb =(int)b.character.gameObject.transform.localPosition.z;
        }
        int deltax = Mathf.Abs(xa - xb);
        int deltaz = Mathf.Abs(za - zb);
        float value = 0;
        if (xa > xb)
        {
            if (za > zb)
            {
                if (deltax > deltaz)
                {
                    value = 45 + 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 45 - 22.5f;
                }
                else
                {
                    value = 45;
                }

            }
            else if (za < zb)
            {

                if (deltax > deltaz)
                {
                    value = 315 - 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 315 + 22.5f;
                }
                else
                {
                    value = 315;
                }

            }
            else
            {
                value = 270;
            }

        }
        else if (xa < xb)
        {
            if (za > zb)
            {

                if (deltax > deltaz)
                {

                    value = 225 - 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 225 - 22.5f;
                }
                else
                {
                    value = 225;
                }
            }
            else if (za < zb)
            {

                if (deltax > deltaz)
                {
                    value = 135 - 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 135 + 22.5f;
                }
                else
                {
                    value = 135;
                }
            }
            else
            {
                value = 90;
            }

        }
        if (za > zb)
        {
            if (xa > xb)
            {
                if (deltax > deltaz)
                {
                    value = 225 + 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 225 - 22.5f;
                }
                else
                {
                    value = 225;
                }
            }
            else if (xa < xb)
            {

                if (deltax > deltaz)
                {

                    value = 135 - 22.5f;

                }
                else if (deltax < deltaz)
                {
                    value = 135 + 22.5f;

                }
                else
                {
                    value = 135;
                }
            }
            else
            {
                value = 180;
            }

        }
        else if (za < zb)
        {
            if (xa > xb)
            {
                if (deltax > deltaz)
                {
                    value = 315 - 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 315 + 22.5f;
                }
                else
                {
                    value = 315;
                }
            }
            else if (xa < xb)
            {

                if (deltax > deltaz)
                {
                    value = 45 + 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 45 - 22.5f;
                }
                else
                {
                    value = 45;
                }
            }
            else
            {
                value = 0;
            }
        }
        return value;
    }


    public bool CheckAttackField(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < gridScript.grid.width && y < gridScript.grid.height)
        {
            if (!gridScript.fields[x, y].blockArrow)
                return true;
            
        }
        return false;
    }

    public bool CheckJumpField(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < gridScript.grid.width && y < gridScript.grid.height)
        {
            if (gridScript.fields[x, y].isJumpable||(gridScript.fields[x,y].door!=null&& gridScript.fields[x, y].door.opened))
                return true;
            else
                return false;
        }
        return false;
    }

    public void UpdateCharacters()
    {
        foreach (Player p in MainScript.players)
        {
            foreach (Character c in p.getCharacters())
            {
                foreach (Skill s in c.charclass.skills)
                {
                    s.Update();//Todo only update active Skills
                }
            }
        }
    }
    
    #region GetSurroundingTargets
    public void GetAttackableCharacters(Character character, int x, int y, int range, List<AttackTarget> characters, List<int> direction, List<Vector3> positions)
    {
        if (range <= 0)
        {
            Character c = gridScript.fields[x, y].character;
            //MeshRenderer m = gridScript.fields[x, y].gameObject.GetComponent<MeshRenderer>();
            //m.material.mainTexture = gridScript.AttackTexture;
            if (gridScript.fields[x, y].isAccessible)
            {
                if (gridScript.fields[x, y].door != null)
                {
                    if (gridScript.fields[x, y].door.opened)
                        if (!positions.Contains(new Vector3(x, gridScript.fields[x, y].height, y)))
                            positions.Add(new Vector3(x, gridScript.fields[x, y].height, y));
                }
                else if (!positions.Contains(new Vector3(x, gridScript.fields[x, y].height, y)))
                    positions.Add(new Vector3(x, gridScript.fields[x, y].height, y));
            }
            if (c != null && c.team != character.team&& c.isAlive)
            {
                bool contains = false;
                foreach (AttackTarget a in characters)
                {
                    if (a.character == c)
                    {
                        contains = true;

                    }
                }
                if (!contains)
                {
                    characters.Add(new AttackTarget(c));
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
                GetAttackableCharacters(character, x + 1, y, range - 1, characters, newdirection, positions);
            }
        }
        if (!direction.Contains(1))
        {
            if (CheckAttackField(x - 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                GetAttackableCharacters(character, x - 1, y, range - 1, characters, newdirection, positions);
            }
        }
        if (!direction.Contains(4))
        {
            if (CheckAttackField(x, y + 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                GetAttackableCharacters(character, x, y + 1, range - 1, characters, newdirection, positions);
            }
        }
        if (!direction.Contains(3))
        {
            if (CheckAttackField(x, y - 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                GetAttackableCharacters(character, x, y - 1, range - 1, characters, newdirection, positions);
            }
        }

    }
    public void GetAttackableCharacters(Character character, int x, int y, int range, List<AttackTarget> characters, List<int> direction)
    {
        if (range <= 0)
        {
            Character c = gridScript.fields[x, y].character;
         
            if (c != null && c.team != character.team && c.isAlive)
            {
                bool contains = false;
                foreach (AttackTarget a in characters)
                {
                    if (a.character == c)
                    {
                        contains = true;

                    }
                }
                if (!contains)
                {
                    characters.Add(new AttackTarget(c));
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
    public void GetTradeRange(int x, int y, int range, List<int> direction)
    {
        if (range <= 0)
        {
            Character c = gridScript.fields[x, y].character;
            MeshRenderer m = gridScript.fields[x, y].gameObject.GetComponent<MeshRenderer>();
            m.material.mainTexture = gridScript.MoveTexture;
            return;
        }
        if (!direction.Contains(2))
        {
            if (CheckAttackField(x + 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                GetTradeRange(x + 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(1))
        {
            if (CheckAttackField(x - 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                GetTradeRange(x - 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(4))
        {
            if (CheckAttackField(x, y + 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                GetTradeRange(x, y + 1, range - 1, newdirection);
            }
        }
        if (!direction.Contains(3))
        {
            if (CheckAttackField(x, y - 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                GetTradeRange(x, y - 1, range - 1, newdirection);
            }
        }

    }

    public List<Character> GetTradeMenuePartners()
    {
        int x = (int)activeCharacter.GetPositionOnGrid().x;
        int z = (int)activeCharacter.GetPositionOnGrid().y;
        List<Character> characters = new List<Character>();
        if (x + 1 < gridScript.grid.width)
        {
            Character c = gridScript.fields[x + 1, z].character;
            if (c != null && c.team == activeCharacter.team)
                characters.Add(c);
        }
        if (x - 1 >= 0)
        {
            Character c = gridScript.fields[x - 1, z].character;
            if (c != null && c.team == activeCharacter.team)
                characters.Add(c);
        }
        if (z + 1 < gridScript.grid.height)
        {

            Character c = gridScript.fields[x, z + 1].character;
            if (c != null && c.team == activeCharacter.team)
                characters.Add(c);
        }
        if (z - 1 >= 0)
        {
            Character c = gridScript.fields[x, z - 1].character;
            if (c != null && c.team == activeCharacter.team)
                characters.Add(c);
        }
        return characters;
    }
    public List<Character> GetAllyMovedUnits(Vector3 location, Character character)
    {
        int x = (int)location.x;
        int z = (int)location.z;
        List<Character> characters = new List<Character>();
        if (x + 1 < gridScript.grid.width)
        {
            Character c = gridScript.fields[x + 1, z].character;
            if (c != null && c.IsWaiting && c.team == character.team && c != character)
                characters.Add(c);
        }
        if (x - 1 >= 0)
        {
            Character c = gridScript.fields[x - 1, z].character;
            if (c != null && c.IsWaiting && c.team == character.team && c != character)
                characters.Add(c);
        }
        if (z + 1 < gridScript.grid.height)
        {
            Character c = gridScript.fields[x, z + 1].character;
            if (c != null && c.IsWaiting && c.team == character.team && c != character)
                characters.Add(c);
        }
        if (z - 1 >= 0)
        {
            Character c = gridScript.fields[x, z - 1].character;
            if (c != null && c.IsWaiting && c.team == character.team && c != character)
                characters.Add(c);
        }
        return characters;
    }
    public Character GetCharacterAtLocation(Vector3 location)
    {
        return gridScript.fields[(int)location.x, (int)location.z].character;
    }
    public void GetCharacters(int x, int y, int range, List<global::Character> characters, List<int> direction, int team, bool allies, bool showFields)
    {

        if (range <= 0)
        {
            if (showFields)
            {
                Debug.Log("test3");
                MeshRenderer m = gridScript.fields[x, y].gameObject.GetComponent<MeshRenderer>();
                if (!allies)
                    m.material.mainTexture = gridScript.AttackTexture;
                else
                    m.material.mainTexture = gridScript.healTexture;
            }
            global::Character c = gridScript.fields[x, y].character;
            if (c != null && ((c.team == team && allies) || (c.team != team && !allies)))
            {
                bool contains = false;
                foreach (global::Character a in characters)
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
                GetCharacters(x + 1, y, range - 1, characters, newdirection, team, allies, showFields);
            }
        }
        if (!direction.Contains(1))
        {
            if (CheckAttackField(x - 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                GetCharacters(x - 1, y, range - 1, characters, newdirection, team, allies, showFields);
            }
        }
        if (!direction.Contains(4))
        {
            if (CheckAttackField(x, y + 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                GetCharacters(x, y + 1, range - 1, characters, newdirection, team, allies,showFields);
            }
        }
        if (!direction.Contains(3))
        {
            if (CheckAttackField(x, y - 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                GetCharacters(x, y - 1, range - 1, characters, newdirection, team, allies, showFields);
            }
        }
    }
    public List<AttackTarget> GetAttackableTargetsAtLocation(Vector3 location, Character character)
    {
        List<AttackTarget> attackTargets = new List<AttackTarget>();
        int x = (int)location.x;
        int z = (int)location.z;
        foreach (int range in character.charclass.AttackRanges)
        {
            GetAttackableCharacters(character, x, z, range, attackTargets, new List<int>());
        }
        return attackTargets;
    }
    public int GetAttackRange(Vector3 location, Character defender)
    {
        int deltaX = Math.Abs((defender.x - (int)location.x));
        int deltaZ = Math.Abs((defender.z - (int)location.z));
        return (deltaX+deltaZ);
    }
	public List<ItemDrop> GetItemsOnGround(int x , int z){
		List<ItemDrop> drops = new List<ItemDrop> ();
		foreach (ItemDrop c in itemDrops)
		{
			if (c.GetX() == x && c.GetZ () == z)
				drops.Add (c);
		}
		return drops;
	}
	public List<CrystalScript> GetNearbyCrystals(int x , int z){
		
		List<CrystalScript> drops = new List<CrystalScript> ();
		foreach (CrystalScript c in crystals)
		{
			if (c.crystals <= 0)
				continue;
			if (c.GetX() == x + 1 && c.GetZ () == z)
				drops.Add (c);
			if (c.GetX() == x - 1 && c.GetZ() == z)
				drops.Add (c);
			if (c.GetX() == x && c.GetZ() == z + 1)
				drops.Add (c);
			if (c.GetX() == x && c.GetZ() == z - 1)
				drops.Add (c);
		}
		return drops;
	}

    public TreasureChest GetNearbyChest()
    {
        int x = (int)activeCharacter.GetPositionOnGrid().x;
        int z = (int)activeCharacter.GetPositionOnGrid().y;
        foreach (TreasureChest c in chests)
        {
            if (c.getX() == x + 1 && c.getZ() == z)
                return c;
            if (c.getX() == x - 1 && c.getZ() == z)
                return c;
            if (c.getX() == x && c.getZ() == z + 1)
                return c;
            if (c.getX() == x && c.getZ() == z - 1)
                return c;
        }
        return null;
    }

    public DoorSwitch GetNearbyDoorSwitch()
    {
        int x = (int)activeCharacter.GetPositionOnGrid().x;
        int z = (int)activeCharacter.GetPositionOnGrid().y;
        foreach (DoorSwitch c in doorSwitches)
        {
            if (!c.activated)
            {
                if (c.getX() == x + 1 && c.getZ() == z)
                    return c;
                if (c.getX() == x - 1 && c.getZ() == z)
                    return c;
                if (c.getX() == x && c.getZ() == z + 1)
                    return c;
                if (c.getX() == x && c.getZ() == z - 1)
                    return c;
            }
        }
        return null;
    }

    public void JumpPositionTargets(int x, int y, int range, List<int> direction, List<Vector3> positions)
    {
        if (range <= 0)
        {
            Character c = gridScript.fields[x, y].character;
            //MeshRenderer m = gridScript.fields[x, y].gameObject.GetComponent<MeshRenderer>();
            //m.material.mainTexture = gridScript.AttackTexture;
            if (gridScript.fields[x, y].isAccessible || gridScript.fields[x, y].door != null)
            {
                if (gridScript.fields[x, y].character == null)
                {
                    if (gridScript.fields[x, y].door != null)
                    {
                        if (gridScript.fields[x, y].door.opened)
                            if (!positions.Contains(new Vector3(x, gridScript.fields[x, y].height, y)))
                                positions.Add(new Vector3(x, gridScript.fields[x, y].height, y));
                    }
                    else if (!positions.Contains(new Vector3(x, gridScript.fields[x, y].height, y)))
                    {
                        positions.Add(new Vector3(x, gridScript.fields[x, y].height, y));
                    }
                }
            }
            return;
        }
        if (!direction.Contains(2))
        {
            if (CheckJumpField(x + 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                JumpPositionTargets(x + 1, y, range - 1, newdirection, positions);
            }
        }
        if (!direction.Contains(1))
        {
            if (CheckJumpField(x - 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                JumpPositionTargets(x - 1, y, range - 1, newdirection, positions);
            }
        }
        if (!direction.Contains(4))
        {
            if (CheckJumpField(x, y + 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                JumpPositionTargets(x, y + 1, range - 1, newdirection, positions);
            }
        }
        if (!direction.Contains(3))
        {
            if (CheckJumpField(x, y - 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                JumpPositionTargets(x, y - 1, range - 1, newdirection, positions);
            }
        }

    }
    public void ShowAttackRanges(int x, int y, int range, List<int> direction)
    {
        if (range <= 0)
        {
            Character c = gridScript.fields[x, y].character;
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


    public void ShowTradeRange(Character c)
    {
        int x = (int)c.GetPositionOnGrid().x;
        int z = (int)c.GetPositionOnGrid().y;
        GetTradeRange(x, z, 1, new List<int>());
    }

    public void ShowAttackRange(Character c)
    {
        List<AttackTarget> characters = new List<AttackTarget>();
        int x = (int)c.GetPositionOnGrid().x;
        int z = (int)c.GetPositionOnGrid().y;
        foreach (int range in c.charclass.AttackRanges)
        {
            ShowAttackRanges(x, z, range, new List<int>());
        }
    }

    public List<AttackTarget> GetAttackTargets(Character c){
        skillPositionTargets = new List<Vector3>();
        jumpPositionTargets = new List<Vector3>();
		int x = (int)c.GetPositionOnGrid().x;
        int z = (int)c.GetPositionOnGrid().y;
        List<AttackTarget> characters = new List<AttackTarget>();
        foreach (int range in c.charclass.AttackRanges)
        {
            GetAttackableCharacters(c, x, z, range, characters,new List<int>(), skillPositionTargets);
            JumpPositionTargets(x, z, range, new List<int>(), jumpPositionTargets);
        }
        return characters;
	}

    public List<TreasureChest> GetChestTargets()
    {
        int x = (int)activeCharacter.GetPositionOnGrid().x;
        int z = (int)activeCharacter.GetPositionOnGrid().y;
        List<TreasureChest> chests = new List<TreasureChest>();
        if (x + 1 < gridScript.grid.width)
        {
            TreasureChest c = gridScript.fields[x + 1, z].chest;
			if (c != null && !c.opened)
                chests.Add(c);
        }
        if (x - 1 >= 0)
        {
            TreasureChest c = gridScript.fields[x - 1, z].chest;
			if (c != null&& !c.opened)
                chests.Add(c);
        }
        if (z + 1 < gridScript.grid.height)
        {
            TreasureChest c = gridScript.fields[x, z + 1].chest;
			if (c != null&& !c.opened)
                chests.Add(c);
        }
        if (z - 1 >= 0)
        {
            TreasureChest c = gridScript.fields[x, z - 1].chest;
			if (c != null&& !c.opened)
                chests.Add(c);
        }
        return chests;
    }

    public List<Door> GetDoorTargets()
    {
        int x = (int)activeCharacter.GetPositionOnGrid().x;
        int z = (int)activeCharacter.GetPositionOnGrid().y;
        List<Door> doors = new List<Door>();
        Door c = gridScript.fields[x, z].door;
        if (c != null)
           doors.Add(c);
        return doors;
    }
    #endregion
    [HideInInspector]
    public Character clickedCharacter=null;
    #region RedirectingMethods
	public Dictionary<string, int> characterRooms;
	public void CharacterEnteredRoom(int number, Character character)
	{
		//Debug.Log (character.name + " entered Room: " + number);

		//for(int i=0; i< characterRooms.Values.Count; i++) {
		//	Debug.Log (characterRooms.Keys.ElementAt(i).name+" Room: "+characterRooms.Values.ElementAt(i));
		//}
		if(characterRooms.ContainsKey(character.name))
			characterRooms[character.name] = number;
		else{
			characterRooms.Add(character.name,number);
		}

	}
    public void ShowMovement(Character c)
    {
        gridScript.ShowMovement((int)c.gameObject.transform.localPosition.x, (int)c.gameObject.transform.localPosition.z, c.charclass.movRange, 0, new List<int>(c.charclass.AttackRanges), 0, c.team, false);
        gridScript.ShowAttack(c, new List<int>(c.charclass.AttackRanges), false);
    }
    public int GetDistance(int x, int z, int x2, int z2)
    {
        MovementPath p = gridScript.getPath(x, z, x2, z2, 0, true,new List<int>());
        //Debug.Log("PATHLENGTH " + (p.getLength() - 1));
        if (p == null)
            return 100;
        return p.getLength()-1;//Subtract Start Node
    }
    public void OnEndTurnClicked()
    {
        if(gameState is GameplayState)
        {
            ((GameplayState)gameState).EndTurn();
        }
    }
	public void OnBackButtonClicked()
	{
		Debug.Log ("UNDO");
		if(gameState is GameplayState)
		{
			((GameplayState)gameState).Back();
		}

	}

   
    public void DeselectActiveCharacter()
    {
        if(activeCharacter!=null)
        activeCharacter.Selected = false;
        MouseManager.ResetMoveArrow();
        // activeCharacter.gameObject.GetComponent<CharacterScript>().ForceIdle();
        Debug.Log("DeselectCharacter");
        activeCharacter = null;
        gridScript.HideMovement();
    }
    public void SetActiveCharacter(Character c, bool switchChar){
        Debug.Log("SetActive");
		lastClickedCharacter = c;
		if (activePlayerNumber == 0&& c.team==0&&activeCharacter==null)
        {
            clickedCharacter = c;
            gridScript.HideMovement();
        }
        if(gameState is GameplayState)
        {
            UXRessources ux = GameObject.FindObjectOfType<UXRessources>();
            foreach (global::Character chara in activePlayer.getCharacters())
            {
                if (!chara.hasMoved && chara.gameObject.GetComponentInChildren<ActiveUnitEffect>() == null)
                {
                    Debug.Log(chara.name + " Spawn effect");
                    GameObject.Instantiate(ux.activeUnitField, chara.gameObject.transform.position, Quaternion.identity, chara.gameObject.transform);
                }
            }
            if (activeCharacter == c)
            {
                Debug.Log("Deselect4");
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
 
		//fieldHoveredEvent(x, z);
    }
    #endregion

    public void ActiveCharWait(){
        if (activeCharacter != null && !activeCharacter.IsWaiting)
        {
            gridScript.HideMovement();
			TerrainEffectPosition tmp = null;
			foreach(TerrainEffectPosition t in terraineffects){
				if ((int)t.transform.position.x == activeCharacter.x && (int)t.transform.position.z == activeCharacter.z) {
					tmp = t;
					t.Effect(activeCharacter);
				}

			}
			if(tmp != null)
				terraineffects.Remove (tmp);
			lastClickedCharacter = null;
			HideCharacterInfo ();
            activeCharacter.gameObject.GetComponent<CharacterScript>().StopRun();
            activeCharacter.IsWaiting = true;
           // hasMoved = false;
            activeCharacter.Selected = false;
            activeCharacter = null;
        }

	}
    
    private void UnhoverAllCharacters()
    {
        foreach (Player p in players)
        {
            foreach (Character c in p.getCharacters())
            {
                c.hovered = false;
                //if(c.gameObject != null)
                //    c.gameObject.GetComponentInChildren<HighlightSelected>().Hovered = false;
            }
        }
	}



    public bool MoveCursorTo(int x, int z)
    {
        cameraSpeed = 3f;
        if (isFighting)
            return false;
        CursorScript c = GameObject.Find(CURSOR_NAME).GetComponent<CursorScript>();
        if (x < 0) 
        {
            c.GetComponent<CursorScript>().SetPosition(0, 0, z);
        }
        else if (z < 0 )
        {
            c.GetComponent<CursorScript>().SetPosition(x, 0, 0);
        }
        else if (x >= gridScript.grid.width)
        {
            c.GetComponent<CursorScript>().SetPosition(gridScript.grid.width-1, 0, z);
           
        }
        else if ( z >= gridScript.grid.height)
        {
            c.GetComponent<CursorScript>().SetPosition (x, 0, gridScript.grid.height-1);
        }
        else
        {
            c.transform.localPosition = new Vector3(x + 0.5f, gridScript.fields[x, z].height + CURSOROFFSET, z + 0.5f);
           
            if (OldCursorPosition != null && OldCursorPosition == new Vector2(x, z))
            {
            }
            else
            {
                GameObject.Find(CURSOR_NAME).GetComponentInChildren<AudioSource>().Play();
            }
            OldCursorPosition = new Vector2(x, z);
        }
        if (!gameStarted)
        {
            return true;
        }
        UnhoverAllCharacters();
        if (x < 0 || z < 0 || x >= gridScript.grid.width || z >= gridScript.grid.height)
            return true;
       
        if (gridScript.fields[x, z].character != null)
        {
            gridScript.fields[x, z].character.hovered = true;
            //if (activeCharacter != gridScript.fields[x, z].character)
            //    gridScript.fields[x, z].character.gameObject.GetComponentInChildren<HighlightSelected>().Hovered = true;
            if (activeCharacter != null)
            {

           }
            else
            {
                if (activePlayer.getCharacters().Contains(gridScript.fields[x, z].character))
                {
                        GameObject.Find(CURSOR_NAME).GetComponent<MeshRenderer>().material.color = activePlayer.GetColor();
                    //if (players[0].getCharacters().Contains(gridScript.fields[x, z].character))
                    //    ShowCharacterOnInfo(gridScript.fields[x, z].character);
                    //else
                    //    ShowCharacterOnInfo2(gridScript.fields[x, z].character);
                }
                else
                {
                    foreach(Player p in players)
                    {
                        if(p.getCharacters().Contains(gridScript.fields[x, z].character))
                        {
                            GameObject.Find(CURSOR_NAME).GetComponent<MeshRenderer>().material.color = p.GetColor();
                        }
                    }
                    //if (players[0].getCharacters().Contains(gridScript.fields[x, z].character))
                    //    ShowCharacterOnInfo(gridScript.fields[x, z].character);
                    //else
                    //    ShowCharacterOnInfo2(gridScript.fields[x, z].character);

                }
            }
        }
        return true;
    }

    #region AnimationEvents
    public void AttackAnimationEvent()
    {
        if (gameState is FightState)
        {
            ((FightState)gameState).AttackAnimationEvent();
            return;
        }
        if (gameState is SkillState)
        {
            ((SkillState)gameState).SkillAnimationEvent();
        }
    }
	public void DodgeEvent()
	{
		if (gameState is FightState)
		{
			((FightState)gameState).DodgeEvent();
			return;
		}
	}

    
  
    #endregion
    */
}
