using System;
using Game.GameActors.Units.Skills;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseSkillDialogController : OKCancelDialogController
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private SkillUI skillUI;
    public void Show(Skill skill, Action action)
    {
        
        skillName.text = skill.Name;
        skillUI.SetSkill(skill, true, false,false,false,false);
        base.Show(skill.Description, action);

     
    }
}