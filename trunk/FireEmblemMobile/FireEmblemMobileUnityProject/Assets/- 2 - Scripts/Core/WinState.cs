using Assets.Core.GameStates;
using UnityEngine;

namespace Assets.Core
{
    public class WinState : GameState<NextStateTrigger>
    {
        public override void Enter()
        {
            Debug.Log("Player Won");
        }

        public override void Exit()
        {
        }

        public override GameState<NextStateTrigger> Update()
        {
            return NextState;
        }
    }
}