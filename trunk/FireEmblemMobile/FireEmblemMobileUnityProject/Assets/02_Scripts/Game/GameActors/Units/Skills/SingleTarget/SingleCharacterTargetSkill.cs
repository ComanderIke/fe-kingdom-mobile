using System;

namespace Game.GameActors.Units.Skills
{
    [Serializable]

    public class SingleCharacterTargetSkill:SingleTargetSkill
    {
        public override int GetDamage(Unit user, bool justToShow)
        {
            return 1;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return true;
        }
    }
}