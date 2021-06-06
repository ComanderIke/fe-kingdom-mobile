using UnityEngine;

namespace Game.WorldMapStuff.Manager
{
    [ExecuteInEditMode]
    public class StartSpawn:MonoBehaviour
    {
        public LocationController location;
        void Update()
        {
            transform.position = location.transform.position;
        }
    }
}