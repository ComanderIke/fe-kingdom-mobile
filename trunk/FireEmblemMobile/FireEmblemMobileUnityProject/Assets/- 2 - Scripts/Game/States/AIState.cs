using Game.Manager;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.AI
{
    public class AIState : GameState<NextStateTrigger>
    {
        public const float PAUSE_BETWEEN_ACTIONS = 0.25f;
        private Brain brain;
        private ConditionManager ConditionManager;
        private float pauseTime;

        public AIState(ConditionManager manager)
        {
            ConditionManager = manager;
        }
        public override void Enter()
        {
            //Debug.Log("Enter AI State");
            brain = new Brain(GridGameManager.Instance.FactionManager.ActiveFaction);
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
                return  GridGameManager.Instance.GameStateManager.WinState;
            }
            
            pauseTime += Time.deltaTime;
            //wait so the player can follow what the AI is doing
            if (pauseTime >= PAUSE_BETWEEN_ACTIONS)
            {
                pauseTime = 0;

                if (!brain.IsFinished())
                {
                   // Debug.Log("THINK");
                    brain.Think();
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