using System;
using System.Collections.Generic;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnTile", fileName = "OnTileMixin")]
    public class OnTileEffectSkillMixin:PassiveSkillMixin, ITurnStateListener
    {
        public List<TerrainType> TerrainTypes;
        private bool activated = false;

        void OnTileChanged(Tile tile)
        {
            if (TerrainTypes.Contains(tile.TileData.TerrainType))
            {
                if (activated)
                    return;
                activated = true;
                foreach (var skillEffect in skillEffectMixins)
                {
                    if (skillEffect is SelfTargetSkillEffectMixin stm)
                    {
                        stm.Activate(skill.owner, skill.Level);
                    }
                    if (skillEffect is UnitTargetSkillEffectMixin uts)
                    {
                        uts.Activate(skill.owner, skill.owner, skill.Level);
                    }
                }
            }
            else if(activated)
            {
                activated = false;
                foreach (var skillEffect in skillEffectMixins)
                {
                    if (skillEffect is SelfTargetSkillEffectMixin stm)
                    {
                        stm.Deactivate(skill.owner, skill.Level);
                    }
                    if (skillEffect is UnitTargetSkillEffectMixin uts)
                    {
                        uts.Deactivate(skill.owner, skill.owner, skill.Level);
                    }
                }
            }
            
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            unit.GridComponent.OnTileChanged+=OnTileChanged;
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.GridComponent.OnTileChanged-=OnTileChanged;
        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            foreach (var skillEffect in skillEffectMixins)
            {
                list.AddRange(skillEffect.GetEffectDescription(unit,level));
            }
            return list;
        }
    }
}