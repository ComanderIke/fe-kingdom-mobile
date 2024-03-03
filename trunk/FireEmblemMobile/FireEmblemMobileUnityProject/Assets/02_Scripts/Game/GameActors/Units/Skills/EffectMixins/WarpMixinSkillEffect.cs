using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.Grid;
using Game.Grid.Tiles;
using Game.Manager;
using UnityEngine;

namespace Game.GameActors.Units.Skills.EffectMixins
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