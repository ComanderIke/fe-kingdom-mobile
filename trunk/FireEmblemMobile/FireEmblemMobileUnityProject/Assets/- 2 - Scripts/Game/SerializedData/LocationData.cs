using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;

namespace Game.Systems
{
    public class LocationData
    {
        public string name;
        public WM_Actor actor;
        public WorldMapPosition worldMapPosition;

        public LocationData(LocationController controller)
        {
            name = controller.name;
            actor = controller.Actor;
            worldMapPosition = controller.worldMapPosition;
        }
        
    }
}