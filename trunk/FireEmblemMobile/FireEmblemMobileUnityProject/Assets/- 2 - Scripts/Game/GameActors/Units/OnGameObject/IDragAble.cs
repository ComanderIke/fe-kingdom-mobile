using UnityEngine;

namespace Assets.GameActors.Units.OnGameObject
{
    public interface IDragAble
    {
        Transform GetTransform();
        void StartDrag();
        void EndDrag();
        void NotDragging();
        void Dragging();
    }
}