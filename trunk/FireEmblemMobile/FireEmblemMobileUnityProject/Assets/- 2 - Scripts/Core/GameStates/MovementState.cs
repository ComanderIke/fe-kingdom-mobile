using System;
using Assets.GameActors.Units;
using Assets.Grid.PathFinding;
using Assets.Mechanics;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core.GameStates
{
    public class MovementState : GameState<NextStateTrigger>
    {
        private int x;
        private int y;
        private bool active;
        private readonly GridGameManager gridGameManager;
        private List<Vector2> mousePath;
        public MovementPath Path;
        public int PathCounter;
        private Unit unit;

        public MovementState()
        {
            gridGameManager = GridGameManager.Instance;
            PathCounter = 0;
        }

        public void StartMovement(Unit c, int x, int y, List<Vector2> path = null)
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
            //Debug.Log("EnterMoveState");
            if (unit.GridPosition.X == x && unit.GridPosition.Y == y) //already on Destination
            {
                FinishMovement();
                return;
            }

            active = true;
            UnitActionSystem.OnStartMovingUnit();
            if (mousePath == null || mousePath.Count == 0)
            {
                Path = gridGameManager.GetSystem<MoveSystem>().GetPath(unit.GridPosition.X, unit.GridPosition.Y, x, y,
                    unit.Faction.Id, false, new List<int>());
                Path?.Reverse();
            }
            else
            {
                Path = new MovementPath();
                for (var i = 0; i < mousePath.Count; i++) Path.PrependStep(mousePath[i].x, mousePath[i].y);
            }
            //Debug.Log("EndEnterMoveState" + Path.GetLength());
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
           
            float x = unit.GameTransform.GameObject.transform.localPosition.x;
            float y = unit.GameTransform.GameObject.transform.localPosition.y;
            float z = unit.GameTransform.GameObject.transform.localPosition.z;
            //Debug.Log("WTF"+Path.GetLength()+" "+PathCounter);
            float tx = Path.GetStep(PathCounter).GetX();
            float ty = Path.GetStep(PathCounter).GetY();
            //Debug.Log("Moving to x: " + tx + " y: " + ty+ " "+x+" "+y);
            var walkSpeed = 5f;
            float value = walkSpeed * Time.deltaTime;
            float tolerance = 0.05f;
            x = Math.Abs(x - tx) < value ? tx : x + (x < tx ? value : -value);
            y = Math.Abs(y - ty) < value ? ty : y + (y < ty ? value : -value);
            unit.GameTransform.GameObject.transform.localPosition = new Vector3(x, y, z);

           
            if (Math.Abs(x - tx) < value && Math.Abs(y - ty) < value) PathCounter++;

            if (PathCounter >= Path.GetLength())
            {
                active = false;
                FinishMovement();
            }
        }

        public override void Exit()
        {
            unit.SetPosition(x, y);
            Debug.Log("Unit moved to x: "+x+" y: "+y);
            UnitActionSystem.OnStopMovingUnit();
        }

        private void FinishMovement()
        {
            Debug.Log("Finished Movement!");
            unit.UnitTurnState.HasMoved = true;
            if (unit.Faction.IsPlayerControlled)
                gridGameManager.GetSystem<UnitActionSystem>().ActiveCharWait();
            GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.FinishedMovement);
            UnitActionSystem.OnCommandFinished();
           
        }
    }
}