using UnityEngine;

namespace Utility
{
    public class StartPosition : MonoBehaviour {

        void Start () {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        public int GetXOnGrid()
        {
            return (int)transform.localPosition.x;
        }
        public int GetYOnGrid()
        {
            return (int)transform.localPosition.y;
        }

    }
}
