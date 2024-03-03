using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataAndReferences.References;
using Game.GameActors.Units.Components;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.GameActors.Units.Skills.Enums;
using Game.Grid;
using Game.Manager;
using Game.Map;
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

    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Aura", fileName = "AuraMixin")]
    public class AuraSkillMixin : PassiveSkillMixin
    {
        public AuraType auraType;
        public AffectType affectType;
        public SkillTargetArea areaType;
        private List<Unit> inRangeTargets;
        [SerializeField] private bool visualizeAura = false;
        
       [SerializeField] private int[] range;
        
        void AddToTargetList(Unit u)
        {
            if (inRangeTargets.Contains(u))
                return;
            Debug.Log("Add to AuraTargetList: "+u.name);
            inRangeTargets.Add(u);
            foreach (var skillEffect in skillEffectMixins)
            {
                if (skillEffect is UnitTargetSkillEffectMixin utsm)
                {
                    utsm.Activate(u, skill.owner, skill.level);
                }
                if (skillEffect is SelfTargetSkillEffectMixin stsm)
                {
                    stsm.Activate(u, skill.level);
                }
            }
        }
        void RemoveFromTargetList(Unit u)
        {
            Debug.Log("Remove from AuraTargetList: "+u.name);
            foreach (var skillEffect in skillEffectMixins)
            {
                if (skillEffect is UnitTargetSkillEffectMixin utsm)
                {
                    utsm.Deactivate(u, skill.owner, skill.level);
                }
                if (skillEffect is SelfTargetSkillEffectMixin stsm)
                {
                    stsm.Deactivate(u, skill.level);
                }
            }
            inRangeTargets.Remove(u);
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            base.BindToUnit(unit, skill);
            Debug.Log("BIND TO UNIT========================================");
            inRangeTargets = new List<Unit>();
            //Also on ResetPosition
            //TurnSystem.OnStartOfFirstTurn += UpdateList;
            GridActorComponent.AnyUnitChangedPosition += OnAnyUnitMoved;//Not just moving but every time a unit position changes
                                                               //(warp, jump, reset position, attack preview from different postition
                                                               //so better subscribe to UnitSetPosition Function
        }

        void ClearAllTargets()
        {
            Debug.Log("Clear All Targets");
            for (int i = inRangeTargets.Count - 1; i >= 0; i--)
            {
                RemoveFromTargetList(inRangeTargets[i]);
            }
            inRangeTargets.Clear();
        }

        private List<GameObject> InstantiatedVisualizations;
        void ShowVisualization(int x, int y)
        {
            if (!visualizeAura)
                return;
            if (InstantiatedVisualizations == null)
                InstantiatedVisualizations = new List<GameObject>();
            GameObject go=Instantiate(GameAssets.Instance.visuals.debug.auraRangePrefab, new Vector3(x, y, 0.01f), Quaternion.identity,null) as GameObject;
            InstantiatedVisualizations.Add(go);
        }
        void ClearVisualization()
        {
            if (!visualizeAura)
                return;
            if (InstantiatedVisualizations == null)
                return;
            for (int i = InstantiatedVisualizations.Count - 1; i >= 0; i--)
            {
                Destroy(InstantiatedVisualizations[i]);
            }
            InstantiatedVisualizations.Clear();
        }
        void OnAnyUnitMoved(IGridActor gridActor)
        {
            Unit unit = (Unit)gridActor;
            Debug.Log("On Unit Moved: "+unit.name);
            Debug.Log("Skillowner: "+skill.owner);
            if (unit.Equals(skill.owner))
            {
                ClearVisualization();
                ClearAllTargets();
                var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
                var startPos = unit.GridComponent.GridPosition;
                //Debug.Log("Check for objects in range: "+range[skill.Level]);
                for (int x = -range[skill.Level]; x <= range[skill.Level]; x++)
                {
                    for (int y = -range[skill.Level]; y <= range[skill.Level]; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;
                      
                        var currentPos = new Vector2Int(startPos.X + x, startPos.Y + y);
                        ShowVisualization(currentPos.x, currentPos.y);
                        //Debug.Log("CURRENT POS: "+currentPos);
                        if(gridSystem.IsOutOfBounds(currentPos))
                            continue;
                        
                        var currentTile = gridSystem.Tiles[currentPos.x, currentPos.y]; 
                       
                        if (currentTile.GridObject != null && currentTile.GridObject is Unit unitOnTile)
                        {
                           
                            if(unitOnTile.Equals(skill.owner))
                                continue;
                            //Debug.Log("GRIDOBJECT DETECTED: "+currentPos);
                            if (affectType == AffectType.All)
                            {
                                if (!inRangeTargets.Contains(unitOnTile))
                                    AddToTargetList(unitOnTile);
                                    
                            }
                            else if (affectType == AffectType.Allies && unitOnTile.Faction.Id == unit.Faction.Id)
                            {
                                if (!inRangeTargets.Contains(unitOnTile))
                                    AddToTargetList(unitOnTile);
                            }
                            else if (affectType == AffectType.Enemies &&
                                     unitOnTile.Faction.IsOpponentFaction(unit.Faction))
                            {
                                //Debug.Log("ENEMY DETECTED: "+currentPos);
                                if (!inRangeTargets.Contains(unitOnTile))
                                    AddToTargetList(unitOnTile);
                            }
                        }
                    }
                }
            }
            else if (affectType == AffectType.All)
            {
                var diff = new Vector2Int(Math.Abs(skill.owner.GridComponent.GridPosition.X - unit.GridComponent.GridPosition.X),
                    Math.Abs(skill.owner.GridComponent.GridPosition.Y - unit.GridComponent.GridPosition.Y));
                var r = range[skill.Level];
                if ((diff.x<= r&&diff.y<=r))
                {
                    if (!inRangeTargets.Contains(unit))
                        AddToTargetList(unit);
                }
                else
                {
                    if (inRangeTargets.Contains(unit))
                        RemoveFromTargetList(unit);
                }
            }
            else if (unit.Faction.Id == skill.owner.Faction.Id && affectType == AffectType.Allies)
            {
                var diff = new Vector2Int(Math.Abs(skill.owner.GridComponent.GridPosition.X - unit.GridComponent.GridPosition.X),
                    Math.Abs(skill.owner.GridComponent.GridPosition.Y - unit.GridComponent.GridPosition.Y));
                var r = range[skill.Level];
                if ((diff.x<= r&&diff.y<=r))
                {
                    if (!inRangeTargets.Contains(unit))
                        AddToTargetList(unit);
                }
                else
                {
                    if (inRangeTargets.Contains(unit))
                        RemoveFromTargetList(unit);
                }
                //Unit is ally
            }
            else if (unit.Faction.IsOpponentFaction(skill.owner.Faction) && affectType == AffectType.Enemies)
            {
                Debug.Log("IS ENEMY");
                var diff = new Vector2Int(Math.Abs(skill.owner.GridComponent.GridPosition.X - unit.GridComponent.GridPosition.X),
                    Math.Abs(skill.owner.GridComponent.GridPosition.Y - unit.GridComponent.GridPosition.Y));
                var r = range[skill.Level];
                if ((diff.x<= r&&diff.y<=r))
                {
                    if (!inRangeTargets.Contains(unit))
                        AddToTargetList(unit);
                }
                else
                {
                    if (inRangeTargets.Contains(unit))
                        RemoveFromTargetList(unit);
                }
            }
        }

        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            // Unit.OnAnyUnitsCombat -= OnAnyUnitsCombat;

            base.UnbindFromUnit(unit, skill);
            if(inRangeTargets==null)
                inRangeTargets = new List<Unit>();
            ClearAllTargets();
            Debug.Log("UNBIND TO UNIT========================================");
            
            GridActorComponent.AnyUnitChangedPosition -= OnAnyUnitMoved;
        }

        

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            return base.GetEffectDescription(unit, level);
        }
    }
}