using UnityEngine;

namespace Game.GameInput
{
    public class CursorPosition
    {
        public CursorPosition PreviousCursorPosition;
        public Vector2 Position;

        public CursorPosition(Vector2 position, CursorPosition previousCursorPosition)
        {
            Position = position;
            PreviousCursorPosition = previousCursorPosition;
        }
    }
}