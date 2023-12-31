using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    void UpdateUI()
    {
        if (skillUI != null&&equipable!=null)
        {
            var skill = equipable.Skill;
            if (skill != null)
            {
                // Debug.Log("TODO");
                bool canAffordHPCost = skill.FirstActiveMixin != null && unit.Hp > skill.FirstActiveMixin.GetHpCost(skill.level) || skill.CombatSkillMixin != null && unit.Hp > skill.CombatSkillMixin.GetHpCost(skill.level);
                bool hasUses=skill.FirstActiveMixin != null &&skill.FirstActiveMixin.Uses>0 || skill.CombatSkillMixin != null && skill.CombatSkillMixin.Uses>0;

                skillUI.SetSkill(skill, true, false, canAffordHPCost, hasUses);
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
            image.color = emptyColor;
            image.sprite = emptySprite;
            slot.gameObject.SetActive(false);
            gem.sprite = null;
            gem.enabled = false;
            
        }
        else
        {
            image.sprite = equipable.Sprite;
            image.color = normalColor;
            if (equipable is Relic relic)
            {
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
}