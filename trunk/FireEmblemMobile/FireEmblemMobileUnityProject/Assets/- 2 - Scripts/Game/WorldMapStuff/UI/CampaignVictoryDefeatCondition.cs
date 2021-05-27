using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
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
            if (victory)
            {
                switch (type)
                {
                    case CampaignConditionType.Survive: if(WorldMapGameManager.Instance.GetSystem<TurnSystem>().TurnCount>=5)
                        return true;break;
                    case CampaignConditionType.KillBoss: break;
                }

               
            }
            else
            {
                switch (type)
                {
                    case CampaignConditionType.Survive: foreach (var faction in factionManager.Factions)
                        {
                            var p = (WM_Faction) faction;

                            if (p.IsPlayerControlled && !p.IsAlive())
                            {
                                return true;
                            }
                        }

                        return false;break;
                    case CampaignConditionType.KillBoss: break;
                }
            }
            return false;
        }
        public enum CampaignConditionType{
            Survive,
            KillBoss
        }
    }
}