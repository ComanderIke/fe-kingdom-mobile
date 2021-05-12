using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Input;
using Game.WorldMapStuff.Systems;
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
    

        public WM_PlayerPhaseState(TurnSystem turnSystem)
        {
            inputSystem = new WorldMapInputSystem();
            this.turnSystem = turnSystem;
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
            return NextState;
        }
        private void SetUpInputForUnits()
        {
            foreach (var party in GameObject.FindObjectsOfType<PartyController>())
            {
                party.inputReceiver=inputSystem;
            }
        }
        private void SetUpInputForLocations()
        {
            foreach (var location in GameObject.FindObjectsOfType<WorldMapPosition>())
            {
                location.inputReceiver = inputSystem;
            }
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