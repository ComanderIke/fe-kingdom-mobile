using UnityEngine;

namespace Assets.Scripts.Characters
{
    public interface DragAble
    {
        Transform GetTransform();
        void StartDrag();
        void EndDrag();
        void NotDragging();
        void Dragging();
    }
}
