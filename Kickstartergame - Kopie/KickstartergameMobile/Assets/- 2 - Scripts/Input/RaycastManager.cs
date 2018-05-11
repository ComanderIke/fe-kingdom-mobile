using UnityEngine;


namespace Assets.Scripts.Input
{
    public class RaycastManager
    {
        RaycastHit hit;
        public RaycastManager()
        {
            
        }

        public Vector2 GetMousePositionOnGrid()
        {
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            hit = new RaycastHit();
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            int x = (int)Mathf.Floor(hit.point.x - GridSystem.GRID_X_OFFSET);
            int y = (int)Mathf.Floor(hit.point.y);
            return new Vector2(x, y);
        }
        public RaycastHit GetLatestHit()
        {
            return hit;
        }
    }
}
