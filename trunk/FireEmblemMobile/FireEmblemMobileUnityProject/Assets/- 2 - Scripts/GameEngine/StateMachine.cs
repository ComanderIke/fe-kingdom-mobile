using GameEngine.GameStates;
using UnityEngine;

namespace GameEngine
{
    public class StateMachine<TFeed>
    {
        private GameState<TFeed> currentState;

        public StateMachine(GameState<TFeed> initialState)
        {
            currentState = initialState;
        }

        public void Update()
        {
          
            SwitchState(currentState.Update());
        }

        public void Init()
        {
            currentState.Enter();
        }

        public void Feed(TFeed input)
        {
            var nextState = currentState.Feed(input);
            //Debug.Log("Feed "+input+" Next State: "+
            if(nextState==null)
                Debug.LogError("Feed not resulting in State " + input +" "+currentState);
            SwitchState(nextState);
            
        }



        public void SwitchState(GameState<TFeed> nextState)
        {
            if (nextState != null)
            {
                currentState.Exit();
                nextState.PreviousState = currentState;
                Debug.Log("SwitchState: "+ nextState+" PreviousState: "+nextState.PreviousState);
                currentState = nextState;
                nextState.Enter();
            }
        }
    }
}