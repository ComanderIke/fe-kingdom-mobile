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
        public BuffDebuffBase appliedBuff;
        public StatModifier AppliedStatModifier;
        private bool applied = false;
       

        

        public override void Activate(Unit target,Unit caster, int level)
        {
            Debug.Log("ACTIVATE APPLY BUFF EFFECT MIXIN");
            float rng = Random.value;
            if (applyChance.Length <= level || rng <= applyChance[level])
            {
                applied = true;
                if (effect != null)
                    GameObject.Instantiate(effect, target.GameTransformManager.GetCenterPosition(),
                        Quaternion.identity);
                if (appliedBuff != null)
                    target.StatusEffectManager.AddBuffDebuff(Instantiate(appliedBuff), caster, level);
                if (AppliedStatModifier != null)
                {
                    Debug.Log("ADD STAT MODIFIER");
                    target.StatusEffectManager.AddStatModifier(Instantiate(AppliedStatModifier), level);
                }
            }
        }

        public override void Deactivate(Unit target, Unit caster, int skillLevel)
        {
            if (target == null||!applied)
                return;
            Debug.Log("REMOVE BUFF/DEBUFF/STAT MODIFIER");
            if (appliedBuff != null)
                target.StatusEffectManager.RemoveBuff(Instantiate(appliedBuff));
            if (AppliedStatModifier!= null)
                target.StatusEffectManager.RemoveStatModifier(Instantiate(AppliedStatModifier));
        }

        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            var buff=appliedBuff==null?null:appliedBuff.GetEffectDescription(caster, level);
            var statModifier=AppliedStatModifier==null?null:AppliedStatModifier.GetEffectDescription(level);

            if (applyChance.Length > level&& applyChance[level]!=1f)
            {
                string val = applyChance[level] * 100f + "%";
                string upg = applyChance.Length > level + 1 ? applyChance[level+1] * 100f + "%" : val;
                list.Add(new EffectDescription("Chance: ", val,upg));
            }
            if(buff!=null)
             list.AddRange(buff);
            if(statModifier!=null)
                list.AddRange(statModifier);
       
            return list;
        }

       
    }
}