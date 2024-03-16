using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.Grid;
using Game.Manager;
using Game.Systems;
using LostGrace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Summon", fileName = "SummonSkillEffect")]
    public class SummonSkillEffectMixin : SelfTargetSkillEffectMixin
    {
        public GameObject effect;
        public List<UnitBP> summons;
        public bool resurrect;
        private Unit caster;
        public override void Activate(Unit caster, int level)
        {
            Debug.Log("ACTIVATE SUMMON EFFECT MIXIN");
            this.caster = caster;
            if (resurrect)
            {
                var screen = FindObjectOfType<ResurrectionScreen>();
                screen.Show( Player.Player.Instance.Party.deadMembers);
                screen.onUnitChosen += ResurrectUnit;
            }
            else
            {
                int rng = Random.Range(0, summons.Count);
                var summon =summons[rng].Create(Guid.NewGuid());
                UnitChosen(summon);
            }
           
        }

        void ResurrectUnit(Unit unit)
        {
            Player.Player.Instance.Party.ReviveCharacter(unit);
            if (GridGameManager.Instance != null) //Skill used on Grid
            {


                Vector2Int adjacentPos = GetFreeAdjacentPosition(unit);
                GridGameManager.Instance.FactionManager.ActiveFaction.AddUnit(unit);
                GridGameManager.Instance.GetSystem<ReinforcementSystem>()
                    .SpawnUnit(unit, unit.Faction.Id, adjacentPos.x, adjacentPos.y);
                ShowEffect();
            }
        }

        Vector2Int GetFreeAdjacentPosition(Unit unit)
        {
            var gridPosition = caster.GridComponent.GridPosition;
            int posX = -1;
            int posY = -1;
            var gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            
            Debug.Log("CASTER POS "+gridPosition.X+" "+gridPosition.Y);
            if (gridSystem.Tiles[gridPosition.X+1, gridPosition.Y].CanMoveOnto(unit))
            {
                posX = gridPosition.X + 1;
                posY = gridPosition.Y;
            }
            else if (gridSystem.Tiles[gridPosition.X-1, gridPosition.Y].CanMoveOnto(unit))
            {
                posX = gridPosition.X - 1;
                posY = gridPosition.Y;
            }
            else if (gridSystem.Tiles[gridPosition.X, gridPosition.Y+1].CanMoveOnto(unit))
            {
                posX = gridPosition.X;
                posY = gridPosition.Y+1;
            }
            else if (gridSystem.Tiles[gridPosition.X, gridPosition.Y-1].CanMoveOnto(unit))
            {
                posX = gridPosition.X;
                posY = gridPosition.Y-1;
            }

            return new Vector2Int(posX, posY);
        }
        void UnitChosen(Unit unit)
        {

            Vector2Int adjacentPos = GetFreeAdjacentPosition(unit);
            Debug.Log("SUMMON POSITION: "+adjacentPos.x+" "+adjacentPos.y);
           ShowEffect();
            GridGameManager.Instance.GetSystem<ReinforcementSystem>()
                .SpawnUnit(unit, caster.Faction.Id, adjacentPos.x, adjacentPos.y);
        }

        void ShowEffect()
        {
            if (effect != null)
                GameObject.Instantiate(effect, caster.GameTransformManager.GetCenterPosition(), Quaternion.identity);

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
