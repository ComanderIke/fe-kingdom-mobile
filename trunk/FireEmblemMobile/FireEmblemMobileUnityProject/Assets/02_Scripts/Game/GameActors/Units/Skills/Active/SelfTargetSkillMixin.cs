﻿using System.Collections;
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
                else if (effect is UnitTargetSkillEffectMixin utsm)
                {
                    utsm.Activate(user, user, skill.Level);
                }
                
            }

            if (user != null)
            {
                var go = Instantiate(AnimationObject);
                go.transform.position = user.GameTransformManager.GetCenterPosition();
            }

            base.PayActivationCost();
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
                else if (effect is UnitTargetSkillEffectMixin utsm)
                {
                    utsm.Deactivate(unit, unit, skill.Level);
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