using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace.AchievementSystem
{
    public class AchievementEventsManager
    {
        private AchievementManager achievementManager;

        public AchievementEventsManager(AchievementManager manager)
        {
            achievementManager = manager;
        }
        public void InitEvents()
        {
            Unit.UnitDied += UnitDied;
            EncounterPosition.OnNodeVisited += NodeVisited;
        }

        public void Cleanup()
        {
            Unit.UnitDied -= UnitDied;
            EncounterPosition.OnNodeVisited -= NodeVisited;
        }

        void NodeVisited(EncounterNode node)
        {
            switch (node)
            {
                case ChurchEncounterNode: achievementManager.AddAchievementProgress("churchVisited", 1); break;
                case InnEncounterNode: achievementManager.AddAchievementProgress("innVisited", 1); break;
                case SmithyEncounterNode: achievementManager.AddAchievementProgress("smithyVisited", 1); break;
                case MerchantEncounterNode: achievementManager.AddAchievementProgress("merchantVisited", 1); break;
                
            }
        }

        void UnitDied(Unit deadUnit)
        {
            if (deadUnit.Faction != null && deadUnit.Faction.Id == FactionId.ENEMY)
            {
                achievementManager.AddAchievementProgress("cleansing1", 1);
            }
        }
    }
}