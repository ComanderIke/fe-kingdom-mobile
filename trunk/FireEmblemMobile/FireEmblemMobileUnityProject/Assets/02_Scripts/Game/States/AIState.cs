using Game.AI;
using Game.Manager;
using Game.States.Mechanics;
using Game.Systems;
using GameCamera;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.States
{
    public class AIState : GameState<NextStateTrigger>, IMainPhaseState
    {
       
        private readonly GridGameManager gridGameManager;
        private CameraSystem cameraSystem;
        private ConditionManager ConditionManager;
        private MainStateAI mainState;
        private StartOfTurnState startOfTurnAIState;
        private FactionManager factionManager;
        protected StateMachine<PPStateTrigger> stateMachine;
        public AIState(ConditionManager manager)
        {
            ConditionManager = manager;
            gridGameManager = GridGameManager.Instance;
            factionManager = gridGameManager.FactionManager;
            cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
            startOfTurnAIState = new StartOfTurnState(gridGameManager, factionManager, this);
            mainState = new MainStateAI(gridGameManager);
            startOfTurnAIState.Transitions.Add( PPStateTrigger.StartTurnFinished,mainState);
            gridGameManager.GetSystem<TurnSystem>().OnStartTurn+= StartOfTurn;
        }
        public override void Enter()
        {
            //Debug.Log("Enter AI State");
           
            stateMachine = new StateMachine<PPStateTrigger>(startOfTurnAIState);
            stateMachine.Init();
           

        }
        bool startTurnFinished = false;

        void StartOfTurn()
        {
            startTurnFinished = false;
        }

        public override void Exit()
        {
           // Debug.Log("Exit AI State");
           // cameraSystem.RemoveMixin<ClampCameraMixin>();
           // cameraSystem.RemoveMixin<ViewOnGridMixin>();
           // cameraSystem.RemoveMixin<FocusCameraMixin>();
        }

        public override GameState<NextStateTrigger> Update()
        {
            if (ConditionManager.CheckLose())
            {
                return  GridGameManager.Instance.GameStateManager.GameOverState;
            }
            else if (ConditionManager.CheckWin())
            {
                return WinState.Create();
            }
            stateMachine.Update();
           
            return NextState;

        }

        public void Feed(PPStateTrigger trigger)
        {
            stateMachine.Feed(trigger);
        }

        public void SetStartTurnFinished()
        {
            startTurnFinished = true;
        }
    }
}