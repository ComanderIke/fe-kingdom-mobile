using UnityEngine;

namespace Game.GameInput
{
    public class CursorPosition
    {
        public Vector2 Position;
        public bool IsFree;

        public CursorPosition(Vector2 position, bool isFree)
        {
            Position = position;
            IsFree = isFree;

        }
    }
}