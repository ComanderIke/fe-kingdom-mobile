using Assets.Core.GameStates;

namespace Assets.Core
{
    public class GameStateManager
    {

        public static GameplayState GameplayState { get; set; }
        public static AIState AIState { get; set; }
        public static GameOverState GameOverState { get; set; }
        public static WinState WinState { get; set; }

        private StateMachine<NextStateTrigger> stateMachine;

        public GameStateManager()
        {
            GameplayState = new GameplayState();
            AIState = new AIState();
            GameOverState = new GameOverState();
            WinState = new WinState();
            stateMachine = new StateMachine<NextStateTrigger>(GameplayState);
        
        }
        public void Init()
        {
            InitGameStateTransitions();
            stateMachine.Init();
        }
        private void InitGameStateTransitions()
        {
            AIState.AddTransition(GameplayState, NextStateTrigger.AISystemFinished);
            GameplayState.AddTransition(GameOverState, NextStateTrigger.GameOver);
            GameplayState.AddTransition(WinState, NextStateTrigger.PlayerWon);

        }
        public void SwitchState(GameState<NextStateTrigger> nextState)
        {
            stateMachine.SwitchState(nextState);
        }
        public void Feed(NextStateTrigger trigger)
        {
            stateMachine.Feed(trigger);
        }
    
        public void Update()
        {
            stateMachine.Update();
        }

    
    }
}