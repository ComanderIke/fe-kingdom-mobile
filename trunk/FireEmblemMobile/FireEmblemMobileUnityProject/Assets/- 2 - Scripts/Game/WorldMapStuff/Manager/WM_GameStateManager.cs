using Game.AI;
using Game.Mechanics;
using Game.States;
using Game.WorldMapStuff;
using Game.WorldMapStuff.Manager;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Manager
{
    public class WM_GameStateManager:GameStateManager
    {

        public WM_PlayerPhaseState PlayerPhaseState{ get; set; }
        public WM_EnemyPhaseState EnemyPhaseState{ get; set; }
        public WM_BattleState BattleState{ get; set; }
        public WM_MovementState MovementState{ get; set; }
        public PhaseTransitionState PhaseTransitionState { get; set; }
        

        

        public WM_GameStateManager()
        {
            PlayerPhaseState = new WM_PlayerPhaseState(WorldMapGameManager.Instance.GetSystem<TurnSystem>());
            EnemyPhaseState = new WM_EnemyPhaseState(WorldMapGameManager.Instance.FactionManager);
            PhaseTransitionState = new PhaseTransitionState(WorldMapGameManager.Instance.FactionManager, this);
            
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
            EnemyPhaseState.AddTransition(PhaseTransitionState, NextStateTrigger.Transition);
            PlayerPhaseState.AddTransition(PhaseTransitionState, NextStateTrigger.Transition);
            //PlayerPhaseState.AddTransition(BattleState, NextStateTrigger.BattleStarted);
            //PlayerPhaseState.AddTransition(MovementState, NextStateTrigger.MoveUnit);
           // EnemyPhaseState.AddTransition(MovementState, NextStateTrigger.MoveUnit);
            //EnemyPhaseState.AddTransition(BattleState, NextStateTrigger.BattleStarted);
            PhaseTransitionState.AddTransition(PlayerPhaseState, NextStateTrigger.StartPlayerPhase);
            PhaseTransitionState.AddTransition(EnemyPhaseState, NextStateTrigger.StartEnemyPhase);
        }
        
    
    }
}