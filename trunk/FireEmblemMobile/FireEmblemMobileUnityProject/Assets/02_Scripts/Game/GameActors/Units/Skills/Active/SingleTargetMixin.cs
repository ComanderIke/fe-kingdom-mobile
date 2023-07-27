using System;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.Grid;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{

    public enum SingleTargetType
    {
        Ally,
        Enemy,
        Any
    }

    
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Active/SingleTarget", fileName = "SingleTargetMixin")]
    public class SingleTargetMixin : ActiveSkillMixin
    {
        public SingleTargetType targetType;
        [field: SerializeField] private int minRange = 0;
        [field: SerializeField] private int[] range;
        [SerializeField]private bool sameAsAttackRange = false;
        public List<SkillEffectMixin> SkillEffectMixins;

        public void Activate(Unit user, Unit target)
        {
            if(AnimationObject!=null)
                GameObject.Instantiate(AnimationObject, target.GameTransformManager.Transform.position, Quaternion.identity, null);
            foreach (SkillEffectMixin effect in SkillEffectMixins)
            {
                if(effect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                    unitTargetSkillEffectMixin.Activate(target, user, skill.Level);
            }
            Debug.Log("ACTIVATE SINGLE TARGET MIXIN");
            if(target!=null&& skill.skillTransferData!=null)
                skill.skillTransferData.data = (object)target;
        }


        public int GetRange(int level)
        {
            
            if (!sameAsAttackRange)
                return range[level];
            return skill.owner.AttackRanges.Max();
            
        }
        public int GetMinRange(int level)
        {
            
            if (!sameAsAttackRange)
                return minRange;
            return skill.owner.AttackRanges.Min();
            
        }

        public List<Unit> GetTargets(Unit user)
        {
            var userPos = user.GridComponent.GridPosition.AsVector();
            var targetList = new List<Unit>();
            foreach (var faction in GridGameManager.Instance.FactionManager.Factions)
            {
                foreach (var unit in faction.FieldedUnits)
                {
                    var targetUnitPos = unit.GridComponent.GridPosition.AsVector();
                    Debug.Log("Check Is In Range: "+unit.name);
                    if (IsInRange(user,unit))
                    {
                        Debug.Log("Is In Range: "+unit.name);
                        if (CanTarget(user, unit))
                        {
                            Debug.Log("Can Target: "+unit.name);
                           Debug.Log(GetRange(skill.Level)+ " "+userPos+" "+targetUnitPos+" "+unit.name+" "+Math.Ceiling(Vector2.Distance(userPos, targetUnitPos)));
                            targetList.Add(unit);
                        }
                    }
                }
            }
            return targetList;
        }

        public bool IsInRange(Unit user, Unit target)
        {
            int distance = (int)Math.Ceiling(Vector2.Distance(user.GridComponent.GridPosition.AsVector(),
                target.GridComponent.GridPosition.AsVector()));
            return distance <= GetRange(skill.Level)&& distance >= GetMinRange(skill.Level);
        }

        public bool CanTarget(Unit user, Unit target)
        {
            if (target == user)
                return false;
            if (targetType == SingleTargetType.Any)
                return true;
            if (targetType == SingleTargetType.Enemy)
            {
                return user.Faction.IsOpponentFaction(target.Faction);
            }
            return !user.Faction.IsOpponentFaction(target.Faction);
        }

     

        public void ShowTargets(Unit user)
        {
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            var tiles = gridSystem.Tiles;
            var targets = GetTargets(user);
            gridSystem.HideMoveRange();
            gridSystem.ShowCastRange(user, GetRange(skill.Level), GetMinRange(skill.Level));
            foreach (var target in targets)
            {
                tiles[target.GridComponent.GridPosition.X, target.GridComponent.GridPosition.Y].SetCastCursorMaterial(EffectType.Bad, user.Faction.Id);
            }

        }
    }
}