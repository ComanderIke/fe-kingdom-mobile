using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Game.GUI;
using Game.Manager;
using Game.Mechanics;
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
    public GameStateManager GameStateManager { get; set; }

    private void Awake()
    {
        Instance = this;
        AddSystems();
        FactionManager = new FactionManager();
        GameStateManager = new GameStateManager();
        Application.targetFrameRate = 60;
    }

    private void AddSystems()
    {
        Systems = new List<IEngineSystem>
        {
            FindObjectOfType<AudioSystem>()
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
        GetSystem<TurnSystem>().StartPhase();
    }
      private void InjectDependencies()
      {


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
