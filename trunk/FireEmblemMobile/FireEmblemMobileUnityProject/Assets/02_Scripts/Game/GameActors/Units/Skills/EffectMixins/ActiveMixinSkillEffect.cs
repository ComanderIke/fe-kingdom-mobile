using System.Collections.Generic;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/ActiveMixin", fileName = "ActiveMixinEffect")]
    public class ActiveMixinSkillEffect : SkillEffectMixin
    {

        public ActiveSkillMixin ActiveSkillMixin;

        public override void Activate(Unit target, Unit caster, int level)
        {
            //Wait until all effects happened then start new  ActiveSkillMixin Activation in ChooseTargetState.
            //Target from first activeMixin needs to be saved and parameterd to the new activeskillmixin.
           
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