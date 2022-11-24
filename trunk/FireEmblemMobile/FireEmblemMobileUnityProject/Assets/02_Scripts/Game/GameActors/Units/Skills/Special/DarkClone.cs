using System;

namespace Game.GameActors.Units.Skills.Special
{
    [Serializable]

    public class DarkClone : SelfTargetSkill
    {

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }
        
    }
}