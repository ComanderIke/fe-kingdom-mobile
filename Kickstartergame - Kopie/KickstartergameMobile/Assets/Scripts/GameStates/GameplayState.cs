
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

        public GameplayState()
        {
            mainScript = MainScript.GetInstance();
        }

        public override void enter()
        {
            
        }

        public override void update()
        {
            CheckGameOver();
        }

        public override void exit()
        {

        }

        public void CheckGameOver()
        {
            foreach(Player p in mainScript.GetSystem<TurnManager>().Players)
            {
                if (p.IsHumanPlayer && !p.IsAlive())
                {
                    mainScript.StartCoroutine(DelayLoadScene(2.0f));
                    return;
                }
                else if(!p.IsHumanPlayer && !p.IsAlive())
                {
                    mainScript.StartCoroutine(DelayLoadScene(2.0f));
                    return;
                }
            }
        }
        IEnumerator DelayLoadScene(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadSceneAsync("MainMenu");
        }

        

    }
}
