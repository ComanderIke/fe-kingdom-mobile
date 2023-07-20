using System.Collections.Generic;
using Game.Grid;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/WarpEffectMixin", fileName = "WarpMixinEffect")]
    public class WarpMixinSkillEffect : SkillEffectMixin
    {
        public SkillTransferData skillTransferData;
        
        // public override void Activate(Unit target, Tile targetTile)
        // {
        //     ServiceProvider.Instance.GetSystem<GridSystem>().SetUnitPosition(target, targetTile.X, targetTile.Y);
        // }
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
           
        }

        

        public override void Activate(Tile target, int level)
        {
            Debug.Log("ACtivate Warp Effect");
            ServiceProvider.Instance.GetSystem<GridSystem>().SetUnitPosition((Unit)(skillTransferData.data), target.X, target.Y);
            // if (target.GridObject == null)
            //     return;
            // if(target.GridObject is Unit u )
            //     Activate(u, level);
        }
    }
}