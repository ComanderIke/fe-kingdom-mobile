using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using UnityEngine;

namespace Game.AI
{
    public class CampaignConditionManager : ConditionManager
    {
        private Campaign campaign;
        private FactionManager factionManager;
        public CampaignConditionManager(Campaign campaign, FactionManager factionManager)
        {
            this.campaign = campaign;
            this.factionManager = factionManager;
        }

        public override bool CheckWin()
        {
            foreach (var cond in campaign.victoryDefeatConditions)
            {
                if(cond.victory)
                    if (!cond.CheckCondition(factionManager))
                    {
                        return false;
                    }
            }

            return true;
        }

        public override bool CheckLose()
        {
            foreach (var cond in campaign.victoryDefeatConditions)
            {
                if(!cond.victory)
                    if (cond.CheckCondition(factionManager))
                    {
                        Debug.Log("Campaign Lose: ");
                        return true;
                    }
            }
            
            return false;

        }
    }
}