using Game.EncounterAreas.Encounters.Battle;
using Game.Manager;
using UnityEngine;

namespace Game.States.Mechanics
{
    public class ChapterConditionManager:ConditionManager
    {
        private BattleMap chapter;
        private FactionManager factionManager;
        public ChapterConditionManager(BattleMap chapter, FactionManager factionManager)
        {
            this.chapter = chapter;
            this.factionManager = factionManager;
        }
        public override bool CheckWin()
        {
        
            foreach (var cond in chapter.victoryDefeatConditions)
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

            foreach (var cond in chapter.victoryDefeatConditions)
            {
                if(!cond.victory)
                    if (cond.CheckCondition(factionManager))
                    {
                        Debug.Log("CheckLose: true "+cond.description);
                        return true;
                    }
            }

            return false;
            
        }
    }
}