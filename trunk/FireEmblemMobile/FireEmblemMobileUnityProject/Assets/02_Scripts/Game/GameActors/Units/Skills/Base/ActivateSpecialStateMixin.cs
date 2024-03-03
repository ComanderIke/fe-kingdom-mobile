using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Base
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/SpecialState", fileName = "SpecialStateEffect")]
    public class ActivateSpecialStateMixin : SelfTargetSkillEffectMixin
    {
        public override void Activate(Unit target, int level)
        {
            target.SpecialState = true;
        }

        public override void Deactivate(Unit user, int skillLevel)
        {
            user.SpecialState = false;
        }

        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            return new List<EffectDescription>();
        }
    }
}