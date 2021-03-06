﻿using Assets.Core.GameStates;
using Assets.GameInput;
using UnityEngine;

namespace Assets.Core
{
    public class WinState : GameState<NextStateTrigger>
    {
        private const float DELAY = 2.0f;
        private float time = 0;
        public override void Enter()
        {
            Debug.Log("Player Won");
            time = 0;
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
                    Debug.Log("SWITCHSCENE");
                    if (time >= DELAY)
                        SceneController.SwitchScene("Level2");
                }
            }

            return NextState;
        }
    }
}