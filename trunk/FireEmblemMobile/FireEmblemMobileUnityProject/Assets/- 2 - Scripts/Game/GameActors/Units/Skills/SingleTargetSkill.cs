using System;

namespace Assets.GameActors.Units.Skills
{
    [Serializable]
    public abstract class SingleTargetSkill : Skill
    {
        public virtual void Activate(Unit user, Unit target)
        {

        }

        public virtual void Effect(Unit user, Unit target)
        {
        }

        public override bool CanTargetCharacters()
        {
            return true;
        }
    }
}