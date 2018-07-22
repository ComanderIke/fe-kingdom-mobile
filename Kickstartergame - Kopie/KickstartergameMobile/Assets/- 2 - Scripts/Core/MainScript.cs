using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.GameStates;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Players;
using Assets.__2___Scripts.Mechanics;

public class MainScript : MonoBehaviour {
	
    private const float TIME_LIMIT = 60;
    private const double START_TURN_DELAY = 0.3f;

    public static MainScript instance;


    public List<EngineSystem> Systems { get; set; }
    public PlayerManager PlayerManager { get; set; }
    
    public GameStateManager GameStateManager { get; set; }

    private bool init = false;

    void Awake()
    {
        instance = this;
        
        Debug.Log("Initialize");
        AddSystems();
        PlayerManager = new PlayerManager();

    }

    private void AddSystems()
    {
        Systems = new List<EngineSystem>();
        Systems.Add(FindObjectOfType<UISystem>());
        Systems.Add(FindObjectOfType<CameraSystem>());
        Systems.Add(FindObjectOfType<MapSystem>());
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
        
        LevelConfig();
        GameStateManager = new GameStateManager();
        GameStateManager.Init();
        Systems.Add(new MoveSystem(GetSystem<MapSystem>()));
        TurnSystem.onStartTurn();
    }
    

    void Update () {
        if (!init)
        {
            Initialize();
            init = true;
        }
        GameStateManager.Update();
    }

    private void LevelConfig()
    {
        TurnSystem turnManager = GetSystem<TurnSystem>();
        Army player = PlayerManager.GetRealPlayer();
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
            Human unit1 = DataScript.instance.GetHuman("Leila");//new Human("Leila", resources.sprites.GetCharacterOnMapSprites(0), data.unitData.SwordFighterStats);
            Human unit2 = DataScript.instance.GetHuman("Flora");//new Human("Flora", resources.sprites.GetCharacterOnMapSprites(2), data.unitData.SpearFighterStats);
            Human unit3 = DataScript.instance.GetHuman("Eldric");//new Human("Eldric", resources.sprites.GetCharacterOnMapSprites(1), data.unitData.ArcherStats);
            Human unit4 = DataScript.instance.GetHuman("Hector"); //new Human("Hector", resources.sprites.GetCharacterOnMapSprites(3), data.unitData.AxeFighterStats);
            //unit1.Inventory.AddItem(data.GetWeapon("WoodenSword"));
            //unit2.Inventory.AddItem(data.GetWeapon("WoodenAxe"));
            //unit3.Inventory.AddItem(data.GetWeapon("WoodenBow"));
            //unit4.Inventory.AddItem(data.GetWeapon("WoodenSpear"));
            player.AddUnit(unit1);
            player.AddUnit(unit2);
            player.AddUnit(unit3);
            player.AddUnit(unit4);
            unitInstantiator.PlaceCharacter(0, unit1, startPositions[0].GetXOnGrid(), startPositions[0].GetYOnGrid());
            unitInstantiator.PlaceCharacter(0, unit2, startPositions[1].GetXOnGrid(), startPositions[1].GetYOnGrid());
            unitInstantiator.PlaceCharacter(0, unit3, startPositions[2].GetXOnGrid(), startPositions[2].GetYOnGrid());
            unitInstantiator.PlaceCharacter(0, unit4, startPositions[3].GetXOnGrid(), startPositions[3].GetYOnGrid());
        }
        UnitController[] player2Units = GameObject.Find("Player2").GetComponentsInChildren<UnitController>();
        foreach(UnitController uc in player2Units)
        {
            if (uc.Unit is Monster)
            {
                Monster m = (Monster)GameObject.Instantiate(uc.Unit);
                uc.Unit = m;
                PlayerManager.Players[1].AddUnit(m);
                m.GameTransform.GameObject = uc.gameObject;
                m.SetPosition((int)m.GameTransform.GetPosition().x, (int)m.GameTransform.GetPosition().y);

            }
        }
        

        //saber.GridPosition.FacingLeft = true;
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
