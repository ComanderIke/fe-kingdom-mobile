using Game.GUI;
using Game.WorldMapStuff.Controller;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.WorldMapStuff.GameStates
{
    public class WM_GameOverState: GameState<NextStateTrigger>
    {


            private const float DELAY = 2.0f;
            private float time;
            public IGameOverRenderer renderer;
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
                            GameSceneController.Instance.UnloadAllExceptMainMenu();
                    }
                }

                return NextState;
            }
        }
    }
