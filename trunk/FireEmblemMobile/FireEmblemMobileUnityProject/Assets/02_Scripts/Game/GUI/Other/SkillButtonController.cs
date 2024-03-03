using Game.GameActors.Units.Skills.Base;
using Game.GameInput.Buttons;
using Game.GameInput.GameplayCommands;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.GUI.Other
{
    public class SkillButtonController:MonoBehaviour
    {
        [FormerlySerializedAs("skill")] public Skill skill;
        public Image Icon;
        public TextMeshProUGUI text;
        public SelectionUI selectionUI;

        public void SetSkill(Skill skill, SelectionUI selectionUI)
        {
            this.skill = skill;
            UpdateValues();
            this.selectionUI = selectionUI;
        }

        private void UpdateValues()
        {
            Icon.sprite = skill.Icon;
            text.text = skill.Name;
        }

        public void Clicked()
        {
            new GameplayCommands().SelectSkill(skill);
            //selectionUI.CloseSkillsClicked();
        }
    }
}