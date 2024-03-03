using Game.EncounterAreas.Model;
using UnityEngine;

namespace Game.Campaigns
{
    [CreateAssetMenu(fileName = "campaignConfig", menuName = "GameData/CampaignConfig")]
    public class CampaignConfig:ScriptableObject
    {
        public new string name;
        public int campaignId;

        public Sprite sprite;
        public Scenes scene;
    }
}