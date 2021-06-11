﻿using Game.WorldMapStuff.Controller;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.States
{
    public class WinState : GameState<NextStateTrigger>
    {
        private const float DELAY = 2.0f;
        private float time = 0;
        public IBattleSuccessRenderer renderer;
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
                    {
                        GameSceneController.Instance.LoadWorldMapFromBattle();
                    }
                }
            }
            return NextState;
        }
    }
}