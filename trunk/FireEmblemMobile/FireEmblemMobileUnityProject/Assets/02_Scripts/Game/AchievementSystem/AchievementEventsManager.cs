using Game.GameActors.Units;
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
        }

        public void Cleanup()
        {
            Unit.UnitDied -= UnitDied;
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