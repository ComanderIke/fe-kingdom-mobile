using System;
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
        public Skill Skill;
        [SerializeField] private TextMeshProUGUI hpCost;
        [SerializeField] private TextMeshProUGUI uses;
        [SerializeField] private float scaleSmall = 0.7f;
        [SerializeField] private float scaleBig = 0.9f;
        [SerializeField] private GameObject hpTextGo;
        [SerializeField] private GameObject usesTextGo;
        [SerializeField] private Image frame;
        [SerializeField] private Image background;
        [SerializeField] private Color blessingFrameColor;
        [SerializeField] private Material blessingFrameMaterial;
        [SerializeField] private Material curseFrameMaterial;
        [SerializeField] private Color curseFrameColor;
        [SerializeField] private Color blessingBackgroundColor;
        [SerializeField] private Color curseBackgroundColor;
        [SerializeField] private GameObject deleteButton;
        public void SetSkill(Skill skill, bool big)
        {
            deleteButton.SetActive(false);
            icon.sprite = skill.Icon;
            this.Skill = skill;
            if (skill.activeMixin is ActiveSkillMixin activatedSkillMixin)
            {
                uses.text = skill.ActiveMixinUses + "/" +
                            activatedSkillMixin.maxUsesPerLevel[skill.Level];
                hpCost.text = ""+activatedSkillMixin.hpCostPerLevel[skill.Level];
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

            if (skill is Blessing)
            {
                frame.color = blessingFrameColor;
                frame.material = blessingFrameMaterial;
                background.color = blessingBackgroundColor;
            }
            else if (skill is Curse)
            {
                frame.color = curseFrameColor;
                frame.material = curseFrameMaterial;
                background.color = curseBackgroundColor;
            }

            
         
        }

        public void Clicked()
        {
            
            if (Skill.activeMixin is ActiveSkillMixin)
            {
                OnClicked?.Invoke(this);
            }
            else
            {
                Debug.Log("Show Skill Tooltip!"+transform.position);
                ToolTipSystem.Show(Skill, transform.position);
            }
        }

        public void ShowDelete()
        {
            deleteButton.SetActive(true);
        }

        public void DeleteClicked()
        {
            Debug.Log("Delete Clicked");
            OnDeleteClicked?.Invoke(this);
        }
        public event Action<SkillUI> OnDeleteClicked;
        public event Action<SkillUI> OnClicked;
    }
}