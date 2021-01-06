using UnityEngine;

namespace Game.GameInput
{
    public class RaycastManager
    {
        private RaycastHit2D hit;
        private bool connected;
        private Camera camera = Camera.main;
        public Vector2 GetMousePositionOnGrid()
        {
            Vector3 point = camera.ScreenToWorldPoint(Input.mousePosition);
            hit = new RaycastHit2D();
            hit= Physics2D.GetRayIntersection(new Ray(point, Vector3.forward),Mathf.Infinity, LayerMask.GetMask("Grid","Characters"));//TODO 10 Grid Layer Mask
            int x = (int)Mathf.Floor(hit.point.x - Map.GridSystem.GRID_X_OFFSET);
            int y = (int)Mathf.Floor(hit.point.y);
            if (hit.collider != null)
                connected = true;
            else
                connected = false;
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
    }
}
