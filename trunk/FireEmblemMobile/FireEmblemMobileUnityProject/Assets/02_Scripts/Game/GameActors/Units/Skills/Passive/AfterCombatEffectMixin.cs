using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    public interface IBattleEventListener
    {
        
    }
    public interface IAfterCombatListener : IBattleEventListener
    {
        
    }

    public enum BattleEvent
    {
        AfterCombat,
        BeforeCombat,
        DuringCombat
    }

    public enum AfterCombatEffect
    {
        Heal,
        StatBuff,
        Galeforce,
        Buff,
        Lunge,
    }
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/AfterCombat", fileName = "AfterCombatMixin")]
    public class AfterCombatEffectMixin:PassiveSkillMixin, IAfterCombatListener//TODO Sub Classes or switch case?
    {

        public AfterCombatEffect effect;
        public string effectLabel;
        public ExtraDataType extraDataType;
        public float[] attackEffectExtraData;
        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            unit.BattleComponent.AddListener(BattleEvent.AfterCombat, this);
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.BattleComponent.RemoveListener(BattleEvent.AfterCombat, this);
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