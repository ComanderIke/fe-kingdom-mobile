using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Base
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/AttackRangeEffect", fileName = "AttackRangeEffect")]
    public class AttackRangeEffect : SelfTargetSkillEffectMixin
    {
        
        public List<int> AttackRanges;

        public override void Activate(Unit target, int level)
        {
            target.BonusAttackRanges=AttackRanges;
        }

        public override void Deactivate(Unit target, int level)
        {
            target.BonusAttackRanges=new List<int>();
        }

        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();

            return list;
        }
    }
}