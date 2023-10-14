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
        [SerializeField] private Image blessingEffect;
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
        [SerializeField] private GameObject selectedVfx;
        [SerializeField] private GameObject selectableVfx;
        [SerializeField] private Image combatSkillActive;
        private bool showTooltips = false;
        private bool blessed;
        public void SetSkill(Skill skill, bool big, bool blessed, bool showTooltips = false)
        {
            deleteButton.SetActive(false);
            this.blessed = blessed;
            this.showTooltips = showTooltips;
            icon.sprite = skill.Icon;
            this.Skill = skill;
            blessingEffect.gameObject.SetActive(blessed);
            if(combatSkillActive!=null)
                combatSkillActive.gameObject.SetActive(false);
            if (skill.IsActive())
            {
                uses.text = skill.FirstActiveMixin.Uses + "/" +
                            skill.FirstActiveMixin.maxUsesPerLevel[skill.Level];
                hpCost.text = "-"+ skill.FirstActiveMixin.hpCostPerLevel[skill.Level];
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

        public void Select()
        {
            selectableVfx.gameObject.SetActive(false);
            selectedVfx.gameObject.SetActive(true);
        }
        public void Deselect()
        {
            selectableVfx.gameObject.SetActive(false);
            selectedVfx.gameObject.SetActive(false);
        }
        public void ShowSelectable()
        {
            selectableVfx.gameObject.SetActive(true);
            selectedVfx.gameObject.SetActive(false);
        }
        public void HideSelectable()
        {
            selectableVfx.gameObject.SetActive(false);
            selectedVfx.gameObject.SetActive(false);
        }
        public void Clicked()
        {
            
            if (showTooltips)
            {
                ToolTipSystem.Show(Skill,blessed, transform.position);
                return;
            }
            if (Skill.activeMixins.Count > 0)
            {
                
                OnClicked?.Invoke(this);
            }
            else if (Skill.CombatSkillMixin!=null)
            {
                OnClicked?.Invoke(this);
            }
            else
            {
                Debug.Log("Show Skill Tooltip!"+transform.position);
                ToolTipSystem.Show(Skill,blessed, transform.position);
            }
        }

        public void SetActiveCombatSkill(bool show)
        {
            if(combatSkillActive!=null)
                combatSkillActive.gameObject.SetActive(show);
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