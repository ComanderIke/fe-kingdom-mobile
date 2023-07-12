using System;
using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Buffs
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Active/SingleTarget/Buff", fileName = "SingleTargetBuffMixin")]
    public class SingleTargetBuffSkillMixin : SingleTargetMixin
    {
        [SerializeField]
        public Buff appliedBuff;
        [SerializeField]
        public Debuff appliedDebuff;

        public override void Activate(Unit user, Unit target)
        {
            GameObject.Instantiate(AnimationObject,target.GameTransformManager.Transform.position, Quaternion.identity, null);
            if(appliedBuff!=null)
                target.StatusEffectManager.AddBuff(appliedBuff);
            if(appliedDebuff!=null)
                target.StatusEffectManager.AddDebuff(appliedDebuff);
        }
    }
}