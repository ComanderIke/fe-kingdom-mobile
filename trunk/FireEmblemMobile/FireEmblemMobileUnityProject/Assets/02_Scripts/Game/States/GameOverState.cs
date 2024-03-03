using Game.Manager;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.States
{
    public class GameOverState : GameState<NextStateTrigger>
    {
        private const float DELAY =.3f;
        private float time;
        public IBattleLostRenderer renderer;
        public override void Enter()
        {
            Debug.Log("Battle Lost");
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
                        GameSceneController.Instance.LoadSanctuaryFromCampaign();
                    }
                }
            }

            return NextState;
        }
    }
}