using System;
using System.Collections.Generic;
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
    public abstract class SingleTargetMixin : ActiveSkillMixin
    {
        public SingleTargetType targetType;
        [field: SerializeField] private int[] range;

        public abstract void Activate(Unit user, Unit target);
        

        public List<Unit> GetTargets(Unit user)
        {
            var userPos = user.GridComponent.GridPosition.AsVector();
            var targetList = new List<Unit>();
            foreach (var faction in GridGameManager.Instance.FactionManager.Factions)
            {
                foreach (var unit in faction.FieldedUnits)
                {
                    var targetUnitPos = unit.GridComponent.GridPosition.AsVector();
                    if (((int)Vector2.Distance(userPos, targetUnitPos)) <= range[skill.Level])
                    {
                        if (CanTarget(user, unit))
                        {
                            targetList.Add(unit);
                        }
                    }
                }
            }
            return targetList;
        }

        public bool CanTarget(Unit user, Unit target)
        {
            if (targetType == SingleTargetType.Any)
                return true;
            if (targetType == SingleTargetType.Enemy)
            {
                return user.Faction.IsOpponentFaction(target.Faction);
            }
            return !user.Faction.IsOpponentFaction(target.Faction);
        }
     
        

        //
        // protected SingleTargetMixin( int[] maxUses, int[] hpCost,GameObject animationObject) : base(maxUses,hpCost,animationObject)
        // {
        // }
    }
}