using Game.AI;
using Game.Mechanics;
using Game.States;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Manager
{
    public class GameStateManager
    {

        public static PlayerPhaseState PlayerPhaseState { get; set; }
        public static AIState EnemyPhaseState { get; set; }
        public static GameOverState GameOverState { get; set; }
        public static WinState WinState { get; set; }
        public static BattleState BattleState { get; set; }
        public static MovementState MovementState { get; set; }
        public static PhaseTransitionState PhaseTransitionState { get; set; }

        private StateMachine<NextStateTrigger> stateMachine;

        public GameStateManager()
        {
            PlayerPhaseState = new PlayerPhaseState();
            EnemyPhaseState = new AIState();
            GameOverState = new GameOverState();
            WinState = new WinState();
            BattleState = new BattleState();
            MovementState = new MovementState();
            PhaseTransitionState = new PhaseTransitionState();
            
            stateMachine = new StateMachine<NextStateTrigger>(PlayerPhaseState);
        }
        public void Init()
        {
            InitGameStateTransitions();
            PlayerPhaseState.Init();
            stateMachine.Init();
            
        }
        private void InitGameStateTransitions()
        {
            EnemyPhaseState.AddTransition(PhaseTransitionState, NextStateTrigger.Transition);
            PlayerPhaseState.AddTransition(PhaseTransitionState, NextStateTrigger.Transition);
            PlayerPhaseState.AddTransition(GameOverState, NextStateTrigger.GameOver);
            PlayerPhaseState.AddTransition(WinState, NextStateTrigger.PlayerWon);
            PlayerPhaseState.AddTransition(BattleState, NextStateTrigger.BattleStarted);
            PlayerPhaseState.AddTransition(MovementState, NextStateTrigger.MoveUnit);
            EnemyPhaseState.AddTransition(MovementState, NextStateTrigger.MoveUnit);
            EnemyPhaseState.AddTransition(BattleState, NextStateTrigger.BattleStarted);
            PhaseTransitionState.AddTransition(PlayerPhaseState, NextStateTrigger.StartPlayerPhase);
            PhaseTransitionState.AddTransition(EnemyPhaseState, NextStateTrigger.StartEnemyPhase);
            //MovementState.AddTransition(PlayerPhaseState, NextStateTrigger.FinishedMovement);
           // BattleState.AddTransition(PlayerPhaseState, NextStateTrigger.BattleEnded);
            //MovementState.AddTransition(EnemyPhaseState, NextStateTrigger.FinishedAIMovement);
           // BattleState.AddTransition(EnemyPhaseState, NextStateTrigger.AIBattleEnded);
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

        public void SwitchState(GameState<NextStateTrigger> gameState)
        {
            stateMachine.SwitchState(gameState);
        }
    
        public void Update()
        {
            stateMachine.Update();
        }

    
    }

  
}