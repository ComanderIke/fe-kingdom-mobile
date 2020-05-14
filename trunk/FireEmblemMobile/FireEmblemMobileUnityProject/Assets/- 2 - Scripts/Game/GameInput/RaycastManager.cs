using UnityEngine;

namespace Assets.GameInput
{
    public class RaycastManager
    {
        private RaycastHit hit;
        private bool connected;
        private Camera camera = Camera.main;
        public Vector2 GetMousePositionOnGrid()
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            hit = new RaycastHit();
            connected = Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Grid","Characters"));//TODO 10 Grid Layer Mask
            int x = (int)Mathf.Floor(hit.point.x - Map.MapSystem.GRID_X_OFFSET);
            int y = (int)Mathf.Floor(hit.point.y);
            return new Vector2(x, y);
        }
        public RaycastHit GetLatestHit()
        {
            return hit;
        }
        public bool ConnectedLatestHit()
        {
            return connected;
        }
    }
}
