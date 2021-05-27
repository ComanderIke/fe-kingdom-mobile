using GameEngine;
using GameEngine.GameStates;
using UnityEngine;


namespace Game.WorldMapStuff.GameStates
{
    public class Wm_WinState: GameState<NextStateTrigger>
    {
        private const float DELAY = 2.0f;
        private float time = 0;
        public IWinRenderer renderer;
        public override void Enter()
        {
            Debug.Log("Campaign Won");
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
                    {
                        WorldMapSceneController.Instance.LoadMainMenu();
                    }
                        
                }
            }

            return NextState;
        }
    }
}