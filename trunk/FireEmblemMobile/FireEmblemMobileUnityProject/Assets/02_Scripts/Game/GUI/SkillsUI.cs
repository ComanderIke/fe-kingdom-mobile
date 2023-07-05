using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public class SkillsUI : MonoBehaviour
    {
        [SerializeField] private GameObject skillButtonPrefab;
        [SerializeField] private GameObject activeSkillButtonPrefab;


        private List<GameObject> instantiatedButtons;

        public void Show(List<Skill> skills)
        {
            transform.DeleteAllChildren();
            foreach (var skill in skills)
            {
                var go = Instantiate(skill.activeMixin is ActiveSkillMixin?activeSkillButtonPrefab:skillButtonPrefab, transform);
                go.GetComponent<SkillUI>().SetSkill(skill, false);
            }
            // var addButtonGo = Instantiate(addSkillButtonPrefab, transform);
            // addButtonGo.GetComponent<AddSkillButton>().Show(skillPoints);
        }
    }
}
