using Assets.GameEngine;
using Assets.GameEngine.GameStates;
using UnityEngine;

namespace Assets.Game.GameStates
{
    public class GameOverState : GameState<NextStateTrigger>
    {
        public override void Enter()
        {
            Debug.Log("Game Over");
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