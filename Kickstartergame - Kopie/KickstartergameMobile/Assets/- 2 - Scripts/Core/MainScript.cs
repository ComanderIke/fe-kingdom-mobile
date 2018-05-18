using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.GameStates;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Players;

public class MainScript : MonoBehaviour {
	
    private const float TIME_LIMIT = 60;
    private const double START_TURN_DELAY = 0.3f;

    public static MainScript instance;

    [HideInInspector]
    public GameState gameState;
    public List<EngineSystem> Systems { get; set; }

    private bool init = false;

    void Awake()
    {
        instance = this;
        gameState = new GameplayState();
        Debug.Log("Initialize");
        AddSystems();
      
    }

    private void AddSystems()
    {
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
        Systems.Add(FindObjectOfType<UnitSelectionSystem>());
    }
    

    private void Initialize()
    {

        InitSystems();
        LevelConfig();
        gameState.Enter();
        TurnSystem.onStartTurn();
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
    private void InitSystems()
    {
       GetSystem<TurnSystem>().Init();
    }
    private void LevelConfig()
    {
        TurnSystem turnManager = GetSystem<TurnSystem>();
        Player player = turnManager.GetRealPlayer();
        StartPosition[] startPositions = FindObjectsOfType<StartPosition>();
        UnitInstantiator unitInstantiator = FindObjectOfType<UnitInstantiator>();
        RessourceScript resources = FindObjectOfType<RessourceScript>();
        DataScript data = FindObjectOfType<DataScript>();
        if (FindObjectOfType<GameData>() != null)
        {
            LoadGameData();
        }
        else
        {   
            Human unit1 = new Human("Leila", resources.sprites.GetCharacterOnMapSprites(0));
            Human unit2 = new Human("Flora", resources.sprites.GetCharacterOnMapSprites(2));
            Human unit3 = new Human("Eldric", resources.sprites.GetCharacterOnMapSprites(1));
            Human unit4 = new Human("Hector", resources.sprites.GetCharacterOnMapSprites(3));
            unit1.Inventory.AddItem(data.weapons.woodenSword);
            unit2.Inventory.AddItem(data.weapons.woodenAxe);
            unit3.Inventory.AddItem(data.weapons.basicBow);
            unit4.Inventory.AddItem(data.weapons.woodenSpear);
            player.AddUnit(unit1);
            player.AddUnit(unit2);
            player.AddUnit(unit3);
            player.AddUnit(unit4);
            unitInstantiator.PlaceCharacter(0, unit1, startPositions[0].GetXOnGrid(), startPositions[0].GetYOnGrid());
            unitInstantiator.PlaceCharacter(0, unit2, startPositions[1].GetXOnGrid(), startPositions[1].GetYOnGrid());
            unitInstantiator.PlaceCharacter(0, unit3, startPositions[2].GetXOnGrid(), startPositions[2].GetYOnGrid());
            unitInstantiator.PlaceCharacter(0, unit4, startPositions[3].GetXOnGrid(), startPositions[3].GetYOnGrid());
        }
        Monster saber = new Monster("Sabertooth", MonsterType.Sabertooth, resources.sprites.GetMonsterOnMapSprites(1));
        turnManager.Players[1].AddUnit(saber);
        unitInstantiator.PlaceCharacter(1, saber, 5, 2);
        saber.GridPosition.FacingLeft = true;
    }
    private void LoadGameData()
    {
        //TODO
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
