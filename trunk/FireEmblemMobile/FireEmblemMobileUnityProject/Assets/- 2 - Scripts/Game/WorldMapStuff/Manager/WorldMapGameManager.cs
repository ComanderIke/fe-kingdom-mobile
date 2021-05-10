using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Game.GameActors.Players;
using Game.GUI;
using Game.GUI.Text;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Controller;
using GameEngine;
using UnityEditor.Build.Content;
using UnityEngine;

public class WorldMapGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static WorldMapGameManager Instance;
    private bool init;
    private List<IEngineSystem> Systems { get; set; }
    public FactionManager FactionManager { get; set; }
    public WM_GameStateManager GameStateManager { get; set; }

    private void Awake()
    {
        Instance = this;
        AddSystems();
        var config = GameObject.FindObjectOfType<WM_Playerconfig>();
        FactionManager = new FactionManager(config.factions.Cast<Faction>().ToList());
        GameStateManager = new WM_GameStateManager();
        Debug.Log("AWAKE" +GameStateManager);
        Application.targetFrameRate = 60;
    }

    private void AddSystems()
    {
        Systems = new List<IEngineSystem>
        {
            FindObjectOfType<AudioSystem>(),
            FindObjectOfType<TurnSystem>()
        };

    }
    private void Initialize()
    {
           
        InjectDependencies();
        foreach (var system in Systems)
        {
            system.Init();
        }
        GameStateManager.Init();
        //GetSystem<TurnSystem>().StartPhase();
    }
      private void InjectDependencies()
      {
          Debug.Log("Inject");
          GetSystem<TurnSystem>().factionManager = FactionManager;
          GetSystem<TurnSystem>().gameStateManager = GameStateManager;
          GameStateManager.PhaseTransitionState.phaseRenderer = FindObjectsOfType<MonoBehaviour>().OfType<IPhaseRenderer>().First();
          GameStateManager.PlayerPhaseState.playerPhaseUI = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerPhaseUI>().First();

      }
      private void Update()
      {
          if (!init)
          {
              Initialize();
              init = true;
          }

          GameStateManager.Update();
      }
      public T GetSystem<T>()
      {
          foreach (var s in Systems.OfType<T>())
              return (T) Convert.ChangeType(s, typeof(T));
          return default;
      }

}
