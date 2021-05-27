using Game.Grid;
using UnityEngine;

namespace Game.WorldMapStuff.UI
{
    [CreateAssetMenu(fileName = "campaign", menuName = "GameData/Campaign")]
    public class Campaign:ScriptableObject
    {
        public CampaignVictoryDefeatCondition[] victoryDefeatConditions;
        public string name;
    }
}