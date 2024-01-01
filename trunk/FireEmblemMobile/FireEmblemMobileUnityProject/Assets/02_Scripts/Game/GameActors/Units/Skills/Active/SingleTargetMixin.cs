using System;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.Manager;
using Game.Map;
using LostGrace;
using UnityEngine;
using UnityEngine.Serialization;

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
        [FormerlySerializedAs("conditions")] public List<SingleTargetCondition> singleTargetConditions;
        public SingleTargetType targetType;
        [field: SerializeField] private int minRange = 0;
        [field: SerializeField] private int[] range;
        [SerializeField]private bool sameAsAttackRange = false;
        public bool confirmPosition = true;

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
                       var go= GameObject.Instantiate(AnimationObject, spawnAnimationAtCaster?user.GameTransformManager.GetCenterPosition():target.GameTransformManager.GetCenterPosition(), Quaternion.identity, null);
                       var castPosition=go.GetComponentInChildren<CastPosition>();
                       if(castPosition!=null)
                           castPosition.transform.position = user.GameTransformManager.GetCenterPosition();
                    }
                }, effectDelay);
            }
            MonoUtility.DelayFunction(() =>
            {
                Activate(target);
                // Debug.Log("ACTIVATE SINGLE TARGET MIXIN");
                if (target != null && skill.skillTransferData != null)
                {
                    // Debug.Log("Set SkilltransferData");
                    skill.skillTransferData.data = (object)target;
                }
            }, logicDelay);
            base.PayActivationCost();
        }
       
        public int GetRange(int level)
        {
            var blessing = GetBlessing(skill.owner);
            int extraRange = 0;
            if (blessing != null)
                extraRange = synergies[blessing].extraRange;
            if (!sameAsAttackRange)
                return range[level]+extraRange;
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
                    // Debug.Log("Check Is In Range: "+unit.name);
                    if (IsInRange(user,unit))
                    {
                        // Debug.Log("Is In Range: "+unit.name);
                        if (CanTarget(user, unit))
                        {
                            // Debug.Log("Can Target: "+unit.name);
                           // Debug.Log(GetRange(skill.Level)+ " "+userPos+" "+targetUnitPos+" "+unit.name+" "+Math.Ceiling(Vector2.Distance(userPos, targetUnitPos)));
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
            foreach(var condition in singleTargetConditions)
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

        public void HideTargets(Unit user)
        {
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            var tiles = gridSystem.Tiles;
            var targets = GetTargets(user);
            gridSystem.HideMoveRange();
            gridSystem.ShowCastRange(user, GetRange(skill.Level), GetMinRange(skill.Level));
            var blessing = GetBlessing(user);
            foreach (var target in targets)
            {
                tiles[target.GridComponent.GridPosition.X, target.GridComponent.GridPosition.Y].SetCastCursorMaterial(EffectType.Bad, user.Faction.Id);
                if (blessing != null)
                {
                    foreach (SkillEffectMixin effect in synergies[GetBlessing(user)].skillEffectMixins)
                    {
                        HideSkillEffectsPreview(effect, target, user);
                    }

                    if (synergies[GetBlessing(user)].replacesOtherConditions)
                        continue;
                }
                foreach (SkillEffectMixin effect in skillEffectMixins)
                {
                    HideSkillEffectsPreview(effect, target, user);
                }
                
           }
        }
        public void ShowTargets(Unit user)
        {
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            var tiles = gridSystem.Tiles;
            var targets = GetTargets(user);
            gridSystem.HideMoveRange();
            gridSystem.ShowCastRange(user, GetRange(skill.Level), GetMinRange(skill.Level));
            var blessing = GetBlessing(user);
            // Debug.Log("Blessing; "+blessing);
            foreach (var target in targets)
            {
                
                tiles[target.GridComponent.GridPosition.X, target.GridComponent.GridPosition.Y].SetCastCursorMaterial(effectType, user.Faction.Id);
                if (blessing != null)
                {
                    foreach (SkillEffectMixin effect in synergies[blessing].skillEffectMixins)
                    {
                        ShowSkillEffectsPreview(effect, target,user,skill.level);
                    }

                    if (synergies[blessing].replacesOtherConditions)
                        continue;
                }

                foreach (SkillEffectMixin effect in skillEffectMixins)
                {
                    ShowSkillEffectsPreview(effect, target,user,skill.level);
                }
                
            }

        }

        

        public int GetDamageDone(Unit user, Unit target)
        {
            var blessing = GetBlessing(user);
            if (blessing != null)
            {
                foreach (SkillEffectMixin effect in synergies[GetBlessing(user)].skillEffectMixins)
                {
                    if (effect is DamageSkillEffectMixin damageSkillEffectMixin)
                        return damageSkillEffectMixin.GetDamageDealtToTarget(user, target, skill.level);
                }

                if (synergies[GetBlessing(user)].replacesOtherConditions)
                    return 0;
            }
            foreach (SkillEffectMixin effect in skillEffectMixins)
            {
                if (effect is DamageSkillEffectMixin damageSkillEffectMixin)
                    return damageSkillEffectMixin.GetDamageDealtToTarget(user, target, skill.level);
                if (effect is OverrideSkillEffectMixin overrideSkillEffect)
                    return overrideSkillEffect.GetDamageDealtToTarget(user, target, skill.level);
            }
            return 0;
        }
        public int GetHealingDone(Unit user, Unit target)
        {
            var blessing = GetBlessing(user);
            if (blessing != null)
            {
                foreach (SkillEffectMixin effect in synergies[GetBlessing(user)].skillEffectMixins)
                {
                    if (effect is HealEffect healMixin)
                        return healMixin.GetHealAmount(user, target,skill.level);
                }

                if (synergies[GetBlessing(user)].replacesOtherConditions)
                    return 0;
            }
            foreach (SkillEffectMixin effect in skillEffectMixins)
            {
                if (effect is HealEffect healMixin)
                    return healMixin.GetHealAmount(user, target,skill.level);
                if (effect is OverrideSkillEffectMixin overrideSkillEffect)
                    return overrideSkillEffect.GetHealAmount(user, target, skill.level);

            }
            return 0;
        }
    }
}