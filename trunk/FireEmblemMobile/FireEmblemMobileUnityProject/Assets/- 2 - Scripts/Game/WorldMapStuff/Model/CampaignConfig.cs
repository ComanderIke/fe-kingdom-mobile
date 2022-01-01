using System.Collections.Generic;
using Game.Grid;
using Game.Systems;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.UI
{
    [CreateAssetMenu(fileName = "campaignConfig", menuName = "GameData/CampaignConfig")]
    public class CampaignConfig:ScriptableObject
    {
        public CampaignVictoryDefeatCondition[] victoryDefeatConditions;
        public new string name;
        public int campaignId;

        public Sprite sprite;
        public Scenes scene;
    }
}