using Game.Manager;
using Game.Mechanics;
using Game.States;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.AI
{
    public class AIState : GameState<NextStateTrigger>
    {
        public const float PAUSE_BETWEEN_ACTIONS = 0.25f;
        private AISystem aiSystem;
        private ConditionManager ConditionManager;
        private float pauseTime;

        public AIState(ConditionManager manager)
        {
            ConditionManager = manager;
        }
        public override void Enter()
        {
            //Debug.Log("Enter AI State");
            if(aiSystem==null)
                aiSystem = GridGameManager.Instance.GetSystem<AISystem>();
            aiSystem.NewTurn();


        }

        public override void Exit()
        {
           // Debug.Log("Exit AI State");
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
                    Debug.Log("AI State Finished");
                    GridGameManager.Instance.GetSystem<TurnSystem>().OnTriggerEndTurn();
                }
            }
           
            return NextState;

        }
    }
}