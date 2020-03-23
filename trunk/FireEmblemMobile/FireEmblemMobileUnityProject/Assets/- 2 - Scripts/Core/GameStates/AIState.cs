using Assets.AI;
using Assets.Mechanics;
using UnityEngine;

namespace Assets.Core.GameStates
{
    public class AIState : GameState<NextStateTrigger>
    {
        public const float PAUSE_BETWEEN_ACTIONS = 0.5f;
        private Brain brain;

        private float pauseTime;

        public override void Enter()
        {
            brain = new Brain(GridGameManager.Instance.FactionManager.ActiveFaction);
        }

        public override void Exit()
        {
        }

        public override GameState<NextStateTrigger> Update()
        {
            pauseTime += Time.deltaTime;
            //wait so the player can follow what the AI is doing
            if (pauseTime >= PAUSE_BETWEEN_ACTIONS)
            {
                pauseTime = 0;

                if (!brain.IsFinished())
                {
                    brain.Think();
                }
                else
                {
                    TurnSystem.OnEndTurn();
                    GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.AISystemFinished);
                }
            }

            return NextState;
        }
    }
}