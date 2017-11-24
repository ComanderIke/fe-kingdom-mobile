using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class GameTransform
    {
        public GameObject GameObject { get; set; }

        public void SetPosition(int x, int y)
        {
            GameObject.transform.localPosition = new Vector3(x, y, 0);
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}
