using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/OnAttackEffect", fileName = "OnAttackEffect")]
    public class OnAttackEffect : SelfTargetSkillEffectMixin
    {
        public AttackEffectEnum attackEffect;
        public string attackEffectExtraDataLabel;
        public ExtraDataType extraDataType;
        public float[] attackEffectExtraData;
        public override List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
            string valueLabel = "";
            string upgLabel="";
            switch (extraDataType)
            {
                case ExtraDataType.number:
                    valueLabel = ""+attackEffectExtraData[level];
                    if (level < attackEffectExtraData.Length - 1)
                        level++;
                    upgLabel =""+attackEffectExtraData[level];
                    list.Add(new EffectDescription(attackEffectExtraDataLabel,valueLabel , upgLabel));
                    break;
                case ExtraDataType.percentage:  
                    valueLabel = ""+attackEffectExtraData[level]*100+"%";
                    if (level < attackEffectExtraData.Length - 1)
                        level++;
                    upgLabel =""+attackEffectExtraData[level]*100+"%";
                    list.Add(new EffectDescription(attackEffectExtraDataLabel, valueLabel , upgLabel));
                    break;
            }
            return list;
        }

        public override void Activate(Unit target, int level)
        {
            Debug.Log("OnAttackEffect Activate!!!");
           
            target.BattleComponent.BattleStats.BonusAttackStats.AddAttackEffect(attackEffect,attackEffectExtraData[level]);
        }

        public override void Deactivate(Unit target, int level)
        {
            target.BattleComponent.BattleStats.BonusAttackStats.AttackEffects.Remove(attackEffect);
        }
    }

    
}