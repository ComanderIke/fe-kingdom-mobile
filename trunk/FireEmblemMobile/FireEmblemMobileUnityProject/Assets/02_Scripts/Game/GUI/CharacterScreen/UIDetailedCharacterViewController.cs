using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GUI;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIDetailedCharacterViewController : UICharacterViewController
{
    public TextMeshProUGUI Lv;
    public IStatBar ExpBar;
    public UIEquipmentController equipmentController;
   
    public SkillTreeUI skillTreeUI;
    public Animator IdleAnimation;
    public SkillsUI skillsUI;

    [SerializeField] private UIBoonBaneController boonBaneController;
    public void SkillTreeClicked()
    {
        skillTreeUI.Show(unit);
    }

    // public void BlessingClicked()
    // {
    //     if (unit.Blessing == null)
    //         return;
    //     ToolTipSystem.Show(unit.Blessing, blessingImage.transform.position);
    // }
    public override void Show(Unit unit, bool useFixedUnitList = false, List<Unit> availableUnits = null)
    {
        base.Show(unit, useFixedUnitList, availableUnits);
        boonBaneController.OnBoonSelected -= BoonSelected;
        boonBaneController.OnBaneSelected -= BaneSelected;
        boonBaneController.OnBoonSelected += BoonSelected;
        boonBaneController.OnBaneSelected += BaneSelected;
    }

    public void ShowBoonBaneSelection()
    {
        boonBaneController.Show();
    }
    void BoonSelected(AttributeType boon)
    {
        unit.Stats.SetBoon(boon);
        UpdateUI(unit);
    }

    void BaneSelected(AttributeType bane)
    {
        unit.Stats.SetBane(bane);
        UpdateUI(unit);
    }
    public void NextClicked()
    {
        if (useFixedUnitList)
        {
            currentFixedIndex++;
            if (currentFixedIndex >= availableUnits.Count)
                currentFixedIndex = availableUnits.Count - 1;
            UpdateUI(availableUnits[currentFixedIndex]);
        }
        else
        {
            Player.Instance.Party.ActiveUnitIndex++;
            UpdateUI(Player.Instance.Party.ActiveUnit);
        }
    }
    public void PrevClicked()
    {
        if (useFixedUnitList)
        {
            currentFixedIndex--;
            if (currentFixedIndex < 0)
                currentFixedIndex = 0;
            UpdateUI(availableUnits[currentFixedIndex]);
        }
        else
        {
            Player.Instance.Party.ActiveUnitIndex--;
            UpdateUI(Player.Instance.Party.ActiveUnit);
        }

      
    }

    [SerializeField] private Color baneColor;
    [SerializeField] private Color boonColor;
    [SerializeField] private Color normalColor;
    protected override void UpdateUI(Unit unit)
    {
        if (unit != base.unit)
        {
            boonBaneController.SoftResetBoonBane();
        }
        base.UpdateUI(unit);
        Lv.SetText("Lv. "+unit.ExperienceManager.Level);
        ExpBar.SetValue(unit.ExperienceManager.Exp, ExperienceManager.MAX_EXP, false);
        skillsUI.Show(unit);
        STR_Label.color=normalColor;
        DEX_Label.color=normalColor;
        INT_Label.color=normalColor;
        AGI_Label.color=normalColor;
        CON_Label.color=normalColor;
        LCK_Label.color=normalColor;
        DEF_Label.color=normalColor;
        FTH_Label.color=normalColor;
        boonBaneController.UpdateUI();
       
        switch (unit.Stats.Bane)
        {
            case AttributeType.STR: STR_Label.color=baneColor;
                break;
            case AttributeType.DEX: DEX_Label.color=baneColor;
                break;
            case AttributeType.INT: INT_Label.color=baneColor;
                break;
            case AttributeType.AGI: AGI_Label.color=baneColor;
                break;
            case AttributeType.CON: CON_Label.color=baneColor;
                break;
            case AttributeType.LCK: LCK_Label.color=baneColor;
                break;
            case AttributeType.DEF: DEF_Label.color=baneColor;
                break;
            case AttributeType.FTH: FTH_Label.color=baneColor;
                break;
        }
        switch (unit.Stats.Boon)
        {
            case AttributeType.STR: STR_Label.color=boonColor;
                break;
            case AttributeType.DEX: DEX_Label.color=boonColor;
                break;
            case AttributeType.INT: INT_Label.color=boonColor;
                break;
            case AttributeType.AGI: AGI_Label.color=boonColor;
                break;
            case AttributeType.CON: CON_Label.color=boonColor;
                break;
            case AttributeType.LCK: LCK_Label.color=boonColor;
                break;
            case AttributeType.DEF: DEF_Label.color=boonColor;
                break;
            case AttributeType.FTH: FTH_Label.color=boonColor;
                break;
        }
        // if (unit.Blessing != null)
        // {
        //     
        //         var go = Instantiate(blessingPrefab, blessingParent);
        //         go.GetComponent<Image>().sprite = unit.Blessing .Skill.Icon;
        //         go.GetComponent<GeneralButtonController>().OnClicked += BlessingClicked;
        //     
        //   
        // }
        // if (unit.Curse != null)
        // {
        //     var go = Instantiate(cursePrefab, curseParent);
        //         go.GetComponent<Image>().sprite = unit.Curse .Skill.Icon;
        //         go.GetComponent<GeneralButtonController>().OnClicked += CurseClicked;
        //     
        // }
       

        equipmentController.Show(unit);
        IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
    }

    void BlessingClicked(RectTransform clickedTransform)
    {
        ToolTipSystem.Show(unit.Blessing, clickedTransform.position);
    }
    void CurseClicked(RectTransform clickedTransform)
    {
        ToolTipSystem.Show(unit.Curse, clickedTransform.position);
    }

    public override void Hide()
    {
        equipmentController.Hide();
        base.Hide();
    }
}

