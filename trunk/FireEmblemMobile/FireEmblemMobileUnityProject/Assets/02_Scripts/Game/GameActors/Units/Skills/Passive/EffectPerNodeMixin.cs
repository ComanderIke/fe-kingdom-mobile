using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Passive;
using UnityEngine;

namespace LostGrace
{
    public enum PerNodeEffect
    {
        Heal,
        Exp,
    }

    public enum EncounterEvent
    {
        LeaveNode
    }
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/EffectPerNode", fileName = "EffectPerNodeMixin")]
    public class EffectPerNodeMixin : PassiveSkillMixin, IEncounterEventListener
    {
        public PerNodeEffect effect;
        public string effectLabel;
        public ExtraDataType extraDataType;
        public float[] attackEffectExtraData;

      
        void Activate( EncounterNode node, Unit u)
        {
            switch (effect)
            {
                case PerNodeEffect.Heal:  u.Heal((int)attackEffectExtraData[skill.Level]);break;
            }
           
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            unit.EncounterComponent.AddListener(EncounterEvent.LeaveNode, this);
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.EncounterComponent.RemoveListener(EncounterEvent.LeaveNode, this);
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