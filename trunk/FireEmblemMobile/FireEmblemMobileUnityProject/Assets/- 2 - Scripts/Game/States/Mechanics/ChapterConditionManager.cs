using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameInput;
using Game.Grid;
using Game.Manager;

namespace Game.Mechanics
{
    public class ChapterConditionManager:ConditionManager
    {
        private Chapter chapter;
        private FactionManager factionManager;
        public ChapterConditionManager(Chapter chapter, FactionManager factionManager)
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
                    if (!cond.CheckCondition(factionManager))
                    {
                        return false;
                    }
            }

            return true;
            
        }
    }
}