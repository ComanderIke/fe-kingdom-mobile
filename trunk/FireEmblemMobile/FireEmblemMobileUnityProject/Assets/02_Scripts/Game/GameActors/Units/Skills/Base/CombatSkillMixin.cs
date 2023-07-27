using System.Collections.Generic;
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

        public void ActivateForNextCombat(Unit user, Unit enemy)
        {
            foreach (var skilleffect in skillEffectMixins)
            {
                if (skilleffect is SelfTargetSkillEffectMixin selfTargetSkillEffectMixin)
                {
                    selfTargetSkillEffectMixin.Activate(user, skill.Level);
                }
                if (skilleffect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                {
                    unitTargetSkillEffectMixin.Activate(enemy,user, skill.Level);
                }
            }
            Debug.Log("UpdateBattlePreview");
            //UpdateBattlePreview
        }
        public void DeactivateForNextCombat(Unit user, Unit enemy)
        {
            foreach (var skilleffect in skillEffectMixins)
            {
                if (skilleffect is SelfTargetSkillEffectMixin selfTargetSkillEffectMixin)
                {
                    selfTargetSkillEffectMixin.Deactivate(user, skill.Level);
                }
                if (skilleffect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                {
                    unitTargetSkillEffectMixin.Deactivate(enemy,user, skill.Level);
                }
            }
            Debug.Log("UpdateBattlePreview");
            //UpdateBattlePreview
        }


    }
}