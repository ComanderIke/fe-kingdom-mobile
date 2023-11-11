using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Movement", fileName = "MovementSkillEffect")]
    public class MovementSkillEffectMixin : UnitTargetSkillEffectMixin
    {
        public int targetMove;
        public int selfMove;
        public bool towardsTarget;
        public bool swapPositions;
        public bool rescueTarget;
        public bool priotizeXMovement;
        public bool towardsSkillTargetDataPosition;
        public bool lastSkilltargetDirection;
        public bool randomMovement;
        public SkillTransferData skillTransferData;

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
            if (lastSkilltargetDirection)
            {
                Vector2Int lastPos= ChooseTargetState.LastSkillTargetPosition;
                direction =  lastPos- caster.GridComponent.GridPosition.AsVector() * (towardsTarget?-1:1);
            } 
            if (towardsTarget)
            {
                if (targetMove != 0)
                {
                    MoveUnit(target, direction, 0);
                }
                if (selfMove != 0)
                {
                    MoveUnit(caster, direction, 0);
                }
            }

            if (randomMovement)
            {
                var gridSystem= ServiceProvider.Instance.GetSystem<GridSystem>();
                Vector2 rand =new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
                Vector2Int newPosition = new Vector2Int(target.GridComponent.GridPosition.X + (int)rand.x,
                    target.GridComponent.GridPosition.Y + (int)rand.y);
                int radius = 1;
                int cnt = 0;
                Debug.Log("Rand: "+rand);
                while ((rand.x==0&&rand.y==0)||gridSystem.IsOutOfBounds(newPosition.x, newPosition.y)||!gridSystem.GridLogic.IsTileFreeAndNotBlocked(newPosition.x, newPosition.y))
                {
                    rand = new Vector2(Random.Range(-radius, radius+1), Random.Range(-radius, radius+1));
                    newPosition = new Vector2Int(target.GridComponent.GridPosition.X + (int)rand.x,
                        target.GridComponent.GridPosition.Y + (int)rand.y);
                    if (cnt >= 10)
                    {
                        radius++;
                        cnt = 0;
                        if (radius > 3)
                            return;
                        //return;
                    }

                    cnt++;
                    Debug.Log("Rand: "+rand);
                    Debug.Log("NewPosition: "+newPosition);
                
                }
                Debug.Log("move Unit: "+rand);
                MoveUnit(target, rand, skillTransferData==null?0:(float)skillTransferData.data);
            }
            
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }

        private void MoveUnit(Unit unit, Vector2 direction, float delay)
        {
            var gridSystem= ServiceProvider.Instance.GetSystem<GridSystem>();
            Vector2Int newPosition = new Vector2Int(unit.GridComponent.GridPosition.X + (int)direction.x,
                unit.GridComponent.GridPosition.Y + (int)direction.y);
            if(!gridSystem.IsOutOfBounds(newPosition)&&gridSystem.GridLogic.IsTileFree(newPosition.x, newPosition.y))
                gridSystem.SetUnitPosition(unit, newPosition.x,newPosition.y, true, false);//true, false
            LeanTween.move(unit.GameTransformManager.GameObject, new Vector2(newPosition.x, newPosition.y), .3f)
                .setEaseOutSine().setDelay(delay);
            //  .setEaseOutQuad();
        }
      
        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            return new List<EffectDescription>();
        }
        
    }
}