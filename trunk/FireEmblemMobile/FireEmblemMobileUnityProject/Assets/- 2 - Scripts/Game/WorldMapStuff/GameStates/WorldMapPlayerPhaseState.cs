using Game.Manager;
using GameEngine;
using GameEngine.GameStates;

namespace Game.WorldMapStuff
{
    public class WorldMapPlayerPhaseState: GameState<NextStateTrigger>
    {
        private WorldMapInputSystem inputSystem;

        public WorldMapPlayerPhaseState()
        {
            inputSystem = new WorldMapInputSystem();
        }
        public override void Enter()
        {
         
            inputSystem.SetActive(true);
            
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

        public void Init()
        {
            inputSystem.Init();
        }
    }
}