using UnityEngine;

namespace GameEngine.Tools
{
    public interface IDragPerformer
    {
        void Drag(Transform transform, Vector3 dragDestination);
        void StartDrag(Transform transform, Vector3 dragDestination, bool opposite=false);
        void EndDrag(Transform transform);
        bool IsDragging { get; }
        bool HasDragStarted();
    }
}