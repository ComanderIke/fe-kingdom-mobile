using System.Collections.Generic;
using Game.Grid;
using Game.Manager;
using Game.Map;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/ChainAttack", fileName = "ChainAttackSkillEffect")]
    public class ChainAttackSkillMixin:UnitTargetSkillEffectMixin
    {
        public int[] bounces;
        public int bounceRange;
        public GameObject animation;

        

        public override void Activate(Unit target,Unit caster, int level)
        {
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            var startposition = target.GridComponent.GridPosition;
            List<Unit> targets = new List<Unit>();
            targets.Add(target);
            for (int x = -bounceRange; x <= bounceRange; x++)
            {
                for (int y = -bounceRange; y <= bounceRange; y++)
                {
                    int currentX = startposition.Y+ x;
                    int currentY = startposition.Y + y;
                    if (gridSystem.Tiles[currentX, currentY].GridObject != null && gridSystem.Tiles[currentX, currentY].GridObject is Unit unit)
                    {
                        if (!unit.Faction.IsOpponentFaction(target.Faction))
                        {
                            if (!targets.Contains(target))
                            {
                                targets.Add(target);
                                if (targets.Count <= bounceRange + 1)
                                {
                                    startposition = target.GridComponent.GridPosition;
                                    x = -bounceRange;
                                    y = -bounceRange;
                                }
                                else
                                {
                                    ActivateChainAttack(targets);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }

        private void ActivateChainAttack(List<Unit> targets)
        {
            Debug.Log("Do Chain Attack");
        }
        
        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            return new List<EffectDescription>()
            {
                new EffectDescription("Bounces: ", "" + bounces[level],
                    "" + bounces[level + 1]),
                new EffectDescription("BounceRange: ", "" + bounceRange,
                "" + bounceRange)
            };
        }
    }
}