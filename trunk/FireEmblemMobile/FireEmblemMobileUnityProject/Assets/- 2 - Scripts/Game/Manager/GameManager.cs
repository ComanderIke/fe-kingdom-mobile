using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Audio;
using Assets.GameEngine;
using Assets.SerializedData;
using UnityEngine;

namespace Assets.Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get { return _instance = _instance ?? new GameManager(); }
        }

        public GameProgress GameProgress;
        public List<IEngineSystem> Systems { get; set; }
        public GameStateManager GameStateManager { get; set; }
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            _instance.AddSystems();
            
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
