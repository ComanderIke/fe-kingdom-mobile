using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public interface IDragAble
    {
        void StartDrag(Transform transform);
        void EndDrag();
        void NotDragging();
        void Dragging(Transform transform, float x, float y);
    }
}