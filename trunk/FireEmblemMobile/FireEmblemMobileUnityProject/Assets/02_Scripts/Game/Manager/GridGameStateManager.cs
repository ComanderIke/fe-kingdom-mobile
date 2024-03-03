using Game.AI;
using Game.States;
using Game.States.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Manager
{
    public class GridGameStateManager:GameStateManager
    {

        public PlayerPhaseState PlayerPhaseState { get; set; }
        public AIState EnemyPhaseState { get; set; }
        public GameOverState GameOverState { get; set; }
        public BattleState BattleState { get; set; }
        public MovementState MovementState { get; set; }
        public PhaseTransitionState PhaseTransitionState { get; set; }
        public UnitPlacementState UnitPlacementState { get; set; }
        //public ConditionsScreenState ConditionScreenState { get; set; }
        

        public GridGameStateManager()
        {
            ConditionManager conditionManager =
                new ChapterConditionManager(GridGameManager.Instance.BattleMap, GridGameManager.Instance.FactionManager);
            PlayerPhaseState = new PlayerPhaseState(conditionManager);
            EnemyPhaseState = new AIState(conditionManager);
            GameOverState = new GameOverState();
         
            BattleState = new BattleState();
            MovementState = new MovementState();
            PhaseTransitionState = new PhaseTransitionState(GridGameManager.Instance.FactionManager, this);
            //ConditionScreenState = new ConditionsScreenState();
            UnitPlacementState = new UnitPlacementState();
            stateMachine = new StateMachine<NextStateTrigger>(UnitPlacementState);
        }

        private bool gameStateTransitionsInitialized = false;
        public override void Init()
        {
            if(!gameStateTransitionsInitialized)
                InitGameStateTransitions();
            PlayerPhaseState.Init();
            UnitPlacementState.Init();
            base.Init();

        }

        private void InitGameStateTransitions()
        {
           
            EnemyPhaseState.AddTransition(PhaseTransitionState, NextStateTrigger.Transition);
        
            PlayerPhaseState.AddTransition(PhaseTransitionState, NextStateTrigger.Transition);
            
            PlayerPhaseState.AddTransition(GameOverState, NextStateTrigger.GameOver);
         
            //PlayerPhaseState.AddTransition(WinState, NextStateTrigger.PlayerWon);
        
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
            gameStateTransitionsInitialized = true;
        }

        public GameState<NextStateTrigger> GetActiveState()
        {
            return stateMachine.GetCurrentState();
        }
        
        public void OnDisable()
        {
            stateMachine.Exit();
        }
    }

  
}