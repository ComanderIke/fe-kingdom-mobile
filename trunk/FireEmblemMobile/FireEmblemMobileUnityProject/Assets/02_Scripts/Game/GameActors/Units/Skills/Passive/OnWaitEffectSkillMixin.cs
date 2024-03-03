using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills.Base;
using Game.GameActors.Units.Skills.Enums;
using Game.GameActors.Units.UnitState;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnWait", fileName = "OnWaitMixin")]
    public class OnWaitEffectSkillMixin:PassiveSkillMixin, ITurnStateListener
    {
        public enum OnWaitEffect
        {
            DEFBuff,
            AvoBuff,
            Heal,
            
        }
        public OnWaitEffect effect;
        public string effectLabel;
        public ExtraDataType extraDataType;
        public float[] attackEffectExtraData;
      
        void OnWait(Unit unit)
        {
            switch (effect)
            {
                case OnWaitEffect.Heal:
                {
                    switch (extraDataType)
                    {
                        case ExtraDataType.number:
                            unit.Heal((int)attackEffectExtraData[skill.Level]);break;
                            break;
                        case ExtraDataType.percentage:
                            unit.Heal((int)(unit.MaxHp/attackEffectExtraData[skill.Level]));break;
                            break;
                    }

                    
                }
                    break;
            }
            
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            unit.TurnStateManager.AddListener(TurnStateManager.TurnStateEvent.Wait, this);
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.TurnStateManager.RemoveListener(TurnStateManager.TurnStateEvent.Wait, this);
        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            switch (extraDataType)
            {
                case ExtraDataType.number:  list.Add(new EffectDescription(effectLabel, ""+attackEffectExtraData[level], ""+attackEffectExtraData[level+1]));
                    break;
                case ExtraDataType.percentage:  list.Add(new EffectDescription(effectLabel, ""+attackEffectExtraData[level]*100+"%", ""+attackEffectExtraData[level+1]*100+"%"));
                    break;
            }
            return list;
        }
    }
   
}