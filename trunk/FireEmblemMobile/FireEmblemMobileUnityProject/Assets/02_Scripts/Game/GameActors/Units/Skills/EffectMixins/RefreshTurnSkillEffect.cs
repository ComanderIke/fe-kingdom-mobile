using System.Collections.Generic;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Refresh", fileName = "RefreshSkillEffect")]
    public class RefreshTurnSkillEffect : UnitTargetSkillEffectMixin
    {
        public bool RefreshTarget = true;
        public bool RefreshCaster;

        public override void Activate(Unit target, Unit caster, int level)
        {
            if (RefreshCaster)
            {
                caster.TurnStateManager.Reset();
            }

            if (RefreshTarget)
            {
                target.TurnStateManager.Reset();
            }
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            return null;
        }


     
    }
}