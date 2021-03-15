using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Units;
using Game.Grid;
using Game.GUI;
using Game.Manager;
using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;

namespace Game.States
{
    public class UnitPlacementState : GameState<NextStateTrigger>, IDependecyInjection
    {
        private const float DELAY = 1.0f;
        private float time = 0;
        public List<Unit> units;
        public IUnitPlacementUI UnitPlacementUI;
        
        public override void Enter()
        {
          //  Debug.Log("UnitPlacement"+units.Count());
            NextState = GameStateManager.PlayerPhaseState;
            UnitPlacementUI.Show(units);
        }

        public void SetUnits(List<Unit> units)
        {
            this.units = units;
        }
        public override void Exit()
        {
            UnitPlacementUI.Hide();
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