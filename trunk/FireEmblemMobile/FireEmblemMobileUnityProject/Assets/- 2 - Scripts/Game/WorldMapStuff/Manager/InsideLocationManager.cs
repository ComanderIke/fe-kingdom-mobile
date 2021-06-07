using Game.WorldMapStuff.Controller;
using Menu;
using UnityEngine;

namespace Game.WorldMapStuff.Manager
{
    public class InsideLocationManager : MonoBehaviour
    {
        // Start is called before the first frame update
 

        public void BackClicked()
        {

            GameSceneController.Instance.LoadWorldMapFromInside();
        }
    }
}
