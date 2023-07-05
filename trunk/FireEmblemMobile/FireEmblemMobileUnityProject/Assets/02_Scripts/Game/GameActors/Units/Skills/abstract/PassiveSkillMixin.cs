using LostGrace;

namespace Game.GameActors.Units.Skills
{
    public abstract class PassiveSkillMixin:SkillMixin
    {
        
        private EffectDescription[] effectDescriptionsPerLevel;
        
        public PassiveSkillMixin(EffectDescription[] effectDescription):base()
        {
            this.effectDescriptionsPerLevel = effectDescription;
        }

        public EffectDescription GetEffectDescription(int level)
        {
            return effectDescriptionsPerLevel[level];
        }
    }
}