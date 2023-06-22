using System;
using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseSkillDialogController : OKCancelDialogController
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private TextMeshProUGUI skillUses;
    public void Show(ActivatedSkill skill, Action action)
    {
        skillIcon.sprite = skill.Icon;
        skillName.text = skill.Name;
        skillCost.text = ""+skill.hpCost;
        skillUses.text = skill.currentUses + "/" + skill.maxUses;
        base.Show(skill.Description, action);

     
    }
}