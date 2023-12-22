using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    public enum GetHitEffectEnum
    {
        Pavise,
        Block,
        Wrath
    }
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/OnGetHitEffect", fileName = "OnGetHitEffect")]
    public class OnGetHitEffect : SelfTargetSkillEffectMixin
    {
        public GetHitEffectEnum getHitEffect;
        public string getHitExtraDataLabel;
        public ExtraDataType extraDataType;
        public float[] getHitEffectExtraData;
        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            string valueLabel = "";
            string upgLabel="";
            switch (extraDataType)
            {
                case ExtraDataType.number:
                    valueLabel = ""+getHitEffectExtraData[level];
                    if (level < getHitEffectExtraData.Length - 1)
                        level++;
                    upgLabel =""+getHitEffectExtraData[level];
                    list.Add(new EffectDescription(getHitExtraDataLabel,valueLabel , upgLabel));
                    break;
                case ExtraDataType.percentage:  
                    valueLabel = ""+getHitEffectExtraData[level]*100+"%";
                    if (level < getHitEffectExtraData.Length - 1)
                        level++;
                    upgLabel =""+getHitEffectExtraData[level]*100+"%";
                    list.Add(new EffectDescription(getHitExtraDataLabel, valueLabel , upgLabel));
                    break;
            }
            return list;
        }

        public override void Activate(Unit target, int level)
        {
            Debug.Log("OnGetHitEffect Activate!!!");
           
            target.BattleComponent.BattleStats.BonusAttackStats.AddGetHitEffect(getHitEffect,getHitEffectExtraData[level]);
        }

        public override void Deactivate(Unit target, int level)
        {
            target.BattleComponent.BattleStats.BonusAttackStats.DefenseEffects.Remove(getHitEffect);
        }
    }
}