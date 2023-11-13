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
   
        [SerializeField] private Image frame;
        [SerializeField] private Image background;
        [SerializeField] private Color blessingFrameColor;
        [SerializeField] private Material blessingFrameMaterial;
        [SerializeField] private Material curseFrameMaterial;
        [SerializeField] private Color curseFrameColor;
        [SerializeField] private Color blessingBackgroundColor;
        [SerializeField] private Color curseBackgroundColor;
        [SerializeField] private TMP_ColorGradient hpCostBlueColor;
        [SerializeField] private TMP_ColorGradient hpCostRedColor;
        [SerializeField] private GameObject deleteButton;
        [SerializeField] private GameObject selectedVfx;
        [SerializeField] private GameObject selectableVfx;
        [SerializeField] private Image combatSkillActive;
        [SerializeField] private Vector2 bigSize;
        [SerializeField] private Button button;
        [SerializeField] private Button CancelSkillButton;
        private bool showTooltips = false;
        private bool blessed;
        private bool canAffordHpCost;
        private bool hasUses;
        public void SetSkill(Skill skill, bool big, bool blessed,bool canAfforHpCost,bool hasUses, bool showTooltips = false, bool interactable=true)
        {
            
            deleteButton.SetActive(false);
            this.blessed = blessed;
            this.showTooltips = showTooltips;
            icon.sprite = skill.Icon;
            this.Skill = skill;
            blessingEffect.gameObject.SetActive(blessed);
            if(combatSkillActive!=null)
                combatSkillActive.gameObject.SetActive(false);
            if(big)
                (transform as RectTransform).sizeDelta = bigSize;
            button.interactable = interactable;
            this.canAffordHpCost = canAfforHpCost;
            this.hasUses = hasUses;
            // if (button.interactable)
            //     button.interactable = canAfforHpCost;
            background.raycastTarget = interactable;
            if (skill.IsActive() || skill.IsCombat())
            {
                if (skill.FirstActiveMixin != null)
                {


                    uses.text = skill.FirstActiveMixin.Uses + "/" +
                                skill.FirstActiveMixin.maxUsesPerLevel[skill.Level];
                    hpCost.text = "-" + skill.FirstActiveMixin.hpCostPerLevel[skill.Level];
                }
                else
                {
                    uses.text = skill.CombatSkillMixin.Uses + "/" +
                                skill.CombatSkillMixin.maxUsesPerLevel[skill.Level];
                    hpCost.text = "-" + skill.CombatSkillMixin.hpCostPerLevel[skill.Level];
                }


                if (!big)
                {
                    hpCost.transform.localScale = new Vector3(scaleSmall, scaleSmall, scaleSmall);
                    uses.transform.localScale = new Vector3(scaleSmall, scaleSmall, scaleSmall);
                }
                else
                {
                    hpCost.transform.localScale = new Vector3(scaleBig, scaleBig, scaleBig);
                    uses.transform.localScale = new Vector3(scaleBig, scaleBig ,scaleBig);
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

            if(hpCost!=null)
                hpCost.colorGradientPreset = canAfforHpCost?hpCostBlueColor: hpCostRedColor;
            
        }

        public void Select()
        {
            if(CancelSkillButton!=null)
                CancelSkillButton.gameObject.SetActive(true);
            selectableVfx.gameObject.SetActive(false);
            selectedVfx.gameObject.SetActive(true);
        }
        public void Deselect()
        {
            if(CancelSkillButton!=null)
                CancelSkillButton.gameObject.SetActive(false);
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

            Debug.Log(Skill.CombatSkillMixin+" "+canAffordHpCost+" "+hasUses);
           
            if (Skill.activeMixins.Count > 0)
            {
                ToolTipSystem.Show(Skill,blessed, transform.position);
                if (!canAffordHpCost||!hasUses)
                    return;
                OnClicked?.Invoke(this);
            }
            else if (Skill.CombatSkillMixin!=null)
            {
                ToolTipSystem.Show(Skill,blessed, transform.position);
                if (!canAffordHpCost||!hasUses)
                    return;
                
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