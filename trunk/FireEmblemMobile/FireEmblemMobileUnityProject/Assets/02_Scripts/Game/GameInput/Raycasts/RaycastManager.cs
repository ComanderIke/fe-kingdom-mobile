using Game.Grid;
using UnityEngine;

namespace Game.GameInput.Raycasts
{
    public class RaycastManager
    {
        private RaycastHit2D hit;
        private bool connected;
        private Camera camera = Camera.main;
        public Vector2 GetMousePositionOnGrid()
        {
            var point = camera.ScreenToWorldPoint(Input.mousePosition);
            hit = new RaycastHit2D();
            hit= Physics2D.GetRayIntersection(new Ray(point, Vector3.forward),Mathf.Infinity, LayerMask.GetMask("Grid","Characters"));//TODO 10 Grid Layer Mask
            int x = (int)Mathf.Floor(hit.point.x - GridSystem.GRID_X_OFFSET);
            int y = (int)Mathf.Floor(hit.point.y);
            connected = hit.collider != null;
            return new Vector2(x, y);
        }
        public RaycastHit2D GetLatestHit()
        {
            return hit;
        }
        public bool ConnectedLatestHit()
        {
            return connected;
        }

        public bool ColliderHit(string tag)
        {
            return hit.collider != null && hit.collider.gameObject.CompareTag(tag);
        }

        public T GetLatestHitComponent<T>()
        {
            return hit.collider != null ? hit.collider.gameObject.GetComponent<T>() : default(T);
        }
    }
}
