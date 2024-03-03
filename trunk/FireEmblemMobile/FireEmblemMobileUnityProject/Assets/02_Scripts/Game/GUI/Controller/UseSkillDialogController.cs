using System;
using Game.GameActors.Units.Skills.Base;
using TMPro;
using UnityEngine;

namespace Game.GUI.Controller
{
    public class UseSkillDialogController : OKCancelDialogController
    {
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private SkillUI skillUI;
        public void Show(Skill skill, Action action, Action onBack=null)
        {
        
            skillName.text = skill.Name;
            skillUI.SetSkill(skill, true, false,false,false,false);
            base.Show(skill.Description, action, onBack);

     
        }
    }
}