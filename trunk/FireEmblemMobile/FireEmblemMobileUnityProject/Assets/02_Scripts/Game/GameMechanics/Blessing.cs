using System;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;

namespace LostGrace
{
    public class Blessing
    {
        private Skill skill;
        private string name;

        private Unit blessedUnit;
        private int tier;
        public const int MINIMUM_DURATION = 3;
        public const float FAITH_DURATION_INCREASE = 0.5f;
        private int currentDuration = 0;

        public Blessing(Skill skill, string name, string description, int tier)
        {
            this.skill = skill;
            this.name = name;
            //this.description = description;
            this.tier = tier;
        }

        public Skill Skill => skill;

        public string Name => name;

       // public string Description => description;

        public int Tier => tier;

        public void BlessUnit(Unit blessedUnit)
        {
            currentDuration = GetDuration(blessedUnit.Stats.Attributes.FAITH);
            this.blessedUnit = blessedUnit;
        }

        public void DecreaseDuration()
        {
            currentDuration--;
            if (currentDuration <= 0)
            {
                blessedUnit.RemoveBlessing();
                blessedUnit = null;
            }
        }
        public int GetDuration(int faith)
        {
            return MINIMUM_DURATION + (int)Math.Floor(faith * FAITH_DURATION_INCREASE);
        }

        public string GetDurationDescription(int faith)
        {
            return "Following effect will be active during the next " + GetDuration(faith) + " encounters:";
        }
        public string GetShortDurationDescription()
        {
            return "Active next " + GetDuration(blessedUnit.Stats.Attributes.FAITH) + " encounters:";
        }
    }
}