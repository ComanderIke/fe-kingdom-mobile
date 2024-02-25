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
        protected bool activated = false;
        private bool blessingActivated = false;
        protected bool bound = false;
        private BlessingBP blessing;
        private bool replaceConditions = false;
        private bool replaceEffects = false;
        private bool blessingConditionValid = false;
        private bool baseConditionValid = false;
        private List<SkillEffectMixin> instantiatedSkillEffects;

        public void Init()
        {
            instantiatedSkillEffects = new List<SkillEffectMixin>();
            foreach (var skillEffect in skillEffectMixins)
            {
                var instSkillEffect = Instantiate(skillEffect);
                instantiatedSkillEffects.Add(instSkillEffect);
            }
            skillEffectMixins.Clear();
            foreach (var skillEffect in instantiatedSkillEffects)
            {
                skillEffectMixins.Add(skillEffect);
            }
            
        }

        public virtual void BindToUnit(Unit unit, Skill skill)
        {
            this.skill = skill;
            bound = true;
            // if(skill.skillTransferData!=null)
            //     skill.skillTransferData.data = null;
        }

       

        protected bool Activate(Unit target=null)
        {
            CheckConditions(target);
            if (replaceConditions?blessingConditionValid:baseConditionValid){
                CheckSkillEffectsAndSynergies(skill.owner, target, skill.level, replaceEffects, blessingConditionValid);
                return true;
            }

            return false;
        }
        protected void Deactivate(Unit target=null)
        {
            CheckConditions(target);
            CheckDeactivateSkillEffectsAndSynergies(skill.owner, target, skill.level, replaceEffects);
        }

       

        void CheckConditions(Unit target, Tile tile=null)
        {
            blessing = GetBlessing(skill.owner); 
            replaceConditions = false;
            blessingConditionValid = false;
            replaceEffects = false;
            baseConditionValid = conditionBigPackage.Valid(skill.owner, target, tile);
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
        }
        protected void UpdateContext(Unit target=null, Tile tile =null)
        {
            
            CheckConditions(target, tile);
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
        private void ActivateSkillEffects(SkillEffectMixin skillEffect, Unit user, Unit target, int level)
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
        private void DeactivateSkillEffects(SkillEffectMixin skillEffect, Unit user, Unit target, int level)
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
            // if(skill.skillTransferData!=null)
            //     skill.skillTransferData.data = null;
            this.skill = null;//TODO is this right?
            bound = false;

        }
        protected BlessingBP GetBlessing(Unit user)
        {
            if (user == null)
                return null;
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
        public virtual List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();

            foreach (var skillEffect in skillEffectMixins)
            {
                list.AddRange(skillEffect.GetEffectDescription(unit, level));
            }
            return list;
        }
        protected void ShowSkillEffectsPreview(SkillEffectMixin effect, Unit target, Unit user, int level)
        {
            if (effect is DamageSkillEffectMixin dmgMixin)
                dmgMixin.ShowDamagePreview(target, user, level);
            if (effect is OverrideSkillEffectMixin overrideMixin)
                overrideMixin.ShowDamagePreview(target, user, level);
            if (effect is HealEffect healMixin)
                healMixin.ShowHealPreview(target, user, level);

        }

        protected void HideSkillEffectsPreview(SkillEffectMixin effect, Unit target, Unit user)
        {
            if (effect is DamageSkillEffectMixin dmgMixin)
                dmgMixin.HideDamagePreview(target);
            if (effect is OverrideSkillEffectMixin overrideMixin)
                overrideMixin.HideDamagePreview(target);
            if (effect is HealEffect healMixin)
                healMixin.HideHealPreview(target);
        }

    }
}