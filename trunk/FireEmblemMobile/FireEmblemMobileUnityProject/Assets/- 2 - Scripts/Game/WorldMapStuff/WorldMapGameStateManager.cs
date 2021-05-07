using Game.AI;
using Game.Mechanics;
using Game.States;
using Game.WorldMapStuff;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Manager
{
    public class WorldMapGameStateManager:GameStateManager
    {

        public static  WorldMapPlayerPhaseState PlayerPhaseState{ get; set; }

        

        public WorldMapGameStateManager()
        {
            PlayerPhaseState = new WorldMapPlayerPhaseState();
            stateMachine = new StateMachine<NextStateTrigger>(PlayerPhaseState);
        }
        public void Init()
        {
            InitGameStateTransitions();
            PlayerPhaseState.Init();
            base.Init();
            
        }
        private void InitGameStateTransitions()
        {
           
        }
        
    
    }
}