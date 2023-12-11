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
        void OnTileChanged(Tile tile)
        {
            UpdateContext(skill.owner, tile);
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