using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameInput;
using Game.Manager;
using Game.Mechanics;
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
            
            if (skill is ActivatedSkill)
            {
                var selectedCharacter = (Unit)ServiceProvider.Instance.GetSystem<UnitSelectionSystem>().SelectedCharacter;
                new GameplayCommands().SelectSkill(skill); //TODO Show Skill Description when using or before with a dialogbox
                new GameplayCommands().ExecuteInputActions(null);
            }
            else
            {
                Debug.Log("Show Skill Tooltip!"+transform.position);
                ToolTipSystem.ShowSkill(skill, transform.position);
            }
        }
    }
}