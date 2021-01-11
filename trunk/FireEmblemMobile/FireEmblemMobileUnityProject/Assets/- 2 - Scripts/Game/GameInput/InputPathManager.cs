using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Grid.PathFinding;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameInput
{
    public class InputPathManager
    {
        public delegate void MovementPathUpdatedEvent(List<Vector2> mousePath, int startX, int startY);
        public static event MovementPathUpdatedEvent OnMovementPathUpdated;
        
        public List<Vector2> MovementPath;
        private readonly List<Vector2> dragPath;
        private IPathProvider pathProvider;

        public InputPathManager()
        {
            MovementPath = new List<Vector2>();
            dragPath = new List<Vector2>();
            pathProvider = GridGameManager.Instance.GetSystem<MoveSystem>();
        }

        public void Reset()
        {
            dragPath.Clear();
            MovementPath.Clear();
        }
        public void CalculateMousePathToPosition(IGridActor character, int x, int y)
        {
            Reset();
            var p = pathProvider.GetPath(character.GridPosition.X,
                character.GridPosition.Y, x, y, character, false, character.AttackRanges);
            if (p != null)
                for (int i = p.GetLength() - 2; i >= 0; i--)
                    dragPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            MovementPath = new List<Vector2>(dragPath);
            UpdatedMovementPath(character);
        }

        public void CalculatePathToPosition(IGridActor character, Vector2 position)
        {
            Reset();
            var p = pathProvider.GetPath(character.GridPosition.X,
                character.GridPosition.Y, (int) position.x, (int) position.y, character, true,
                character.AttackRanges);
            MovementPath = new List<Vector2>();
            p.Reverse();
            for (int i = 1; i < p.GetLength(); i++)
            {
                //Debug.Log(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
                MovementPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            }
            
            UpdatedMovementPath(character);
        }

        public void UpdatedMovementPath(IGridActor character)
        {
            int startX = character.GridPosition.X;
            int startY = character.GridPosition.Y;
            OnMovementPathUpdated?.Invoke(MovementPath, startX, startY);
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
            return MovementPath.Count >= 1;
        }

        public Vector2Int GetLastMovementPathPosition()
        {
            return new Vector2Int((int)MovementPath[MovementPath.Count - 1].x, (int) MovementPath[MovementPath.Count - 1].y);
        }

        private void CreateNewMovementPath(IGridActor gridActor, int x, int y)
        {
            dragPath.Clear();
            var p = pathProvider.GetPath(gridActor.GridPosition.X,
                gridActor.GridPosition.Y, x, y, gridActor, false, gridActor.AttackRanges);
            if (p != null)
                for (int i = p.GetLength() - 2; i >= 0; i--)
                    dragPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
        }

        public void AddToPath(int x, int y, IGridActor gridActor)
        {
            bool contains = dragPath.Contains(new Vector2(x, y));
           
            dragPath.Add(new Vector2(x, y));
            
            //if (dragPath.Count > gridActor.MovementRage || contains || IsLastActiveFieldAdjacent(x,y,gridActor))
            if (dragPath.Count > gridActor.MovementRage || contains)
            {
                CreateNewMovementPath(gridActor, x , y);
              
            }
            MovementPath = new List<Vector2>(dragPath);
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