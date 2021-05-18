using Game.Manager;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.AI
{
    public class AIState : GameState<NextStateTrigger>
    {
        public const float PAUSE_BETWEEN_ACTIONS = 0.5f;
        private IBrain brain;
        private ConditionManager ConditionManager;
        private float pauseTime;

        public AIState()
        {
            ConditionManager = new ConditionManager();
        }
        public override void Enter()
        {
            Debug.Log("Enter AI State");
            brain = new Brain(GridGameManager.Instance.FactionManager.ActiveFaction);
        }

        public override void Exit()
        {
            Debug.Log("Exit AI State");
        }

        public override GameState<NextStateTrigger> Update()
        {
            
            if (ConditionManager.CheckLose(GridGameManager.Instance.FactionManager.Factions))
            {
                return  GridGameManager.Instance.GameStateManager.GameOverState;
            }
            else if (ConditionManager.CheckWin(GridGameManager.Instance.FactionManager.Factions))
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
                    Debug.Log("THINK");
                    brain.Think();
                }
                else
                {
                    Debug.Log("AI State Finished");
                    TurnSystem.OnTriggerEndTurn();
                }
            }
           
            return NextState;

        }
    }
}