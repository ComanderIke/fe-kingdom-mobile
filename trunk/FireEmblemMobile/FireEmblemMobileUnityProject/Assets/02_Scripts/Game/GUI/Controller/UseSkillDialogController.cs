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
    public void Show(Skill skill, Action action)
    {
        skillIcon.sprite = skill.Icon;
        skillName.text = skill.Name;
        skillCost.text = ""+0;
        skillUses.text = 0 + "/" + 0;
        base.Show(skill.Description, action);

     
    }
}