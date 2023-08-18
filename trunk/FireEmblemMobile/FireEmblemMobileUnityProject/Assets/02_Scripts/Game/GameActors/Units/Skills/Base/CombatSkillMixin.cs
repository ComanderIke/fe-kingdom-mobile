using System.Collections.Generic;
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
        public int[] maxUsesPerLevel;
        public int[] hpCostPerLevel;
        private Unit target;
        private Unit unit;
        private int activatedLevel = -1;
        public bool preventDouble = true;
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
                list.AddRange(skillEffect.GetEffectDescription(level));
            }
            return list;
        }

        public void ActivateForNextCombat()
        {
            ServiceProvider.Instance.GetSystem<BattleSystem>().AddAttackerActivatedSkills(this);
        }

        public void Activate(Unit user, Unit enemy)
        {
            this.unit = user;
            this.target = enemy;
            this.activatedLevel = skill.Level;
            user.BattleComponent.BattleStats.SetPreventDoubleAttacks(preventDouble);
            foreach (var skilleffect in skillEffectMixins)
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
                    if(unitTargetSkillEffectMixin.TargetIsCaster)
                        unitTargetSkillEffectMixin.Activate(user,user, skill.Level);
                    else
                        unitTargetSkillEffectMixin.Activate(enemy,user, skill.Level);
                }
            }
           // Debug.Log("UpdateBattlePreview");
        }

        public void Deactivate()
        {
            unit.BattleComponent.BattleStats.SetPreventDoubleAttacks(false);
            foreach (var skilleffect in skillEffectMixins)
            {
                if (skilleffect is SelfTargetSkillEffectMixin selfTargetSkillEffectMixin)
                {
                    selfTargetSkillEffectMixin.Deactivate(unit, activatedLevel);
                }
                if (skilleffect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                {
                    unitTargetSkillEffectMixin.Deactivate(target,unit, activatedLevel);
                }
            }
        }
        public void DeactivateForNextCombat()
        {
            ServiceProvider.Instance.GetSystem<BattleSystem>().RemoveAttackerActivatedSkills(this);
           
            Debug.Log("UpdateBattlePreview");
            //UpdateBattlePreview
        }


    }
}