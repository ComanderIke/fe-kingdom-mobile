using Game.AI;
using Game.Mechanics;
using Game.States;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Manager
{
    public class GameStateManager
    {
        protected StateMachine<NextStateTrigger> stateMachine;

        public GameStateManager()
        {

            stateMachine = new StateMachine<NextStateTrigger>(null);
        }
        public virtual void Init()
        {
          
            stateMachine.Init();
            
        }
        public void Feed(NextStateTrigger trigger)
        {
            //Debug.Log("Feed State: "+trigger);
            stateMachine.Feed(trigger);
        }

        public void SwitchState(GameState<NextStateTrigger> gameState)
        {
            stateMachine.SwitchState(gameState);
        }
    
        public void Update()
        {
            stateMachine.Update();
        }
      
    }
}