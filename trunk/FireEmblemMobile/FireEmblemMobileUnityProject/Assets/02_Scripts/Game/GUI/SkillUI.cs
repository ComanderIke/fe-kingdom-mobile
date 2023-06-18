using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class SkillUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        private Skill skill;
        [SerializeField] private TextMeshProUGUI hpCost;
        [SerializeField] private TextMeshProUGUI uses;
        public void SetSkill(Skill skill)
        {
            icon.sprite = skill.Icon;
            this.skill = skill;
            if (skill is ActivatedSkill activatedSkill)
            {
                uses.text = activatedSkill.currentUses + "/" +
                            activatedSkill.maxUses;
                hpCost.text = ""+activatedSkill.hpCost;
            }
        
         
        }

        public void Clicked()
        {
            ToolTipSystem.ShowSkill(skill, transform.position);
        }
    }
}