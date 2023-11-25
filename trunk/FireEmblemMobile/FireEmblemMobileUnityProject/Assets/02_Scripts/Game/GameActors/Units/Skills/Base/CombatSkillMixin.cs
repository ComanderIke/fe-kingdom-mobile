using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.Manager;
using Game.Mechanics;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/CombatSkillMixin", fileName = "CombatMixin")]
    public class CombatSkillMixin:SkillMixin
    {
        
        [SerializeField]protected List<SkillEffectMixin> skillEffectMixins;
        [SerializeField]public SerializableDictionary<BlessingBP,List<SkillEffectMixin>> synergies;
        public int[] maxUsesPerLevel;
        public int[] hpCostPerLevel;
        private Unit target;
        private Unit unit;
        private int activatedLevel = -1;
        public bool preventDouble = true;
        private bool activated = false;
        [HideInInspector]public int Uses { get; set; }
      

        public int GetMaxUses(int level)
        {
            return maxUsesPerLevel[level];
        }
        public int GetHpCost(int level)
        {
            return hpCostPerLevel[level];
        }
        //[SerializeField]private EffectDescription[] effectDescriptionsPerLevel;

        public virtual List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();

            foreach (var skillEffect in skillEffectMixins)
            {
                list.AddRange(skillEffect.GetEffectDescription(unit, level));
            }
            return list;
        }

        public void ActivateForNextCombat()
        {
            ServiceProvider.Instance.GetSystem<BattleSystem>().AddAttackerActivatedSkills(this);
        }

        public void Activate(Unit user, Unit enemy, bool reduceHp=false)
        {
            if (activated)
                return;
            if (reduceHp)
            {
                user.Hp -= hpCostPerLevel[skill.level];
                Uses--;
            }

            activated = true;
            this.unit = user;
            this.target = enemy;
            this.activatedLevel = skill.Level;
            user.BattleComponent.BattleStats.SetPreventDoubleAttacks(preventDouble);
            foreach (var skilleffect in skillEffectMixins)
            {
                ActivateSkillEffect(user, enemy, skilleffect);
            }

            var key = GetKey(unit);
            if (key != null)
            {
                foreach (var skillEffect in synergies[key])
                {
                    ActivateSkillEffect(user, enemy, skillEffect);
                }
            }
            
        }

        private void ActivateSkillEffect(Unit user, Unit enemy, SkillEffectMixin skilleffect)
        {
            if (skilleffect is SelfTargetSkillEffectMixin selfTargetSkillEffectMixin)
            {
                //Debug.Log(skill.Name);
                // Debug.Log(user.Name);
                // Debug.Log(skilleffect);
                selfTargetSkillEffectMixin.Activate(user, skill.Level);
            }

            if (skilleffect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
            {
                if (unitTargetSkillEffectMixin.TargetIsCaster)
                    unitTargetSkillEffectMixin.Activate(user, user, skill.Level);
                else
                    unitTargetSkillEffectMixin.Activate(enemy, user, skill.Level);
            }
        }


        private BlessingBP GetKey(Unit user)
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
        private bool HasSynergy(Unit user)
        {
            foreach (var key in synergies.Keys)
            {
                if (key.Name == user.Blessing.Name)
                    return true;
            }

            return false;
        }
        public void Deactivate()
        {
            if (!activated)
                return;
            activated = false;
            unit.BattleComponent.BattleStats.SetPreventDoubleAttacks(false);
            foreach (var skilleffect in skillEffectMixins)
            {
                DeactivateSkillEffects(skilleffect);
            }
            var key = GetKey(unit);
            if (key != null)
            {
                foreach (var skillEffect in synergies[key])
                {
                    DeactivateSkillEffects(skillEffect);
                }
            }
        }

        private void DeactivateSkillEffects(SkillEffectMixin skilleffect)
        {
            if (skilleffect is SelfTargetSkillEffectMixin selfTargetSkillEffectMixin)
            {
                selfTargetSkillEffectMixin.Deactivate(unit, activatedLevel);
            }

            if (skilleffect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
            {
                unitTargetSkillEffectMixin.Deactivate(target, unit, activatedLevel);
            }
        }

        public void DeactivateForNextCombat()
        {
            ServiceProvider.Instance.GetSystem<BattleSystem>().RemoveAttackerActivatedSkills(this);
           
            Debug.Log("UpdateBattlePreview");
            //UpdateBattlePreview
        }


        public void RefreshUses(int skillLevel)
        {
            Uses = GetMaxUses(skillLevel);
        }
    }
}