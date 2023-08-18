using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/ApplyBuff", fileName = "ApplyBuffSkillEffect")]
    public class ApplyBuffSkillEffectMixin : UnitTargetSkillEffectMixin
    {
        public float[] applyChance;
        public Buff appliedBuff;
        public StatModifier AppliedStatModifier;
        public Debuff appliedDebuff;

       

        

        public override void Activate(Unit target,Unit caster, int level)
        {
            if (appliedBuff != null)
                target.StatusEffectManager.AddBuff(Instantiate(appliedBuff));
            if (appliedDebuff != null)
                target.StatusEffectManager.AddDebuff(Instantiate(appliedDebuff));
            if (AppliedStatModifier!= null)
                target.StatusEffectManager.AddStatModifier(Instantiate(AppliedStatModifier));
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }

        public override List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
            var buff=appliedBuff==null?null:appliedBuff.GetEffectDescription(level);
            var debuff=appliedDebuff==null?null:appliedDebuff.GetEffectDescription(level);
            var statModifier=AppliedStatModifier==null?null:AppliedStatModifier.GetEffectDescription(level);
         
            list.Add(buff);
            list.Add(debuff);
            list.AddRange(statModifier);
       
            return list;
        }

       
    }
}