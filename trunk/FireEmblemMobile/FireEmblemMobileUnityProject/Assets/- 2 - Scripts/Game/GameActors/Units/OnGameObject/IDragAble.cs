using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public interface IDragAble
    {
        Transform GetTransform();
        void StartDrag();
        void EndDrag();
        void NotDragging();
        void Dragging(float x, float y);
    }
}