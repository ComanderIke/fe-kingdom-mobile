using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]

    public enum AuraType
    {
        DuringCombat,
        Always,
        EnemyPhase,
        PlayerPhase,
        OnInitiateCombat,
        OnInitiatedOnCombat
    }

    public enum AffectType
    {
        Allies,
        Enemies,
        All
    }
    public class AuraSkillMixin:PassiveSkillMixin
    {

        public AuraType auraType;
        public AffectType affectType;
        public BonusStats BonusStats;
        private List<Unit> inRangeTargets;
        private int[] range;
     
        // check whenever a unit moves
        // if ally and is in range add to List
        // if ally leaves range remove from list
        // also check at start of turn(or start of battle?) and update list;
        // the depending on auratype add a listener to those units which checks for during combat etc.. then add stats
        
        //other option apply auratype to Tile (and give reference to this skilleffectinstance)and update whenever this unit moves
        //then if a unit enters a tile with it will call a BindAuraToUnit method 
        //Here it will apply listeners depending on aura type
        //unbind if aura disappears or unit moves away
        public override void BindToUnit(Unit unit, Skill skill)
        {
           
            base.BindToUnit(unit,skill);
            inRangeTargets = new List<Unit>();
            MovementState.OnAnyUnitMoved += OnAnyUnitMoved;
            //TODO list of targets done but no subscription to oncombat or w/e yet

        }

        void OnAnyUnitMoved(Unit unit)
        {
            if (unit == skill.owner)
            {
                inRangeTargets.Clear();
                var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
                var startPos = unit.GridComponent.GridPosition;
                for (int x = -range[skill.Level]; x <= range[skill.Level]; x++)
                {
                    for (int y = -range[skill.Level]; y <= range[skill.Level]; y++)
                    {
                        if(x==0 && y==0)
                            continue;
                        var currentPos = new Vector2Int(startPos.X + x, startPos.Y + y);
                        var currentTile = gridSystem.Tiles[currentPos.x, currentPos.y];//TODO Check boundaries
                        if (currentTile.GridObject != null && currentTile.GridObject is Unit unitOnTile)
                        {
                            if (affectType == AffectType.All)
                            {
                                if(!inRangeTargets.Contains(unitOnTile))
                                    inRangeTargets.Add(unitOnTile);
                            }
                            else if (affectType == AffectType.Allies && unitOnTile.Faction.Id == unit.Faction.Id)
                            {
                                if(!inRangeTargets.Contains(unitOnTile))
                                    inRangeTargets.Add(unitOnTile);
                            }
                            else if (affectType == AffectType.Enemies && unitOnTile.Faction.IsOpponentFaction(unit.Faction))
                            {
                                if(!inRangeTargets.Contains(unitOnTile))
                                    inRangeTargets.Add(unitOnTile);
                            }
                        }
                    }
                }
            }
            else if (affectType == AffectType.All)
            {
                if (unit.GridComponent.IsInRange(skill.owner.GridComponent, range[skill.Level]))
                {
                    if(!inRangeTargets.Contains(unit))
                     inRangeTargets.Add(unit);
                }
                else
                {
                    if (inRangeTargets.Contains(unit))
                        inRangeTargets.Remove(unit);
                }
            }
            else if (unit.Faction.Id == skill.owner.Faction.Id&&affectType==AffectType.Allies)
            {
                if (unit.GridComponent.IsInRange(skill.owner.GridComponent, range[skill.Level]))
                {
                    if(!inRangeTargets.Contains(unit))
                        inRangeTargets.Add(unit);
                }
                else
                {
                    if (inRangeTargets.Contains(unit))
                        inRangeTargets.Remove(unit);
                }
                //Unit is ally
            }
            else if(unit.Faction.IsOpponentFaction(skill.owner.Faction)&&affectType==AffectType.Enemies)
            {
                if (unit.GridComponent.IsInRange(skill.owner.GridComponent, range[skill.Level]))
                {
                    if(!inRangeTargets.Contains(unit))
                        inRangeTargets.Add(unit);
                }
                else
                {
                    if (inRangeTargets.Contains(unit))
                        inRangeTargets.Remove(unit);
                }
            }
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            // Unit.OnAnyUnitsCombat -= OnAnyUnitsCombat;
           
            base.UnbindFromUnit(unit,skill);
            inRangeTargets.Clear();
            MovementState.OnAnyUnitMoved -= OnAnyUnitMoved;
        }

        // private void OnAnyUnitsCombatEnded(Unit unit)
        // {
        //     if (inRangeTargets.Contains(unit))
        //     {
        //         inRangeTargets.Remove(unit);
        //         unit.BattleComponent.BattleStats.BonusStats.Decrease(BonusStats);
        //     }
        // }
        // private void OnAnyUnitsCombat(Unit unit)
        // {
        //     //Check if unit is adjacent
        //     inRangeTargets.Clear();
        //     if (unit.GridComponent.IsInRange(skill.owner.GridComponent, range[skill.Level]))
        //     {
        //         inRangeTargets.Add(unit);
        //         unit.BattleComponent.BattleStats.BonusStats.Add(BonusStats);
        //     }
        // }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            throw new NotImplementedException();
        }
    }
}