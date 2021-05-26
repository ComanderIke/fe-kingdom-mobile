
using Pathfinding;
using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    public class SpriteDirectionController : MonoBehaviour
    {
        public AIPath path;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (path.desiredVelocity.x >= 0.01f)
                transform.localScale = new Vector3(1, 1, 1);
            else if(path.desiredVelocity.x <= -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
