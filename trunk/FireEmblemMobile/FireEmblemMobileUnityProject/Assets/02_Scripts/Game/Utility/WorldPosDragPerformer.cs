using GameEngine.Tools;
using UnityEngine;

namespace Game.Utility
{
    public class WorldPosDragPerformer : IDragPerformer
    {
        private float dragSpeed;
        private Vector3 lastPosition;
        private bool startedDrag;
        private Camera camera;
        private Vector3 startPos;
        private bool dragOpposite = false;
        public bool IsDragging { get; private set; }
        public bool HasDragStarted()
        {
            return startedDrag;
        }

        private const float DISTANCE_TILL_DRAG = 0.15f;
        public WorldPosDragPerformer(float dragSpeed, Camera camera)
        {
            this.dragSpeed = dragSpeed;
            this.camera = camera;
        }
        public void Drag(Transform transform, Vector3 dragDestination)
        {
            if (!startedDrag)
                return;
            if (!IsDragging && Vector3.Distance(camera.ScreenToWorldPoint(startPos),  camera.ScreenToWorldPoint(dragDestination)) >= DISTANCE_TILL_DRAG)
            {
                IsDragging = true;
            }

            var delta = camera.ScreenToWorldPoint(dragDestination) -
                    camera.ScreenToWorldPoint(lastPosition); //screen to world must be calculated here otherwise lastPosition has worldPosition of 1 frame before instead of mousePosition of 1 frame before!!!
            lastPosition = dragDestination;
            transform.Translate(-delta.x * dragSpeed*(dragOpposite?-1:1), -delta.y * dragSpeed*(dragOpposite?-1:1), 0, Space.World);

        }

        public void StartDrag(Transform transform, Vector3 dragDestination, bool opposite=false)
        {
            lastPosition = startPos = dragDestination;
            startedDrag = true;
            IsDragging = false;
            dragOpposite = opposite;
        }

        public void EndDrag(Transform transform)
        {
            startedDrag = false;
            IsDragging = false;
            startPos = Vector3.zero;
        }
    }
}