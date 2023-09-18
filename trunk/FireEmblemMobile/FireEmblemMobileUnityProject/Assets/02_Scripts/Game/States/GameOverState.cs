using Game.GUI;
using Game.Manager;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;

namespace Game.Mechanics
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
                        GameSceneController.Instance.LoadEncounterAreaAfterBattle(false);
                    }
                }
            }

            return NextState;
        }
    }
}