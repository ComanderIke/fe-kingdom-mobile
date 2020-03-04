namespace Assets.GameActors.Units.Skills
{
    public abstract class SelfTargetSkill : Skill
    {
        public override bool CanTargetCharacters()
        {
            return false;
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public virtual void Activate(Unit user)
        {

        }
    }
}