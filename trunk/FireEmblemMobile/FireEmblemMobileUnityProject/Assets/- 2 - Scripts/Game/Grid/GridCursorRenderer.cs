using UnityEngine;

namespace Game.Map
{
    public class GridCursorRenderer : MonoBehaviour
    {
        public void Show(Vector2 position)
        {
            //Debug.Log("SHOW");
            gameObject.transform.localPosition = new Vector3(position.x +0.5f, position.y+0.5f,gameObject.transform.localPosition.z);
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            //Debug.Log("Hide");
            gameObject.SetActive(false);
        }
    }
}