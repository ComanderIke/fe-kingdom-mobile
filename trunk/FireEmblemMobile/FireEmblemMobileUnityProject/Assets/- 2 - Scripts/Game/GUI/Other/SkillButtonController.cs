using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController:MonoBehaviour
{
    public Skill skill;
    public Image Icon;
    public TextMeshProUGUI text;

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        UpdateValues();
    }

    private void UpdateValues()
    {
        Icon.sprite = skill.Icon;
        text.text = skill.Name;
    }

    public void Clicked()
    {
        
    }
}