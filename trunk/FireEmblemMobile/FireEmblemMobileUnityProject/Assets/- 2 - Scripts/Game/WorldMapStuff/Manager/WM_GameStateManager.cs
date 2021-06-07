using Game.AI;
using Game.Mechanics;
using Game.States;
using Game.WorldMapStuff;
using Game.WorldMapStuff.GameStates;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
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
        public PhaseTransitionState PhaseTransitionState { get; set; }
        public Wm_WinState WM_WinState { get; set; }
        public WM_GameOverState WM_GameOverState { get; set; }
        

        

        public WM_GameStateManager()
        {
            ConditionManager conditionManager =new CampaignConditionManager(Campaign.Instance, WorldMapGameManager.Instance.FactionManager);
            PlayerPhaseState = new WM_PlayerPhaseState(WorldMapGameManager.Instance.GetSystem<TurnSystem>(), conditionManager);
            EnemyPhaseState = new WM_EnemyPhaseState(WorldMapGameManager.Instance.FactionManager, conditionManager);
            PhaseTransitionState = new PhaseTransitionState(WorldMapGameManager.Instance.FactionManager, this);
            WM_GameOverState = new WM_GameOverState();
            WM_WinState = new Wm_WinState();
            stateMachine = new StateMachine<NextStateTrigger>(PhaseTransitionState);
        }
        public override void Init()
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