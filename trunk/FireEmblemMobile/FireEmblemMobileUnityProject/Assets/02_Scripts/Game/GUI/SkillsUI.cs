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
        public void Show(Unit unit, bool showDeleteIfFull=false)
        {
            this.unit = unit;
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
            skillUI.SetSkill(skill, false, unit.Blessing!=null);
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
    }
}
