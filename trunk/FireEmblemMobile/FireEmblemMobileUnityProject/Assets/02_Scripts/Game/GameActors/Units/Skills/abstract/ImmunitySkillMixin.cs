using System;
using System.Collections.Generic;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Immunity", fileName = "ImmunityMixin")]
    public class ImmunitySkillMixin:PassiveSkillMixin
    {
        [SerializeField]
        private BattleStats.ImmunityType type;
        
        
        public override void BindToUnit(Unit unit, Skill skill)
        {
            unit.BattleComponent.BattleStats.Immunities.Add(type);
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            unit.BattleComponent.BattleStats.Immunities.Remove(type);
        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Immunity:", ""+type, ""+type));
            return list;
        }
    }
}