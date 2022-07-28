using Game.AI;
using Game.Grid;
using Game.GUI;
using Game.Manager;
using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;

namespace Game.States
{
    public class ConditionsScreenState : GameState<NextStateTrigger>
    {
        private const float DELAY = 0.5f;
        private float time = 0;
        public BattleMap chapter;
        
        public override void Enter()
        {
 
            NextState =  GridGameManager.Instance.GameStateManager.UnitPlacementState;
            GridGameManager.Instance.GetSystem<UiSystem>().ShowObjectiveCanvas(chapter);
        }

        public override void Exit()
        {
            GridGameManager.Instance.GetSystem<UiSystem>().HideObjectiveCanvas();
        }

        public override GameState<NextStateTrigger> Update()
        {
            time += Time.deltaTime;
            if (time >= DELAY)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (time >= DELAY)
                        return NextState;
                }
            }

            return null;
        }
    }
}