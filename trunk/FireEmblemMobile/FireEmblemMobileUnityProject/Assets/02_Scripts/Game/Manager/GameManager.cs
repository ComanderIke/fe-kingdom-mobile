using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using GameEngine;
using SerializedData;
using UnityEngine;

namespace Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        private List<IEngineSystem> Systems { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public SessionManager SessionManager;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance == null)
            {
                Instance = this;
                SessionManager = new SessionManager();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            Instance.AddSystems();
            
        }
        private void AddSystems()
        {
            Systems = new List<IEngineSystem>
            {
                FindObjectOfType<AudioSystem>()
            };
        }
        public T GetSystem<T>()
        {
            foreach (var s in Systems.OfType<T>())
                return (T)Convert.ChangeType(s, typeof(T));
            return default;
        }
    }
}
