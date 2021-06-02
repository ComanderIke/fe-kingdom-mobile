using System.Collections.Generic;
using Game.GameActors.Players;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;

namespace Game.Systems
{
    public class SaveData
    {
        public PlayerData player;
        public CampaignData campaignData;
    }

    public class CampaignData
    {
        public int campaignId;
        public List<LocationData>locationData;
        public int turnCount;

    }

    public class LocationData
    {
        public string name;
        public WM_Actor actor;
        public WorldMapPosition worldMapPosition;
        
    }
}