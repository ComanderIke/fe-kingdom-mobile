using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Systems;
using Game.WorldMapStuff.WM_Input;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.WorldMapStuff
{
    public class WM_PlayerPhaseState: GameState<NextStateTrigger>
    {
        private WorldMapInputSystem inputSystem;
        public IPlayerPhaseUI playerPhaseUI;
        public TurnSystem turnSystem;
        private ConditionManager ConditionManager;

        public WM_PlayerPhaseState(TurnSystem turnSystem, ConditionManager conditionManager)
        {
            inputSystem = new WorldMapInputSystem();
            this.turnSystem = turnSystem;
            ConditionManager = conditionManager;
        }
        public override void Enter()
        { 

           
            inputSystem.SetActive(true);
         
            SetUpInputForLocations();
            SetUpInputForUnits();
            playerPhaseUI.Show(turnSystem.TurnCount);
            
        }

        public override void Exit()
        {
            inputSystem.SetActive(false);
            playerPhaseUI.Hide();
            
        }

        public override GameState<NextStateTrigger> Update()
        {
            inputSystem.Update();
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
        private void SetUpInputForUnits()
        {
            foreach (var party in GameObject.FindObjectsOfType<WM_ActorController>())
            {
                party.inputReceiver=inputSystem;
                
            }
        }
        private void SetUpInputForLocations()
        {
            foreach (var position in GameObject.FindObjectsOfType<WorldMapPosition>())
            {
                
                position.SetInputReceiver(inputSystem);
            }

            GameObject.FindObjectOfType<WorldMapInputController>().InputReceiver=inputSystem;
        }

        public void Init()
        {
            inputSystem.inputReceiver = new WorldMapGameplayInputReceiver(WorldMapGameManager.Instance.FactionManager, 
                WorldMapGameManager.Instance.GetSystem<WM_PartySelectionSystem>(),
                new WM_GameplayInput(WorldMapGameManager.Instance.GetSystem<WM_PartySelectionSystem>(), 
                    WorldMapGameManager.Instance.GetSystem<WM_PartyActionSystem>()));
            inputSystem.Init();
        }
    }
}