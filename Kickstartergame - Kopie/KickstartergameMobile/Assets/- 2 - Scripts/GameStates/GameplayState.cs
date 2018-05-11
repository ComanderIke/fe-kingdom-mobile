
using Assets.Scripts.Characters;
using Assets.Scripts.Players;
using System.Collections;
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
            mainScript = MainScript.GetInstance();
        }

        public override void enter()
        {
            
           
            active = true;
        }

        public override void update()
        {
            if(active)
                CheckGameOver();
        }

        public override void exit()
        {
            
        }

        public void CheckGameOver()
        {
            foreach(Player p in mainScript.GetSystem<TurnSystem>().Players)
            {
                if (p.IsHumanPlayer && !p.IsAlive())
                {
                    mainScript.GetSystem<UISystem>().ShowGameOver();
                    mainScript.GetSystem<InputSystem>().active = false;
                    CameraSystem.locked = true;
                    active = false;
                    return;
                }
                else if(!p.IsHumanPlayer && !p.IsAlive())
                {
                    mainScript.GetSystem<UISystem>().ShowWinScreen();
                    mainScript.GetSystem<InputSystem>().active = false;
                    CameraSystem.locked = true;
                    active = false;
                    return;
                }
            }
        }

        

    }
}
