using Game.GameActors.Items.Relics;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills.Base;
using Game.GUI.EncounterUI.Merchant;
using Game.GUI.ToolTips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Screens
{
    public class SmithingSlot : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Color emptyColor;
        [SerializeField] Color normalColor;
        [SerializeField] Sprite emptySprite;
        [SerializeField] Image backGround;
        [SerializeField] Color bGNormalColor;
        [SerializeField] Color bGSelectedColor;
        [SerializeField] private Image gem;
        [SerializeField] private GameObject slot;
        [SerializeField] private SkillUI skillUI;
        [SerializeField] private TextMeshProUGUI soulsText;
        [SerializeField] private GameObject TooltipButtonArea;
        [SerializeField] private WeightUI weightUI;
        private bool selected = false;
        private EquipableItem equipable;
        private Unit unit;
        public void Show(Unit unit,EquipableItem equipable, bool selected=false)
        {
            this.unit = unit;
            this.selected = selected;
            this.equipable = equipable;
            UpdateUI();
      
        }

        public void TooltipButtonClicked()
        {
            ToolTipSystem.Show(new StockedItem(equipable,1), transform.position);
        }

        void UpdateUI()
        {
            if (skillUI != null&&equipable!=null)
            {
                var skill = equipable.Skill;
                if (skill != null)
                {
                    // Debug.Log("TODO");
                    bool canAffordHPCost = skill.CanCast(unit);
                    bool hasUses=skill.FirstActiveMixin != null &&skill.FirstActiveMixin.Uses>0 || skill.CombatSkillMixin != null && skill.CombatSkillMixin.Uses>0;

                    skillUI.SetSkill(skill, false, false, canAffordHPCost, hasUses);
                }
                else
                {
                    skillUI.Hide();
                }
            }
            else
            {
                skillUI.Hide();
            }
            if (equipable == null)
            {
                if(TooltipButtonArea!=null)
                    TooltipButtonArea.gameObject.SetActive(false);
                image.color = emptyColor;
                image.sprite = emptySprite;
                slot.gameObject.SetActive(false);
                gem.sprite = null;
                gem.enabled = false;
                if (weightUI != null)
                    weightUI.Hide();

            }
            else
            {
                if(TooltipButtonArea!=null)
                    TooltipButtonArea.gameObject.SetActive(true);
                image.sprite = equipable.Sprite;
                image.color = normalColor;
                if (equipable is Relic relic)
                {
                    if (weightUI != null)
                        weightUI.Hide();
                    slot.gameObject.SetActive(true);
                    if (relic.GetGem() != null)
                    {
                        gem.sprite = relic.GetGem().Sprite;
                        gem.enabled = true;
                        soulsText.enabled = true;
                        soulsText.text = ""+relic.GetGem().GetCurrentSouls();
                    }
                    else
                    {
                        gem.sprite = null;
                        gem.enabled = false;
                        soulsText.enabled = false;
                    }
                }
                else
                {
                    if (equipable is Weapon weapon)
                    {
                        if (weightUI != null)
                            weightUI.Show(weapon.GetWeight(), unit.Stats.CombinedAttributes().STR-weapon.GetWeight()<0);
                    }
                    else
                    {
                        if (weightUI != null)
                            weightUI.Hide();
                    }
                    slot.gameObject.SetActive(false);
                    gem.sprite = null;
                    gem.enabled = false;
                }
            }

            if (selected)
            {
                backGround.color = bGSelectedColor;
            }
            else
            {
                backGround.color = bGNormalColor;
            }
        }
        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        public void Select()
        {
            selected = true;
            UpdateUI();
        }

        public void Deselect()
        {
            selected = false;
            UpdateUI();
        }

        public void Highlight()
        {
            Debug.Log("Highlight Slot somehow");
        }

        public bool HasSkill(Skill skill)
        {
            if (equipable == null)
                return false;
            if (equipable.Skill == null)
                return false;
            return equipable.Skill.Equals(skill);
        }

        public void SelectSkill()
        {
            if (equipable.Skill!=null)
            {
                skillUI.Select();
            }
        }
        public void DeselectSkill()
        {
            if (equipable.Skill!=null)
            {
                skillUI.Deselect();
            }
        }
    }
}