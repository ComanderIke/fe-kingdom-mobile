using System;
using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Units.Numbers;
using Game.GameInput;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;
using Random = System.Random;

namespace Game.GameActors.Units.Skills.Passive
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnAttack", fileName = "OnAttack")]
    public class OnAttackEffectMixin:ChanceBasedPassiveSkillMixin
    {
        public AttackEffectEnum attackEffect;
        public string attackEffectExtraDataLabel;
        public ExtraDataType extraDataType;
        public float[] attackEffectExtraData;
        
       
        private void ReactToAttack(IBattleActor unit)
        {
            if(DoesActivate(skill.Level))
                unit.BattleComponent.BattleStats.BonusAttackStats.AttackEffects.Add(attackEffect,attackEffectExtraData[skill.Level]);
        }

        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            unit.BattleComponent.onAttack += ReactToAttack;
            
        }

        

        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.BattleComponent.onAttack -= ReactToAttack;
        }

        public override List<EffectDescription> GetEffectDescription(int level)
        {
            var list = base.GetEffectDescription(level);
            switch (extraDataType)
            {
                case ExtraDataType.number:  list.Add(new EffectDescription(attackEffectExtraDataLabel, ""+attackEffectExtraData[level], ""+attackEffectExtraData[level+1]));
                    break;
                case ExtraDataType.percentage:  list.Add(new EffectDescription(attackEffectExtraDataLabel, ""+attackEffectExtraData[level]*100+"%", ""+attackEffectExtraData[level+1]*100+"%"));
                    break;
            }
           return list;
        }
        void OnValidate()
        {
            if (attackEffectExtraData == null || attackEffectExtraData.Length != MAXLEVEL)
            {
                Array.Resize(ref attackEffectExtraData, MAXLEVEL);
            }

            base.OnValidate();
        }
    }
}