using System.Collections.Generic;
using Game.Grid;
using Game.Manager;
using Game.Map;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/ChainAttack", fileName = "ChainAttackSkillEffect")]
    public class ChainAttackSkillMixin:SkillEffectMixin
    {
        public int[] bounces;
        public int bounceRange;
        public GameObject animation;

        public override void Activate(Unit target, Unit caster, int level)
        {
            throw new System.NotImplementedException();
        }

        public override void Activate(Unit target, int level)
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

        private void ActivateChainAttack(List<Unit> targets)
        {
            Debug.Log("Do Chain Attack");
        }

        public override void Activate(Tile target, int level)
        {
            throw new System.NotImplementedException();
        }

        public override void Activate(List<Unit> targets, int level)
        {
            throw new System.NotImplementedException();
        }
    }
}