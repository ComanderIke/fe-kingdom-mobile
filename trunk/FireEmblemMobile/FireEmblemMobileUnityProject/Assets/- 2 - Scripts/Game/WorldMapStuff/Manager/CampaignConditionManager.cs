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
        public CampaignConditionManager(Campaign campaign)
        {
            this.campaign = campaign;
        }

        public override bool CheckWin()
        {
            foreach (var cond in campaign.victoryDefeatConditions)
            {
                if(cond.victory)
                    if (!cond.CheckCondition())
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
                    if (cond.CheckCondition())
                    {
                        Debug.Log("Campaign Lose: ");
                        return true;
                    }
            }
            
            return false;

        }
    }
}