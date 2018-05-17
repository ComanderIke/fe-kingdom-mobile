using Assets.Scripts;
using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    class AIState : GameState
    {
        AISystem aiSystem;

        public AIState(Player p)
        {
            aiSystem = new AISystem(p);

        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
            Debug.Log("Exit AI State");
        }

        public override void Update()
        {
            aiSystem.Update();
        }
    }
}
