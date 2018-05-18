
using Assets.Scripts.Characters;
using Assets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameStates
{
    public class GameplayState : GameState
    {
        MainScript mainScript;
        bool active;
        public GameplayState()
        {
            mainScript = MainScript.instance;
            
        }

        public override void Enter()
        {
            mainScript.GetSystem<CameraSystem>().AddMixin<DragCameraMixin>();
            mainScript.GetSystem<CameraSystem>().AddMixin<SnapCameraMixin>();
            mainScript.GetSystem<CameraSystem>().AddMixin<ClampCameraMixin>().BoundsBorder(1).GridHeight(12).GridWidth(10).Locked(true);
            mainScript.GetSystem<CameraSystem>().AddMixin<ViewOnGridMixin>();
            active = true;
        }

        public override void Update()
        {
            if(active)
                CheckGameOver();
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
            foreach(Player p in mainScript.GetSystem<TurnSystem>().Players)
            {
                if (p.IsHumanPlayer && !p.IsAlive())
                {
                    mainScript.GetSystem<UISystem>().ShowGameOver();
                    mainScript.GetSystem<InputSystem>().active = false;
                    active = false;
                    return;
                }
                else if(!p.IsHumanPlayer && !p.IsAlive())
                {
                    mainScript.GetSystem<UISystem>().ShowWinScreen();
                    mainScript.GetSystem<InputSystem>().active = false;

                    active = false;
                    return;
                }
            }
        }

        

    }
}
