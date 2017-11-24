
using Assets.Scripts.Characters;
using Assets.Scripts.Players;
using UnityEngine;

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
            
        }

        

    }
}
