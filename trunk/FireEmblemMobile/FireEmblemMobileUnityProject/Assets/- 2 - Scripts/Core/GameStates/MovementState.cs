using Assets.GameActors.Units;
using Assets.Grid.PathFinding;
using Assets.Mechanics;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core.GameStates
{
    public class MovementState : GameState<NextStateTrigger>
    {
        private readonly int x;
        private readonly int y;
        private bool active;
        private readonly MainScript mainScript;
        private readonly List<Vector2> mousePath;
        public MovementPath Path;
        public int PathCounter;
        private readonly Unit unit;

        public MovementState(Unit c, int x, int y, List<Vector2> path = null)
        {
            mousePath = path;
            this.x = x;
            this.y = y;
            mainScript = MainScript.Instance;
            unit = c;
            PathCounter = 0;
        }

        public override void Enter()
        {
            if (unit.GridPosition.X == x && unit.GridPosition.Y == y) //already on Destination
            {
                FinishMovement();
                return;
            }

            active = true;
            UnitActionSystem.OnStartMovingUnit();
            if (mousePath == null || mousePath.Count == 0)
            {
                Path = mainScript.GetSystem<MoveSystem>().GetPath(unit.GridPosition.X, unit.GridPosition.Y, x, y,
                    unit.Player.Id, false, new List<int>());
                Path?.Reverse();
            }
            else
            {
                Path = new MovementPath();
                for (var i = 0; i < mousePath.Count; i++) Path.PrependStep(mousePath[i].x, mousePath[i].y);
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
            float x = unit.GameTransform.GameObject.transform.localPosition.x;
            float y = unit.GameTransform.GameObject.transform.localPosition.y;
            float z = unit.GameTransform.GameObject.transform.localPosition.z;
            float tx = Path.GetStep(PathCounter).GetX();
            float ty = Path.GetStep(PathCounter).GetY();
            var walkSpeed = 5f;
            float value = walkSpeed * Time.deltaTime;
            var offset = 0.05f;
            x = x + offset > tx && x - offset < tx || x == tx ? tx : x + (x < tx ? value : -value);
            y = y + offset > ty && y - offset < ty || y == ty ? ty : y + (y < ty ? value : -value);
            unit.GameTransform.GameObject.transform.localPosition = new Vector3(x, y, z);

            if (x == tx && y == ty) PathCounter++;

            if (PathCounter >= Path.GetLength())
            {
                active = false;
                FinishMovement();
            }
        }

        public override void Exit()
        {
            unit.SetPosition(x, y);
            UnitActionSystem.OnStopMovingUnit();
        }

        private void FinishMovement()
        {
            unit.UnitTurnState.HasMoved = true;
            if (unit.Player.IsPlayerControlled)
                mainScript.GetSystem<UnitActionSystem>().ActiveCharWait();
            UnitActionSystem.OnCommandFinished();
        }
    }
}