using Game.Manager;
using Game.WorldMapStuff.Systems;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.WorldMapStuff
{
    public class WM_PlayerPhaseState: GameState<NextStateTrigger>
    {
        private WorldMapInputSystem inputSystem;

        public WM_PlayerPhaseState()
        {
            inputSystem = new WorldMapInputSystem();
        }
        public override void Enter()
        { 
            inputSystem.SetActive(true);
         
            SetUpInputForLocations();
            SetUpInputForUnits();
           
            
        }

        public override void Exit()
        {
            inputSystem.SetActive(false);
        }

        public override GameState<NextStateTrigger> Update()
        {
            inputSystem.Update();
            return NextState;
        }
        private void SetUpInputForUnits()
        {
            foreach (var party in GameObject.FindObjectsOfType<WorldMapPlayerParty>())
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
            inputSystem.inputReceiver = new WorldMapGameplayInputReceiver();
            inputSystem.Init();
        }
    }
}