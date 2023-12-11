using System;
using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Units.Skills.Passive;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class SkillMixin : ScriptableObject
    {
        public const int MAXLEVEL = 5;
        [NonSerialized]public Skill skill;
        [SerializeField]public SerializableDictionary<BlessingBP,SynergieEffects> synergies;
        [SerializeField] private ConditionBigPackage conditionBigPackage;
        [SerializeField]protected List<SkillEffectMixin> skillEffectMixins;
        private bool activated = false;
        private bool blessingActivated = false;
        protected bool bound = false;

        public virtual void BindToUnit(Unit unit, Skill skill)
        {
            this.skill = skill;
            bound = true;
            if(skill.skillTransferData!=null)
                skill.skillTransferData.data = null;
        }

        private BlessingBP blessing;
        protected void UpdateContext(Unit target=null, Tile tile =null)
        {
            blessing = GetBlessing(skill.owner);
            bool replaceConditions = false;
            bool blessingConditionValid = false;
            bool replaceEffects = false;
            bool baseConditionValid = conditionBigPackage.Valid(skill.owner, target, tile);
            
            if (blessing != null)
            {
                replaceEffects = synergies[blessing].replacesOtherEffects;
                replaceConditions = synergies[blessing].replacesOtherConditions;
                
                blessingConditionValid = synergies[blessing].conditionManager.Valid(skill.owner, target, tile);
                switch (synergies[blessing].compareTypeWithExistingConditions)
                {
                    case ConditionCompareType.OR:
                        baseConditionValid = baseConditionValid || blessingConditionValid;break;
                    case ConditionCompareType.AND:
                        baseConditionValid = baseConditionValid && blessingConditionValid;break;
                    case ConditionCompareType.XOR:
                        baseConditionValid = (baseConditionValid || blessingConditionValid)&& !(baseConditionValid&&blessingConditionValid);break;
                }
                
            }
            if (replaceConditions?blessingConditionValid:baseConditionValid){
                CheckSkillEffectsAndSynergies(skill.owner, target, skill.level, replaceEffects, blessingConditionValid);
            }
            else 
            {
                CheckDeactivateSkillEffectsAndSynergies(skill.owner, target, skill.level, replaceEffects);
            }
        }

        private void CheckSkillEffectsAndSynergies(Unit user, Unit target, int level,bool replaceEffects, bool blessingConditionValid)
        {
            if (activated)
                return;
            activated = true;
            if (blessingConditionValid)
            {
                blessingActivated = true;
                foreach (SkillEffectMixin effect in synergies[blessing].skillEffectMixins)
                {
                    ActivateSkillEffects(effect, user, target, level);
                }
            }
            if (replaceEffects) return;
            foreach (var skillEffect in skillEffectMixins)
            {
                ActivateSkillEffects(skillEffect, user, target, level);
            }
        }

        private void CheckDeactivateSkillEffectsAndSynergies(Unit user, Unit target, int level, bool replaceEffects)
        {
            if (!activated)
                return;
            activated = false;
            if (blessing != null)
            {
                if (blessingActivated)
                {
                    foreach (SkillEffectMixin effect in synergies[blessing].skillEffectMixins)
                    {
                        DeactivateSkillEffects(effect, user, target, level);
                    }
                    blessingActivated = false;
                }
            }
            if (replaceEffects) return;
            foreach (var skillEffect in skillEffectMixins)
            {
                DeactivateSkillEffects(skillEffect, user, target, level);
            }
        }
        public void ActivateSkillEffects(SkillEffectMixin skillEffect, Unit user, Unit target, int level)
        {
            if (skillEffect is SelfTargetSkillEffectMixin stm)
            {
                stm.Activate(user, level);
            }
            if (skillEffect is UnitTargetSkillEffectMixin uts)
            {
                uts.Activate(target, user,level);
            }
        }
        public void DeactivateSkillEffects(SkillEffectMixin skillEffect, Unit user, Unit target, int level)
        {
            if (skillEffect is SelfTargetSkillEffectMixin stm)
            {
                stm.Deactivate(user, level);
            }
            if (skillEffect is UnitTargetSkillEffectMixin uts)
            {
                uts.Deactivate(target, user,level);
            }
        }

        public virtual void UnbindFromUnit(Unit unit, Skill skill)
        {
            if(skill.skillTransferData!=null)
                skill.skillTransferData.data = null;
            this.skill = null;//TODO is this right?
            bound = false;

        }
        protected BlessingBP GetBlessing(Unit user)
        {
            if (user.Blessing == null)
                return null;
            foreach (var key in synergies.Keys)
            {
                if (key.Name == user.Blessing.Name)
                    return key;
            }

            return null;
        }
        protected bool HasSynergy(Unit user)
        {
            foreach (var key in synergies.Keys)
            {
                if (key.Name == user.Blessing.Name)
                    return true;
            }

            return false;
        }
       
    }
}