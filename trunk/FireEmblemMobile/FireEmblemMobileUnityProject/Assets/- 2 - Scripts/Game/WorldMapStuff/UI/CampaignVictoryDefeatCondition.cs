using Game.Manager;
using UnityEngine;

namespace Game.WorldMapStuff.UI
{
    [CreateAssetMenu(fileName = "campaignCondition", menuName = "GameData/CampaignCondition")]
    public class CampaignVictoryDefeatCondition:ScriptableObject
    {
        public string description;
        public bool victory;
        public CampaignConditionType type;
        
        public bool CheckCondition(FactionManager factionManager)
        {
            
            switch (type)
            {
                case CampaignConditionType.Survive: break;
                case CampaignConditionType.KillBoss: break;
            }

            return false;
        }
        public enum CampaignConditionType{
            Survive,
            KillBoss
        }
    }
}