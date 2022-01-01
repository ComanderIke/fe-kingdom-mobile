using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Systems;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.WorldMapStuff
{
    public class WM_PlayerPhaseState: GameState<NextStateTrigger>
    {
        public IPlayerPhaseUI playerPhaseUI;
        public TurnSystem turnSystem;
        private ConditionManager ConditionManager;

        public WM_PlayerPhaseState(TurnSystem turnSystem, ConditionManager conditionManager)
        {
            this.turnSystem = turnSystem;
            ConditionManager = conditionManager;
        }
        public override void Enter()
        {

            playerPhaseUI.Show(turnSystem.TurnCount);
            
        }

        public override void Exit()
        {
            playerPhaseUI.Hide();
            
        }

        public override GameState<NextStateTrigger> Update()
        {
            if (ConditionManager.CheckLose())
            {
                return  WorldMapGameManager.Instance.GameStateManager.WM_GameOverState;
            }
            else if (ConditionManager.CheckWin())
            {
                return  WorldMapGameManager.Instance.GameStateManager.WM_WinState;
            }
            return NextState;
        }

        public void Init()
        {

        }
    }
}