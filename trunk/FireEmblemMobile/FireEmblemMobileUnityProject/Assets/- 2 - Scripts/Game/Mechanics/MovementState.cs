using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid;
using Game.Grid.PathFinding;
using Game.Manager;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Mechanics
{
    public class MovementState : GameState<NextStateTrigger>
    {
        public static event Action<IGridActor> OnMovementFinished;
        public static event Action OnEnter;
        private int x;
        private int y;
        private bool active;
        private readonly GridGameManager gridGameManager;
        private List<GridPosition> mousePath;
        public MovementPath Path;
        public int PathCounter;
        private IGridActor unit;

        public MovementState()
        {
            gridGameManager = GridGameManager.Instance;
            PathCounter = 0;
        }

        public void StartMovement(IGridActor c, int x, int y, List<GridPosition> path = null)
        {
            mousePath = path;
            this.x = x;
            this.y = y;
            unit = c;
            GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.MoveUnit);
            PathCounter = 0;
        }

        public override void Enter()
        {
            if (unit.GridPosition.X == x && unit.GridPosition.Y == y) //already on Destination
            {
                FinishMovement();
                return;
            }
            OnEnter?.Invoke();
            active = true;
            GridInputSystem.SetActive(false);
            if (mousePath == null || mousePath.Count == 0)
            {
                Path = gridGameManager.GetSystem<MoveSystem>().GetPath(unit.GridPosition.X, unit.GridPosition.Y, x, y,
                    unit, false, new List<int>());
                Path?.Reverse();
                
            }
            else
            {
                Path = new MovementPath();
                for (var i = 0; i < mousePath.Count; i++) Path.PrependStep(mousePath[i].X, (int)mousePath[i].Y);
            }
            /*remove first Step if it is the same position as the unit is on*/

            if (Path.GetStep(0).GetX() == unit.GridPosition.X&& Path.GetStep(0).GetY() == unit.GridPosition.Y)
            {
                //Debug.Log("Remove: [" + Path.GetStep(0).GetX() + "/" + Path.GetStep(0).GetY() + "]");
                Path.Remove(0);
            }
            //PrintMovementPath();
            FinishMovement();
        }
        private void PrintMovementPath()
        {
            for(int i=0; i < Path.GetLength(); i++)
            {
                Debug.Log("["+Path.GetStep(i).GetX() +"/" +Path.GetStep(i).GetY()+"]");
            }
        }
        public override GameState<NextStateTrigger> Update()
        {
            if (!active)
                return null;
            MoveUnit();
            return NextState;
        }

        private void MoveUnit()
        {
          
            float x = unit.GetTransform().localPosition.x;
            float y = unit.GetTransform().localPosition.y;
            float z = unit.GetTransform().localPosition.z;
            float tx = Path.GetStep(PathCounter).GetX();
            float ty = Path.GetStep(PathCounter).GetY();
            //Debug.Log("Moving to x: " + tx + " y: " + ty+ " "+x+" "+y);
            var walkSpeed = 5f;
            float value = walkSpeed * Time.deltaTime;
            float tolerance = 0.05f;
            x = Math.Abs(x - tx) < value ? tx : x + (x < tx ? value : -value);
            y = Math.Abs(y - ty) < value ? ty : y + (y < ty ? value : -value);
            unit.GetTransform().localPosition = new Vector3(x, y, z);

           
            if (Math.Abs(x - tx) < value && Math.Abs(y - ty) < value) PathCounter++;

            if (PathCounter >= Path.GetLength())
            {
                FinishMovement();
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit MoveState!"+x+" "+y);
            unit.SetPosition(x, y);
            unit.UnitTurnState.Selected = false;
            unit.UnitTurnState.HasMoved = true;
            GridInputSystem.SetActive(true);
            OnMovementFinished?.Invoke(unit);
        }

        private void FinishMovement()
        {
            Debug.Log("Finished Movement!");
            active = false;
            GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.FinishedMovement);
            UnitActionSystem.OnCommandFinished();
           


        }
    }
}