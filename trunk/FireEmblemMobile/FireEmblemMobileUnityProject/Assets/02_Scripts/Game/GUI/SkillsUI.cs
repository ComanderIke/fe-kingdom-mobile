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


        private List<GameObject> instantiatedButtons;

        public void Show(List<Skill> skills)
        {
            transform.DeleteAllChildren();
            foreach (var skill in skills)
            {
                var go = Instantiate(skillButtonPrefab, transform);
                go.GetComponent<SkillUI>().SetSkill(skill);
            }
            // var addButtonGo = Instantiate(addSkillButtonPrefab, transform);
            // addButtonGo.GetComponent<AddSkillButton>().Show(skillPoints);
        }
    }
}
