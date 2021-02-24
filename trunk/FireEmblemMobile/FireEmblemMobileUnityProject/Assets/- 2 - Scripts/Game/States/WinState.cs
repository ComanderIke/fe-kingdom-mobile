using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;

namespace Game.Mechanics
{
    public class WinState : GameState<NextStateTrigger>
    {
        private const float DELAY = 2.0f;
        private float time = 0;
        public IWinRenderer renderer;
        public override void Enter()
        {
            Debug.Log("Player Won");
            time = 0;
            renderer.Show();
        }

        public override void Exit()
        {
        }

        public override GameState<NextStateTrigger> Update()
        {
            time += Time.deltaTime;
            if (time >= DELAY)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (time >= DELAY)
                        SceneController.SwitchScene("Base");
                }
            }

            return NextState;
        }
    }
}