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
            switch (extraDataType)
            {
                case ExtraDataType.number:  list.Add(new EffectDescription(attackEffectExtraDataLabel, ""+attackEffectExtraData[level], ""+attackEffectExtraData[level+1]));
                    break;
                case ExtraDataType.percentage:  list.Add(new EffectDescription(attackEffectExtraDataLabel, ""+attackEffectExtraData[level]*100+"%", ""+attackEffectExtraData[level+1]*100+"%"));
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