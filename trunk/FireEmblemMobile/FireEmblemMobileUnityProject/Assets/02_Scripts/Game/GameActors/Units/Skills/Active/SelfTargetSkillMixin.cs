using System.Collections;
using System.Collections.Generic;
using LostGrace;
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
            var go=Instantiate(AnimationObject);
            go.transform.position = user.GameTransformManager.GetCenterPosition();
            base.Activate();
        }

        // protected SelfTargetSkillMixin( int[] maxUses, int[] hpCost,GameObject animationObject) : base(maxUses,hpCost,animationObject)
        // {
        // }
        public void Deactivate(Unit unit)
        {
            foreach (var effect in effectMixins)
            {
                if (effect is SelfTargetSkillEffectMixin unitTargetSkillEffectMixin)
                {
                    unitTargetSkillEffectMixin.Deactivate(unit, skill.Level);
                }
                
            }
        }

        public List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            foreach (var skillEffect in effectMixins)
            {
                list.AddRange(skillEffect.GetEffectDescription(unit,level));
            }
            return list;
        }
    }
}