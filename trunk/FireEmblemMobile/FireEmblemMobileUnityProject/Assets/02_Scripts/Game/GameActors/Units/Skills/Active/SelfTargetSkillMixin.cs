using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Active/SelfTarget", fileName = "SelfTargetSkillMixin")]
    public class SelfTargetSkillMixin : ActiveSkillMixin
    {
        public List<SkillEffectMixin> effectMixins;
        public virtual void Activate(Unit user)
        {
            foreach (var effect in effectMixins)
            {
                if (effect is SelfTargetSkillEffectMixin unitTargetSkillEffectMixin)
                {
                    unitTargetSkillEffectMixin.Activate(user, skill.Level);
                }
                
            }
        }

        // protected SelfTargetSkillMixin( int[] maxUses, int[] hpCost,GameObject animationObject) : base(maxUses,hpCost,animationObject)
        // {
        // }
    }
}