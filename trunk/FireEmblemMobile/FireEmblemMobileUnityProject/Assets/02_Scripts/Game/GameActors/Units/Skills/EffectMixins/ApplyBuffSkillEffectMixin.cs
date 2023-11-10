using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/ApplyBuff", fileName = "ApplyBuffSkillEffect")]
    public class ApplyBuffSkillEffectMixin : UnitTargetSkillEffectMixin
    {
        public GameObject effect;
        public float[] applyChance;
        public Buff appliedBuff;
        public StatModifier AppliedStatModifier;
        public Debuff appliedDebuff;

       

        

        public override void Activate(Unit target,Unit caster, int level)
        {
            if (effect != null)
                GameObject.Instantiate(effect, target.GameTransformManager.GetCenterPosition(), Quaternion.identity);
            if (appliedBuff != null)
                target.StatusEffectManager.AddBuff(Instantiate(appliedBuff), caster, level);
            if (appliedDebuff != null)
                target.StatusEffectManager.AddDebuff(Instantiate(appliedDebuff));
            if (AppliedStatModifier!= null)
                target.StatusEffectManager.AddStatModifier(Instantiate(AppliedStatModifier));
        }

        public override void Deactivate(Unit target, Unit caster, int skillLevel)
        {
            if (target == null)
                return;
            if (appliedBuff != null)
                target.StatusEffectManager.RemoveBuff(Instantiate(appliedBuff));
            if (appliedDebuff != null)
                target.StatusEffectManager.RemoveDebuff(Instantiate(appliedDebuff));
            if (AppliedStatModifier!= null)
                target.StatusEffectManager.RemoveStatModifier(Instantiate(AppliedStatModifier));
        }

        public override List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
            var buff=appliedBuff==null?null:appliedBuff.GetEffectDescription(level);
            var debuff=appliedDebuff==null?null:appliedDebuff.GetEffectDescription(level);
            var statModifier=AppliedStatModifier==null?null:AppliedStatModifier.GetEffectDescription(level);
         
            if(buff!=null)
             list.Add(buff);
            if(debuff!=null)
                list.Add(debuff);
            if(statModifier!=null)
                list.AddRange(statModifier);
       
            return list;
        }

       
    }
}