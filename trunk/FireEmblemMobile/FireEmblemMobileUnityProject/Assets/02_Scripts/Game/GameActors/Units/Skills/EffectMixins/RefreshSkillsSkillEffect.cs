using System.Collections.Generic;
using Game.Grid;
using Game.Manager;
using Game.Map;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/RefreshSkillsEffect", fileName = "RefreshSkillsEffect")]
    public class RefreshSkillsSkillEffect : UnitTargetSkillEffectMixin
    {
        public SkillTransferData skillTransferData;

        public float[] recoveryAmount;
        // public override void Activate(Unit target, Tile targetTile)
        // {
        //     ServiceProvider.Instance.GetSystem<GridSystem>().SetUnitPosition(target, targetTile.X, targetTile.Y);
        // }
        public override void Activate(Unit target, Unit caster, int level)
        {
            //Wait until all effects happened then start new  ActiveSkillMixin Activation in ChooseTargetState.
            //Target from first activeMixin needs to be saved and parameterd to the new activeskillmixin.
           
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(int level)
        {
            return new List<EffectDescription>()
            {
                new EffectDescription("Skill Recovery: ", recoveryAmount[level] * 100 + "%",
                    recoveryAmount[level + 1] * 100 + "%")
            };
        }
    
        
      
    }
}