using UnityEngine;

namespace GameEngine.Tools
{
    public class WorldPosDragPerformer : IDragPerformer
    {
        private float dragSpeed;
        private Vector3 lastPosition;
        private bool startedDrag;
        private Camera camera;

        public WorldPosDragPerformer(float dragSpeed, Camera camera)
        {
            this.dragSpeed = dragSpeed;
            this.camera = camera;
        }
        public void Drag(Transform transform, Vector3 dragDestination)
        {
           
            if (!startedDrag)
                return;
            var delta = camera.ScreenToWorldPoint(dragDestination) - camera.ScreenToWorldPoint(lastPosition);//screen to world must be calculated here otherwise lastPosition has worldPosition of 1 frame before instead of mousePosition of 1 frame before!!!
            lastPosition =dragDestination;
            transform.Translate(-delta.x  * dragSpeed, -delta.y  * dragSpeed, 0, Space.World);
            
        }

        public void StartDrag(Transform transform, Vector3 dragDestination)
        {
            lastPosition =dragDestination;
            startedDrag = true;
        }

        public void EndDrag(Transform transform)
        {
            startedDrag = false;
        }
    }
}