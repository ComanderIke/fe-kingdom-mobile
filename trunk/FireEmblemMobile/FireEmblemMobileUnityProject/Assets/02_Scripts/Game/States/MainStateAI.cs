using Game.AI;
using Game.Manager;
using Game.States.Mechanics;
using Game.Systems;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.States
{
    public class MainStateAI : GameState<PPStateTrigger>
    {
        private AISystem aiSystem;
        private readonly GridGameManager gridGameManager;
       


        private float pauseTime;
        public const float PAUSE_BETWEEN_ACTIONS = 0.9f;//musst be higher than AISystem camera times


        public MainStateAI(GridGameManager gridGameManager)
        {
            this.gridGameManager = gridGameManager;
        }
        public override void Enter()
        {
            if(aiSystem==null)
                aiSystem = GridGameManager.Instance.GetSystem<AISystem>();
            aiSystem.NewTurn();
            //cameraSystem.AddMixin<FocusCameraMixin>().Construct();
            int height = gridGameManager.BattleMap.GetHeight();
            int width = gridGameManager.BattleMap.GetWidth();
            // cameraSystem.AddMixin<ClampCameraMixin>().Construct(width, height);
            //cameraSystem.AddMixin<ViewOnGridMixin>().Construct(width, height);
        }

        public override void Exit()
        {
            
        }

        public override GameState<PPStateTrigger> Update()
        {
            
            pauseTime += Time.deltaTime;
            //wait so the player can follow what the AI is doing
            if (pauseTime >= PAUSE_BETWEEN_ACTIONS)
            {
                pauseTime = 0;

                if (!aiSystem.IsFinished())
                {
                    // Debug.Log("THINK");
                    aiSystem.Think();
                }
                else
                {
                    // Debug.Log("AI State Finished");
                    GridGameManager.Instance.GetSystem<TurnSystem>().OnTriggerEndTurn();
                }
            }

            return NextState;
        }
    }
}