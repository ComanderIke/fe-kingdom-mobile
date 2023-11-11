using System.Collections.Generic;
using Game.Grid;
using Game.Manager;
using Game.Map;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/WarpEffectMixin", fileName = "WarpMixinEffect")]
    public class WarpMixinSkillEffect : TileTargetSkillEffectMixin
    {
        public SkillTransferData skillTransferData;

        public override void Activate(Tile target, int level)
        {
            Debug.Log("Activate Warp Effect");
            ServiceProvider.Instance.GetSystem<GridSystem>().SetUnitPosition((Unit)(skillTransferData.data), target.X, target.Y);
        }
        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            return null;
        }
    }
}