using UnityEngine;

namespace Game.Map
{
    public class GridCursorRenderer : MonoBehaviour
    {
        public Canvas canvas;
        public void Show(Vector2 position)
        {
            //Debug.Log("SHOW");
            gameObject.transform.localPosition = new Vector3(position.x +0.5f, position.y+0.5f,gameObject.transform.localPosition.z);
            gameObject.SetActive(true);
            
        }

        public void ShowTileInfo()
        {
            canvas.enabled = true;
        }
        public void HideTileInfo()
        {
            canvas.enabled = false;
        }
        public void Hide()
        {
            //Debug.Log("Hide");
            gameObject.SetActive(false);
        }
    }
}