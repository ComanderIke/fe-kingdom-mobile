﻿using Game.Manager;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.AI
{
    public class AIState : GameState<NextStateTrigger>
    {
        public const float PAUSE_BETWEEN_ACTIONS = 0.5f;
        private Brain brain;

        private float pauseTime;

        public override void Enter()
        {
            //brain = new Brain(GridGameManager.Instance.FactionManager.ActiveFaction);
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

                //if (!brain.IsFinished())
                //{
                //    brain.Think();
                //}
                //else
                //{
                    TurnSystem.OnTriggerEndTurn();
                    GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.AISystemFinished);
                //}
            }

            return NextState;
        }
    }
}