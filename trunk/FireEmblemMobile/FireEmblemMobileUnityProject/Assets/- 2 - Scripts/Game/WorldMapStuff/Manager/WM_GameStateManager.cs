using Game.AI;
using Game.Mechanics;
using Game.States;
using Game.WorldMapStuff;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Manager
{
    public class WM_GameStateManager:GameStateManager
    {

        public static  WM_PlayerPhaseState PlayerPhaseState{ get; set; }
        public static  WM_EnemyPhaseState EnemyPhaseState{ get; set; }
        public static  WM_BattleState BattleState{ get; set; }
        public static  WM_MovementState MovementState{ get; set; }
        public PhaseTransitionState PhaseTransitionState { get; set; }
        

        

        public WM_GameStateManager()
        {
            PlayerPhaseState = new WM_PlayerPhaseState();
            PhaseTransitionState = new PhaseTransitionState(WorldMapGameManager.Instance.FactionManager, WorldMapGameManager.Instance.GameStateManager);
            stateMachine = new StateMachine<NextStateTrigger>(PhaseTransitionState);
        }
        public void Init()
        {
            InitGameStateTransitions();
            PlayerPhaseState.Init();
            base.Init();
            
        }
        private void InitGameStateTransitions()
        {
           //EnemyPhaseState.AddTransition(PhaseTransitionState, NextStateTrigger.Transition);
            PlayerPhaseState.AddTransition(PhaseTransitionState, NextStateTrigger.Transition);
            //PlayerPhaseState.AddTransition(BattleState, NextStateTrigger.BattleStarted);
            //PlayerPhaseState.AddTransition(MovementState, NextStateTrigger.MoveUnit);
           // EnemyPhaseState.AddTransition(MovementState, NextStateTrigger.MoveUnit);
           // EnemyPhaseState.AddTransition(BattleState, NextStateTrigger.BattleStarted);
            PhaseTransitionState.AddTransition(PlayerPhaseState, NextStateTrigger.StartPlayerPhase);
            //PhaseTransitionState.AddTransition(EnemyPhaseState, NextStateTrigger.StartEnemyPhase);
        }
        
    
    }
}