using Assets.Core.GameStates;

namespace Assets.Core
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
            currentState.Update();
        }

        public void Init()
        {
            currentState.Enter();
        }

        public void Feed(TFeed input)
        {
            var nextState = currentState.Feed(input);
            SwitchState(nextState);
        }

        public void SwitchState(GameState<TFeed> nextState)
        {
            if (nextState != null)
            {
                currentState.Exit();
                currentState = nextState;
                nextState.Enter();
            }
        }
    }
}