using System;
using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/InfluenceMovement", fileName = "OnInfluenceMovMixin")]
    public class InfluenceMovementSkillMixin:PassiveSkillMixin, ITurnStateListener
    {
        public SerializableDictionary<TerrainType, int> TerrainTypeMovementCostsReduction;
        
        public bool canMoveThroughEnemies;
        public List<SkillEffectMixin> onMoveOverEnemy;
        
        private bool activated = false;
        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            activated = true;
            foreach (var keyValuePair in TerrainTypeMovementCostsReduction)
            {
                if (unit.GridComponent is GridActorComponent actor)
                {
                   
                    actor.AddBonusMovementCosts(keyValuePair.Key, keyValuePair.Value);
                }
            }
            
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            if (activated)
            {
                foreach (var keyValuePair in TerrainTypeMovementCostsReduction)
                {
                    if (unit.GridComponent is GridActorComponent actor)
                        actor.RemoveBonusMovementCosts(keyValuePair.Key, keyValuePair.Value);
                }
            }

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