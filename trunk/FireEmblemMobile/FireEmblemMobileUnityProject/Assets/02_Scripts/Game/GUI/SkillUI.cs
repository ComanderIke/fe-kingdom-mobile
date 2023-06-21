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
        [SerializeField] private float scaleSmall = 0.7f;
        [SerializeField] private float scaleBig = 0.9f;
        [SerializeField] private GameObject hpTextGo;
        [SerializeField] private GameObject usesTextGo;
        public void SetSkill(Skill skill, bool big)
        {
            icon.sprite = skill.Icon;
            this.skill = skill;
            if (skill is ActivatedSkill activatedSkill)
            {
                uses.text = activatedSkill.currentUses + "/" +
                            activatedSkill.maxUses;
                hpCost.text = ""+activatedSkill.hpCost;
                Debug.Log(GetComponent<RectTransform>().sizeDelta);
                if (!big)
                {
                    hpTextGo.transform.localScale = new Vector3(scaleSmall, scaleSmall, scaleSmall);
                    usesTextGo.transform.localScale = new Vector3(scaleSmall, scaleSmall, scaleSmall);
                }
                else
                {
                    hpTextGo.transform.localScale = new Vector3(scaleBig, scaleBig, scaleBig);
                    usesTextGo.transform.localScale = new Vector3(scaleBig, scaleBig ,scaleBig);
                }
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