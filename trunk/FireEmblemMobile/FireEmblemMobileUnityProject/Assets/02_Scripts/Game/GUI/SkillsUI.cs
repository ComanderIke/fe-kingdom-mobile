using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.Manager;
using UnityEngine;

namespace LostGrace
{
    public class SkillsUI : MonoBehaviour
    {
        [SerializeField] private GameObject skillButtonPrefab;
        [SerializeField] private GameObject activeSkillButtonPrefab;
        [SerializeField] private GameObject combatSkillButtonPrefab;
   

        private List<SkillUI> instantiatedButtons;
        private Unit unit;
        [SerializeField]private bool showToolTips;
        [SerializeField]private bool interactable = true;
        public void Show(Unit unit, bool showDeleteIfFull=false)
        {
            this.unit = unit;
            
            gameObject.SetActive(true);
           // Debug.Log("SKill UI Show");
            transform.DeleteAllChildren();
            instantiatedButtons = new List<SkillUI>();
            foreach (var skill in unit.SkillManager.Skills)
            {
              //  Debug.Log("instantiate: "+skill.Name+" "+transform.parent.parent.name);
                InstantiateSkill(skill, showDeleteIfFull);
            }
            // var addButtonGo = Instantiate(addSkillButtonPrefab, transform);
            // addButtonGo.GetComponent<AddSkillButton>().Show(skillPoints);
        }

        void InstantiateSkill(Skill skill, bool showDeleteIfFull)
        {
            var prefab = skillButtonPrefab;
            if (skill.activeMixins.Count > 0)
                prefab = activeSkillButtonPrefab;
            else if (skill.CombatSkillMixin != null)
                prefab = combatSkillButtonPrefab;
            var go = Instantiate(prefab, transform);
            var skillUI = go.GetComponent<SkillUI>();
            instantiatedButtons.Add(skillUI);
            bool canAffordHPCost =
                skill.FirstActiveMixin != null && unit.Hp > skill.FirstActiveMixin.GetHpCost(skill.level) ||
                skill.CombatSkillMixin != null && unit.Hp > skill.CombatSkillMixin.GetHpCost(skill.level);
            bool hasUses = skill.FirstActiveMixin != null && skill.FirstActiveMixin.Uses > 0 ||
                           skill.CombatSkillMixin != null && skill.CombatSkillMixin.Uses > 0;
            bool hasSynergy = false;
            if (unit.Blessing != null)
            {
                foreach (var synergy in skill.GetSynergies())
                {
                    if (synergy.Key.god == unit.Blessing.God)
                    {
                        hasSynergy = true;
                        break;
                    }

                }
            }

            skillUI.SetSkill(skill, false, hasSynergy, canAffordHPCost,hasUses,showToolTips, interactable);
            if(showDeleteIfFull)
                skillUI.ShowDelete();
            skillUI.OnDeleteClicked += DeleteClicked;
        }

        void DeleteClicked(SkillUI skill)
        {
            ServiceProvider.Instance.GetSystem<SkillSystem>().RemoveSkill(unit,skill.Skill);
        }
        public void ShowDelete()
        {
            foreach (var skill in instantiatedButtons)
            {
                skill.ShowDelete();
            }
        }

        public void AddSkill(Skill chooseSkill1Skill)
        {
            InstantiateSkill(chooseSkill1Skill, false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
        }
    }
}
