using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Override", fileName = "OverrideSkillEffect")]
    public class OverrideSkillEffectMixin : UnitTargetSkillEffectMixin
    {
        [SerializeField] private GameObject vfxPrefab;
        public List<SkillEffectMixin> skillEffects;
        public override void Activate(Unit target, Unit caster, int level)
        {
            Vector2 direction =  target.GridComponent.GridPosition.AsVector()-caster.GridComponent.GridPosition.AsVector() ;
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            Tile landOnTile = null;
            int newX = (int)(caster.GridComponent.GridPosition.X + direction.x);
            int newY = (int)(caster.GridComponent.GridPosition.Y + direction.y);
            
            Debug.Log("OVERRIDE ACTIVATE: "+direction);
            while (landOnTile == null||gridSystem.IsOutOfBounds(newX, newY))
            {
               
                if(gridSystem.GridLogic.IsValidLocation(caster,caster.GridComponent.GridPosition.X, caster.GridComponent.GridPosition.Y,newX, newY, false, true))
                {
                    landOnTile = gridSystem.Tiles[newX, newY];
                    Debug.Log("Valid Land on Tile: " + landOnTile.X+landOnTile.Y);
                }
                else
                {
                    if (gridSystem.Tiles[newX, newY].GridObject != null)
                    {
                        foreach (var effect in skillEffects)
                        {
                            if (effect is UnitTargetSkillEffectMixin utsm)
                            {
                                utsm.Activate((Unit)gridSystem.Tiles[newX, newY].GridObject ,caster,level);
                            }
                        }
                    }
                    newX = (int)(newX + direction.x);
                    newY = (int)(newY + direction.y);
                }
                
            }

            PlayVFX(new Vector3(landOnTile.X+0.5f, landOnTile.Y+0.5f,1), direction,(new Vector2(landOnTile.X, landOnTile.Y)-caster.GridComponent.GridPosition.AsVector()).magnitude*2-1);
            MoveUnit(caster, new Vector2(landOnTile.X, landOnTile.Y));
            
        }

        private void PlayVFX(Vector3 position,Vector2 direction, float size)
        {
           var go= GameObject.Instantiate(vfxPrefab);
           go.transform.position = position;
         
           float rotationZ = 0;
           if (direction.x > 0)
               rotationZ = 90;
           else if (direction.x < 0)
               rotationZ = 270;
           else if (direction.y > 0)
               rotationZ = 180;
           Debug.Log("Play VFX with Size: "+size+" RotationZ: "+rotationZ+" "+direction);
           go.GetComponent<LightningVFXController>().Play(size,rotationZ);
        }
        private void MoveUnit(Unit unit, Vector2 position)
        {
            var gridSystem= ServiceProvider.Instance.GetSystem<GridSystem>();
           // unit.GameTransformManager.UnitAnimator.Attack(1);
            Debug.Log("OVERRIDE MOVE TO: "+position);
            if(!gridSystem.IsOutOfBounds(position)&&gridSystem.GridLogic.IsTileFree((int)position.x,(int) position.y))
                gridSystem.SetUnitPosition(unit, (int)position.x,(int)position.y, true, false);//true, false
            LeanTween.move(unit.GameTransformManager.GameObject, position, .3f)
                .setEaseInOutBack();
            //  .setEaseOutQuad();
        }
        
        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
            foreach (var effect in skillEffects)
            {
                list.AddRange(effect.GetEffectDescription(level));
            }
            return list;
        }
    }
}