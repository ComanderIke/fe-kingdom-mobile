using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.Grid;
using Game.Manager;
using Game.Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Summon", fileName = "SummonSkillEffect")]
    public class SummonSkillEffectMixin : SelfTargetSkillEffectMixin
    {
        public GameObject effect;
        public List<UnitBP> summons;
        
        public override void Activate(Unit caster, int level)
        {
            Debug.Log("ACTIVATE SUMMON EFFECT MIXIN");
          
            int rng = Random.Range(0, summons.Count);
            var gridPosition = caster.GridComponent.GridPosition;
            
            int posX = -1;
            int posY = -1;
            var gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            var summon =summons[rng].Create(Guid.NewGuid());
            Debug.Log("CASTER POS "+gridPosition.X+" "+gridPosition.Y);
            if (gridSystem.Tiles[gridPosition.X+1, gridPosition.Y].CanMoveOnto(summon))
            {
                posX = gridPosition.X + 1;
                posY = gridPosition.Y;
            }
            else if (gridSystem.Tiles[gridPosition.X-1, gridPosition.Y].CanMoveOnto(summon))
            {
                posX = gridPosition.X - 1;
                posY = gridPosition.Y;
            }
            else if (gridSystem.Tiles[gridPosition.X, gridPosition.Y+1].CanMoveOnto(summon))
            {
                posX = gridPosition.X;
                posY = gridPosition.Y+1;
            }
            else if (gridSystem.Tiles[gridPosition.X, gridPosition.Y-1].CanMoveOnto(summon))
            {
                posX = gridPosition.X;
                posY = gridPosition.Y-1;
            }
            else
            {
                return;
            }
            Debug.Log("SUMMON POSITION: "+posX+" "+posY);
            if (effect != null)
                GameObject.Instantiate(effect, caster.GameTransformManager.GetCenterPosition(), Quaternion.identity);

            
            GridGameManager.Instance.GetSystem<ReinforcementSystem>().SpawnUnit(summon, caster.Faction.Id, posX, posY);
        }

        public override void Deactivate(Unit caster, int skillLevel)
        {
            
        }

        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
       
            return list;
        }

       
    }
}