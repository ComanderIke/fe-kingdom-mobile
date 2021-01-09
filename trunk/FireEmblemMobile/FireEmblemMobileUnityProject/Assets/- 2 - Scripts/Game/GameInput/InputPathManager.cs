using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameInput
{
    public class InputPathManager
    {
        public delegate void OnMovementPathUpdatedEvent(List<Vector2> mousePath, int startX, int startY);
        public static event OnMovementPathUpdatedEvent OnMovementPathUpdated;
        
        public List<Vector2> MovementPath = new List<Vector2>();                // stores the path a unit is moving along the grid.
        
        public void CalculateMousePathToPosition(Unit character, int x, int y)
        {
            ResetDrag();
            var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                character.GridPosition.Y, x, y, character, false, character.Stats.AttackRanges);
            if (p != null)
                for (int i = p.GetLength() - 2; i >= 0; i--)
                    dragPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            MovementPath = new List<Vector2>(dragPath);
            UpdatedMovementPath();
        }

        public void CalculatePathToPosition(Unit character, Vector2 position)
        {
            ResetDrag();
            var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                character.GridPosition.Y, (int) position.x, (int) position.y, character, true,
                character.Stats.AttackRanges);
            MovementPath = new List<Vector2>();
            p.Reverse();
            for (int i = 1; i < p.GetLength(); i++)
            {
                //Debug.Log(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
                MovementPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            }
            
            UpdatedMovementPath();
        }

        public void UpdatedMovementPath()
        {
            int startX = SelectedCharacter.GridPosition.X;
            int startY = SelectedCharacter.GridPosition.Y;
            OnMovementPathUpdated?.Invoke(MovementPath, startX, startY);
        }
        private bool HasMovementPath()
        {
            return MovementPath != null && MovementPath.Count > 0;
        }

        private void PrintMovementPath()
        {
            Debug.Log("Movement Path: ");
            for (int i = 0; i < MovementPath.Count; i++)
            {
                Debug.Log(MovementPath[i]);
            }
        }
    }
}