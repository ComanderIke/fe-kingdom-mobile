using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    public class EnterLocationController : MonoBehaviour
    {
        public WorldMapPosition position;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnMouseDown()
        {
            position.EnterLocationClicked();
        }
    }
}
