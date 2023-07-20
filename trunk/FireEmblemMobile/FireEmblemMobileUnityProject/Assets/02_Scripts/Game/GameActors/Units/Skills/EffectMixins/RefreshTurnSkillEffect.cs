using System.Collections.Generic;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Refresh", fileName = "RefreshSkillEffect")]
    public class RefreshTurnSkillEffect : SkillEffectMixin
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

        public override void Activate(Unit target, int level)
        {
           
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