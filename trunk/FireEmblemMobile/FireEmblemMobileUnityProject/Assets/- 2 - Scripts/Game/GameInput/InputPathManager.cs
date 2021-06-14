using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Graphics;
using Game.Grid.GridPathFinding;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameInput
{
    public class InputPathManager
    {
       // public delegate void MovementPathUpdatedEvent(List<Vector2Int> mousePath, int startX, int startY);
       // public static event MovementPathUpdatedEvent OnMovementPathUpdated;
        
        public List<Vector2Int> MovementPath;
        private readonly List<Vector2Int> dragPath;
        private IPathFinder pathProvider;

        private IMovePathVisual movePathVisual;
        //public static event MovementPathUpdatedEvent OnReset;

        public InputPathManager(IPathFinder pathProvider, IMovePathVisual movePathVisual)
        {
            MovementPath = new List<Vector2Int>();
            dragPath = new List<Vector2Int>();
            this.pathProvider = pathProvider;
            this.movePathVisual = movePathVisual;
        }


        public void Reset()
        {
            dragPath.Clear();
            MovementPath.Clear();
            UpdatedMovementPath(-1, -1);
        }
        public void CalculateMousePathToPosition(IGridActor character, int x, int y)
        {
            Reset();
            var p = pathProvider.FindPath(character.GridComponent.GridPosition.X,
                character.GridComponent.GridPosition.Y, x, y, character, false, character.AttackRanges);
            if (p != null)
                for (int i = p.GetLength() - 2; i >= 0; i--)
                    dragPath.Add(new Vector2Int(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            MovementPath = new List<Vector2Int>(dragPath);
            UpdatedMovementPath(character.GridComponent.GridPosition.X, character.GridComponent.GridPosition.Y);
        }

        public void CalculatePathToPosition(IGridActor character, Vector2 position)
        {
            Reset();
            var p = pathProvider.FindPath(character.GridComponent.GridPosition.X,
                character.GridComponent.GridPosition.Y, (int) position.x, (int) position.y, character, true,
                character.AttackRanges);
            MovementPath = new List<Vector2Int>();
            p.Reverse();
            for (int i = 1; i < p.GetLength(); i++)
            {
                //Debug.Log(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
                MovementPath.Add(new Vector2Int(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            }
            
            UpdatedMovementPath(character.GridComponent.GridPosition.X, character.GridComponent.GridPosition.Y);
        }

        public void UpdatedMovementPath(int startX, int startY)
        {
            movePathVisual.DrawMovementPath(MovementPath, startX, startY);
           // OnMovementPathUpdated?.Invoke(MovementPath, startX, startY);
        }
        private bool HasMovementPath()
        {
            return MovementPath != null && MovementPath.Count > 0;
        }

        private void PrintMovementPath()
        {
            Debug.Log("Movement Path: ");
            foreach (var t in MovementPath)
            {
                Debug.Log(t);
            }
        }

        public bool IsMovementPathEmpty()
        {
            return MovementPath.Count == 0;
        }

        public Vector2Int GetLastMovementPathPosition()
        {
            return new Vector2Int((int)MovementPath[MovementPath.Count - 1].x, (int) MovementPath[MovementPath.Count - 1].y);
        }

        private void CreateNewMovementPath(IGridActor gridActor, int x, int y)
        {
            dragPath.Clear();
         //   Debug.Log(gridActor);
         //   Debug.Log(pathProvider);
            var p = pathProvider.FindPath(gridActor.GridComponent.GridPosition.X,
                gridActor.GridComponent.GridPosition.Y, x, y, gridActor, false, gridActor.AttackRanges);
            if (p != null)
                for (int i = p.GetLength() - 2; i >= 0; i--)
                    dragPath.Add(new Vector2Int(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
        }

        private bool IsPositionAdjacent(int x, int y, IGridActor gridActor)
        {
            if (dragPath.Count > 0)
            {
                return Math.Abs(dragPath[dragPath.Count - 1].x - x) + Math.Abs(dragPath[dragPath.Count - 1].y - y) == 1;
            }
            return Math.Abs(gridActor.GridComponent.GridPosition.X - x) + Math.Abs(gridActor.GridComponent.GridPosition.Y - y) == 1;

        }
        public void AddToPath(int x, int y, IGridActor gridActor)
        {
            bool contains = dragPath.Contains(new Vector2Int(x, y));
            
            
           
           
            //if (dragPath.Count > gridActor.MovementRage || contains || IsLastActiveFieldAdjacent(x,y,gridActor))
            if (dragPath.Count > gridActor.MovementRange || contains||!IsPositionAdjacent(x,y, gridActor))
            {
              //  Debug.Log("Create New Path!");
                CreateNewMovementPath(gridActor, x , y);
            }
            else
            {
                dragPath.Add(new Vector2Int(x, y));
            }
            MovementPath = new List<Vector2Int>(dragPath);
            UpdatedMovementPath(gridActor.GridComponent.GridPosition.X, gridActor.GridComponent.GridPosition.Y);
        }

        public bool HasValidMovementPath(int range)
        {
            return MovementPath != null && MovementPath.Count <= range;
        }
        // private bool IsFieldAdjacent(float x, float y, float x2, float y2)
        // {
        //     return Mathf.Abs(x - x2) + Mathf.Abs(y - y2) > 1;
        // }

        // private bool FunctionWhut(int x, int y)
        // {
        //     return dragPath.Count >= 2 &&
        //            IsFieldAdjacent(dragPath[dragPath.Count - 2].x, dragPath[dragPath.Count - 2].y, x, y);
        // }
        //
        // private bool FunctionWhut2(int x, int y, IGridActor gridActor)
        // {
        //     return dragPath.Count == 1 && IsFieldAdjacent(gridActor.GridPosition.X, gridActor.GridPosition.Y, x, y);
        // }
        // private bool IsLastActiveFieldAdjacent(int x, int y, IGridActor gridActor)
        // {
        //     if(FunctionWhut(x,y))
        //         return true;
        //     if (FunctionWhut2(x,y, gridActor))
        //         return true;
        //     return IsFieldAdjacent(lastDragPosX, lastDragPosY, x, y);
        // }
    }
}