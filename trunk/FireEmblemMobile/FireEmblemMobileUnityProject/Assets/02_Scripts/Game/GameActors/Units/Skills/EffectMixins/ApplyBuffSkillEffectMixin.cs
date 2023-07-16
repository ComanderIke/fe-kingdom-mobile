using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/ApplyBuff", fileName = "ApplyBuffSkillEffect")]
    public class ApplyBuffSkillEffectMixin : SkillEffectMixin
    {
        public Buff appliedBuff;
        public StatModifier AppliedStatModifier;
        public Debuff appliedDebuff;

        public override void Activate(Unit target, Unit caster, int level)
        {
            throw new System.NotImplementedException();
        }

        public override void Activate(Unit target, int level)
        {
            if (appliedBuff != null)
                target.StatusEffectManager.AddBuff(Instantiate(appliedBuff));
            if (appliedDebuff != null)
                target.StatusEffectManager.AddDebuff(Instantiate(appliedDebuff));
            if (AppliedStatModifier!= null)
                target.StatusEffectManager.AddStatModifier(Instantiate(AppliedStatModifier));
        }
        public override void Activate(List<Unit> targets, int level)
        {
            foreach (var target in targets)
            {
                Activate(target, level);
            }
        }

        public override void Activate(Tile target, int level)
        {
            if (target.GridObject == null)
                return;
            if(target.GridObject is Unit u )
                Activate(u, level);
        }
    }
}