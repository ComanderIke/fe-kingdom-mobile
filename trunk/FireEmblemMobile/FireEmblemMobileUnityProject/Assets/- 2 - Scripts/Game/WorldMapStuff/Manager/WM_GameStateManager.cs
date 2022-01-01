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
        public PhaseTransitionState PhaseTransitionState { get; set; }
        public Wm_WinState WM_WinState { get; set; }
        public WM_GameOverState WM_GameOverState { get; set; }

        


        public WM_GameStateManager()
        {
            ConditionManager conditionManager = new CampaignConditionManager(Campaign.Instance);

            PlayerPhaseState = new WM_PlayerPhaseState(WorldMapGameManager.Instance.GetSystem<TurnSystem>(), conditionManager);
           // PhaseTransitionState = new PhaseTransitionState(this); TODO new Transition for Area
            WM_GameOverState = new WM_GameOverState();
            WM_WinState = new Wm_WinState();
            stateMachine = new StateMachine<NextStateTrigger>(PlayerPhaseState);
        }
        public override void Init()
        {
            InitGameStateTransitions();
            PlayerPhaseState.Init();
            base.Init();
            
        }
        private void InitGameStateTransitions()
        {
            PlayerPhaseState.AddTransition(PhaseTransitionState, NextStateTrigger.Transition);
            PhaseTransitionState.AddTransition(PlayerPhaseState, NextStateTrigger.StartPlayerPhase);
        }

        public void Deactivate()
        {
           // stateMachine.Exit();
        }
    }
}