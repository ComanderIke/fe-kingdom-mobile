using System;
using System.Collections.Generic;
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
        [field: SerializeField] private int[] range;

        public List<SkillEffectMixin> SkillEffectMixins;

        public void Activate(Unit user, Unit target)
        {
            if(AnimationObject!=null)
                GameObject.Instantiate(AnimationObject, target.GameTransformManager.Transform.position, Quaternion.identity, null);
            foreach (SkillEffectMixin effect in SkillEffectMixins)
            {
                effect.Activate(target, skill.Level);
            }
            Debug.Log("ACTIVATE SINGLE TARGET MIXIN");
            skill.skillTransferData.data = (object)target;
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
                    if (IsInRange(user,unit))
                    {
                       
                        if (CanTarget(user, unit))
                        {
                           Debug.Log(range[skill.Level]+ " "+userPos+" "+targetUnitPos+" "+unit.name+" "+Math.Ceiling(Vector2.Distance(userPos, targetUnitPos)));
                            targetList.Add(unit);
                        }
                    }
                }
            }
            return targetList;
        }

        public bool IsInRange(Unit user, Unit target)
        {
            return ((int)Math.Ceiling(Vector2.Distance(user.GridComponent.GridPosition.AsVector(),
                target.GridComponent.GridPosition.AsVector()))) <= range[skill.Level];
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

        public int GetRange(int level)  => range[level];

        public void ShowTargets(Unit user)
        {
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            var tiles = gridSystem.Tiles;
            var targets = GetTargets(user);
            gridSystem.HideMoveRange();
            gridSystem.ShowCastRange(user, range[skill.Level]);
            foreach (var target in targets)
            {
                tiles[target.GridComponent.GridPosition.X, target.GridComponent.GridPosition.Y].SetCastCursorMaterial(EffectType.Bad, user.Faction.Id);
            }

        }
    }
}