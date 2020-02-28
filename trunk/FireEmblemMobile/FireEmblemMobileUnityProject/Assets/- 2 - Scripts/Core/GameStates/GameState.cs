
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameStates
{
    [System.Serializable]
    public abstract class GameState<TFeed>
    {
        public Dictionary<TFeed, GameState<TFeed>> Transitions { get; set; }
        protected GameState<TFeed> nextState;
        public GameState()
        {
            Transitions = new Dictionary<TFeed, GameState<TFeed>>();
        }

        public abstract void Enter();
       
        public abstract void Exit();

        public abstract GameState<TFeed> Update();

        public void AddTransition(GameState<TFeed> state, TFeed key)
        {
            Transitions.Add(key, state);
        }
        public GameState<TFeed> Feed(TFeed key)
        {
            if (Transitions.ContainsKey(key)) return Transitions[key];
            else return null;
        }
       

    }
}
