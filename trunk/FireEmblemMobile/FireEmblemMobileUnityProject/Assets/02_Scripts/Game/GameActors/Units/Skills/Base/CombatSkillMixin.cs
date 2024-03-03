using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.Manager;
using Game.Systems;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Base
{
    [CreateAssetMenu(menuName = "GameData/Skills/CombatSkillMixin", fileName = "CombatMixin")]
    public class CombatSkillMixin:SkillMixin
    {
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
            return hpCostPerLevel[level]+ Player.Player.Instance.Modifiers.CombatSkillCost;
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
            if (base.Activate(enemy))
            {
                if (activated)
                    return;
                if (reduceHp)
                {
                    user.Hp -= GetHpCost(skill.level);
                    Uses--;
                }
                this.unit = user;
                this.target = enemy;
                this.activatedLevel = skill.Level;
                user.BattleComponent.BattleStats.SetPreventDoubleAttacks(preventDouble);
            }
        }
       
        public void Deactivate()
        {
            if (!activated)
                return;
            if(unit!=null)
                unit.BattleComponent.BattleStats.SetPreventDoubleAttacks(false);
            Deactivate(skill.owner);
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