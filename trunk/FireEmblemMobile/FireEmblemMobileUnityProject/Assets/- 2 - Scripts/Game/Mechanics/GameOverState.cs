using Game.GUI;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Mechanics
{
    public class GameOverState : GameState<NextStateTrigger>
    {
        public IGameOverRenderer renderer;
        public override void Enter()
        {
            Debug.Log("Game Over");
            renderer.Show();
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