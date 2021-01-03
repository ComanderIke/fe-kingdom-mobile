using System;
using System.Collections.Generic;

namespace GameEngine.GameStates
{
    [Serializable]
    public abstract class GameState<TFeed>
    {
        protected GameState<TFeed> NextState;

        protected GameState()
        {
            Transitions = new Dictionary<TFeed, GameState<TFeed>>();
        }

        public Dictionary<TFeed, GameState<TFeed>> Transitions { get; set; }

        public abstract void Enter();

        public abstract void Exit();

        public abstract GameState<TFeed> Update();

        public void AddTransition(GameState<TFeed> state, TFeed key)
        {
            Transitions.Add(key, state);
        }

        public GameState<TFeed> Feed(TFeed key)
        {
            return Transitions.ContainsKey(key) ? Transitions[key] : null;
        }
    }
}