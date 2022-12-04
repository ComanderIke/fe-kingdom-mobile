using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class SkillUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        private Skill skill;
        public void SetSkill(Skill skill)
        {
            icon.sprite = skill.Icon;
            this.skill = skill;
        }

        public void Clicked()
        {
            //ToolTipSystem.ShowSkill(skill, transform.position);
        }
    }
}