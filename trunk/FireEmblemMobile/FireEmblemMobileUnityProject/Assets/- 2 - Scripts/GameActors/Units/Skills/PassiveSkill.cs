using System;

namespace Assets.GameActors.Units.Skills
{
    [Serializable]
    public class PassiveSkill : Skill
    {
        public override bool CanTargetCharacters()
        {
            return false;
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }
    }
}