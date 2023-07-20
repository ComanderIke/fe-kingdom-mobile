using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Movement", fileName = "MovementSkillEffect")]
    public class MovementSkillEffectMixin : SkillEffectMixin
    {
        public int targetMove;
        public int selfMove;
        public bool towardsTarget;
        public bool swapPositions;
        public bool rescueTarget;

        private bool RescueToPosition( Vector2Int pos, Unit target )
        {
            var gridSystem= ServiceProvider.Instance.GetSystem<GridSystem>();
            if (!gridSystem.IsOutOfBounds(pos.x, pos.y)&&gridSystem.GridLogic.IsTileFree(pos.x, pos.y))
            {
                gridSystem.SetUnitPosition(target, pos.x, pos.y);
                return true;
            }

            return false;
        }
        public override void Activate(Unit target, Unit caster, int level)
        {
             // check if objects in the way
             if (rescueTarget)
             {
              
                 
                 // First check tile farthest to target if same distance first left then down then up then right
                 Vector2Int left = new Vector2Int(caster.GridComponent.GridPosition.X - 1,
                     caster.GridComponent.GridPosition.Y);
                 Vector2Int right = new Vector2Int(caster.GridComponent.GridPosition.X + 1,
                     caster.GridComponent.GridPosition.Y);
                 Vector2Int top = new Vector2Int(caster.GridComponent.GridPosition.X ,
                     caster.GridComponent.GridPosition.Y+1);
                 Vector2Int bottom = new Vector2Int(caster.GridComponent.GridPosition.X ,
                     caster.GridComponent.GridPosition.Y-1);

                 if (RescueToPosition(left, target))
                     return;
                 if (RescueToPosition(bottom, target))
                     return;
                 if (RescueToPosition(top, target))
                     return;
                 RescueToPosition(right, target);
                 return;
             }
             if (swapPositions)
             {
                 var gridSystem= ServiceProvider.Instance.GetSystem<GridSystem>();
                 Vector2 tmpPos = target.GridComponent.GridPosition.AsVector();
                 gridSystem.SetUnitPosition(target, caster.GridComponent.GridPosition.X, caster.GridComponent.GridPosition.Y);
                 gridSystem.SetUnitPosition(caster, (int)tmpPos.x, (int)tmpPos.y);
                 return;
             }
            Vector2 direction = target.GridComponent.GridPosition.AsVector() - caster.GridComponent.GridPosition.AsVector() * (towardsTarget?-1:1);
            if (towardsTarget)
            {
                if (targetMove != 0)
                {
                    MoveUnit(target, direction);
                }
                if (selfMove != 0)
                {
                    MoveUnit(caster, direction);
                }
            }
            else
            {
                if (selfMove != 0)
                {
                    MoveUnit(caster, direction);
                }
                if (targetMove != 0)
                {
                    MoveUnit(target, direction);
                }
            }
        }

        private void MoveUnit(Unit unit, Vector2 direction)
        {
            var gridSystem= ServiceProvider.Instance.GetSystem<GridSystem>();
            Vector2Int newPosition = new Vector2Int(unit.GridComponent.GridPosition.X + (int)direction.x,
                unit.GridComponent.GridPosition.Y + (int)direction.y);
            if(!gridSystem.IsOutOfBounds(newPosition)&&gridSystem.GridLogic.IsTileFree(newPosition.x, newPosition.y))
                gridSystem.SetUnitPosition(unit, newPosition.x,newPosition.y);
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

    public class Vector2int
    {
    }
}