using Game.WorldMapStuff.Controller;
using Menu;
using UnityEngine;

namespace Game.WorldMapStuff.Manager
{
    public class InsideLocationManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void BackClicked()
        {
            WorldMapSceneController.Instance.LoadWorldMap();
        }
    }
}
