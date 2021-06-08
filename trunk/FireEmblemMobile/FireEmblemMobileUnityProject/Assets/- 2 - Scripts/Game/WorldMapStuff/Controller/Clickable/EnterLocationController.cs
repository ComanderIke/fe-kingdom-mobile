using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    public class EnterLocationController : MonoBehaviour
    {
        public WorldMapPosition position;

        public void OnMouseDown()
        {
            position.EnterLocationClicked();
        }
    }
}
