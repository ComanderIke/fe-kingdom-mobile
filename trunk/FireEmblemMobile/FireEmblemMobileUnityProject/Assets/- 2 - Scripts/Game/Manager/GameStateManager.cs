using Game.AI;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Manager
{
    public class GameStateManager
    {

        public static GameplayState GameplayState { get; set; }
        public static AIState AIState { get; set; }
        public static GameOverState GameOverState { get; set; }
        public static WinState WinState { get; set; }
        public static BattleState BattleState { get; set; }
        public static MovementState MovementState { get; set; }

        private StateMachine<NextStateTrigger> stateMachine;

        public GameStateManager()
        {
            GameplayState = new GameplayState();
            AIState = new AIState();
            GameOverState = new GameOverState();
            WinState = new WinState();
            BattleState = new BattleState(); 
            
            MovementState = new MovementState(); 
            stateMachine = new StateMachine<NextStateTrigger>(GameplayState);
        }
        public void Init()
        {
            InitGameStateTransitions();
            stateMachine.Init();
        }
        private void InitGameStateTransitions()
        {
            //AIState.AddTransition(GameplayState, NextStateTrigger.AISystemFinished);TODO MAKE AI A SUBPART OF GAMEPLAYSTATE

            GameplayState.AddTransition(GameOverState, NextStateTrigger.GameOver);
            GameplayState.AddTransition(WinState, NextStateTrigger.PlayerWon);
            GameplayState.AddTransition(BattleState, NextStateTrigger.BattleStarted);
            GameplayState.AddTransition(MovementState, NextStateTrigger.MoveUnit);
            MovementState.AddTransition(GameplayState, NextStateTrigger.FinishedMovement);//TODO NOT WORKING FOR AI
            BattleState.AddTransition(GameplayState, NextStateTrigger.BattleEnded);
        }
        // public void SwitchState(GameState<NextStateTrigger> nextState)
        // {
        //     Debug.Log("Switch State: "+nextState);
        //     stateMachine.SwitchState(nextState);
        // }
        public void Feed(NextStateTrigger trigger)
        {
            Debug.Log("Feed State: "+trigger);
            stateMachine.Feed(trigger);
        }
    
        public void Update()
        {
            stateMachine.Update();
        }

    
    }
}