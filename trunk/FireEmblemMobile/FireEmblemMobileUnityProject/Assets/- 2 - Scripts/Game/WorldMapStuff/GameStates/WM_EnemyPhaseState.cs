using Game.Manager;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.AI
{
    public class WM_EnemyPhaseState : GameState<NextStateTrigger>
    {
        public const float PAUSE_BETWEEN_ACTIONS = 0.5f;
        private IBrain brain;
        private ConditionManager ConditionManager;
        private float pauseTime;
        private FactionManager factionManager;

        public WM_EnemyPhaseState(FactionManager factionManager)
        {
            ConditionManager = new ConditionManager();
            this.factionManager= factionManager;
        }
        public override void Enter()
        {
            brain = new WM_Brain(factionManager.ActiveFaction);
        }

        public override void Exit()
        {
        }

        public override GameState<NextStateTrigger> Update()
        {
            
            // if (ConditionManager.CheckLose(factionManager.Factions))
            // {
            //     return  WorldMapGameManager.Instance.GameStateManager.GameOverState;
            // }
            // else if (ConditionManager.CheckWin(factionManager.Factions))
            // {
            //     return  WorldMapGameManager.Instance.GameStateManager.WinState;
            // }
            
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
                    Debug.Log("Ending Turn with WM_AI!");
                    TurnSystem.OnTriggerEndTurn();
                }
            }
           
            return NextState;

        }
    }
}