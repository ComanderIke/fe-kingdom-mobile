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
        [SerializeField] private GameObject addSkillButtonPrefab;
       

        private List<GameObject> instantiatedButtons;

        public void Show(List<Skill> skills, int skillPoints)
        {
            transform.DeleteAllChildren();
            foreach (var skill in skills)
            {
                var go = Instantiate(addSkillButtonPrefab, transform);
                go.GetComponent<SkillUI>().SetSkill(skill);
            }
            var addButtonGo = Instantiate(addSkillButtonPrefab, transform);
            addButtonGo.GetComponent<AddSkillButton>().Show(skillPoints);
        }
    }
}
