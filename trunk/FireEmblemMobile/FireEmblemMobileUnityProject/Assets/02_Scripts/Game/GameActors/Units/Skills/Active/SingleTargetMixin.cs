using System;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.Manager;
using Game.Map;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{

    public enum SingleTargetType
    {
        Ally,
        Enemy,
        Any
    }

    public abstract class Condition:ScriptableObject
    {
        
    }
    public abstract class SingleTargetCondition :Condition
    {
        public abstract bool CanTarget(Unit caster, Unit target);

    }
    public abstract class SelfTargetCondition :Condition
    {
        public abstract bool CanTarget(Unit caster);

    }

    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Active/SingleTarget", fileName = "SingleTargetMixin")]
    public class SingleTargetMixin : ActiveSkillMixin
    {
        public List<SingleTargetCondition> conditions;
        public SingleTargetType targetType;
        [field: SerializeField] private int minRange = 0;
        [field: SerializeField] private int[] range;
        [SerializeField]private bool sameAsAttackRange = false;
        public List<SkillEffectMixin> SkillEffectMixins;
        public void Activate(Unit user, Unit target)
        {
            if (AnimationObject != null)
            {
                MonoUtility.DelayFunction(() =>
                {
                    if (useScreenPosition)
                    {
                        var go =GameObject.Instantiate(AnimationObject,
                            Camera.main.WorldToScreenPoint(target.GameTransformManager.GetCenterPosition()),
                            Quaternion.identity, null);
                        go.GetComponentInChildren<CastPosition>().transform.position =
                            Camera.main.WorldToScreenPoint(user.GameTransformManager.GetCenterPosition());

                    }
                    else
                    {
                       var go= GameObject.Instantiate(AnimationObject, target.GameTransformManager.Transform.position, Quaternion.identity, null);
                       go.GetComponentInChildren<CastPosition>().transform.position =
                           user.GameTransformManager.GetCenterPosition();
                    }
                }, effectDelay);
             
            }

            MonoUtility.DelayFunction(() =>
            {
                foreach (SkillEffectMixin effect in SkillEffectMixins)
                {
                    if (effect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                        unitTargetSkillEffectMixin.Activate(target, user, skill.Level);
                }

                Debug.Log("ACTIVATE SINGLE TARGET MIXIN");
                if (target != null && skill.skillTransferData != null)
                {
                    Debug.Log("Set SkilltransferData");
                    skill.skillTransferData.data = (object)target;
                }
            }, logicDelay);
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
            foreach(var condition in conditions)
            {
                if (!condition.CanTarget(user, target))
                    return false;
            }
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
                foreach (SkillEffectMixin effect in SkillEffectMixins)
                {
                    if (effect is DamageSkillEffectMixin dmgMixin)
                        dmgMixin.ShowDamagePreview(target, user, skill.Level);
                }
                
                tiles[target.GridComponent.GridPosition.X, target.GridComponent.GridPosition.Y].SetCastCursorMaterial(EffectType.Bad, user.Faction.Id);
            }

        }

        
        public List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            foreach (var skillEffect in SkillEffectMixins)
            {
                var skillEffectList = skillEffect.GetEffectDescription(level);
                if(skillEffectList!=null)
                list.AddRange(skillEffectList);
            }
            return list;
        }
    }
}