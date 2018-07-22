
using Assets.Scripts.Characters;
using Assets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameStates
{
    public class GameplayState : GameState<NextStateTrigger>
    {
        MainScript mainScript;
        public GameplayState()
        {
            mainScript = MainScript.instance;
            
        }

        public override void Enter()
        {
            mainScript.GetSystem<CameraSystem>().AddMixin<DragCameraMixin>();
            mainScript.GetSystem<CameraSystem>().AddMixin<SnapCameraMixin>();
            int height = mainScript.GetSystem<MapSystem>().grid.height;
            int width = mainScript.GetSystem<MapSystem>().grid.width;
            mainScript.GetSystem<CameraSystem>().AddMixin<ClampCameraMixin>().BoundsBorder(1).GridHeight(height).GridWidth(width).Locked(true);
            mainScript.GetSystem<CameraSystem>().AddMixin<ViewOnGridMixin>().zoom=0;
        }

        public override GameState<NextStateTrigger> Update()
        {
            CheckGameOver();
            return nextState;
        }

        public override void Exit()
        {
            mainScript.GetSystem<CameraSystem>().RemoveMixin<DragCameraMixin>();
            mainScript.GetSystem<CameraSystem>().RemoveMixin<SnapCameraMixin>();
            mainScript.GetSystem<CameraSystem>().RemoveMixin<ClampCameraMixin>();
            mainScript.GetSystem<CameraSystem>().RemoveMixin<ViewOnGridMixin>();
        }

        public void CheckGameOver()
        {
            foreach(Army p in mainScript.PlayerManager.Players)
            {
                if (p.IsPlayerControlled && !p.IsAlive())
                {
                    mainScript.GetSystem<UISystem>().ShowGameOver();
                    mainScript.GetSystem<InputSystem>().active = false;
                    mainScript.GameStateManager.Feed(NextStateTrigger.GameOver);
                    
                    return;
                }
                else if(!p.IsPlayerControlled && !p.IsAlive())
                {
                    mainScript.GetSystem<UISystem>().ShowWinScreen();
                    mainScript.GetSystem<InputSystem>().active = false;
                    mainScript.GameStateManager.Feed(NextStateTrigger.PlayerWon);
                    return;
                }
            }
        }

        

    }
}
