using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
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
    [SerializeField]  Transform blessingParent;
    [SerializeField]  Transform curseParent;
    [SerializeField] private GameObject cursePrefab;
    [SerializeField] private GameObject blessingPrefab;
    
    
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
    protected override void UpdateUI(Unit unit)
    {
        base.UpdateUI(unit);
        Lv.SetText("Lv. "+unit.ExperienceManager.Level);
        ExpBar.SetValue(unit.ExperienceManager.Exp, ExperienceManager.MAX_EXP);
        skillsUI.Show(unit.SkillManager.Skills);
        blessingParent.DeleteAllChildren();
        curseParent.DeleteAllChildren();
        if (unit.Blessings != null)
        {
            foreach (var blessing in unit.Blessings)
            {
                var go = Instantiate(blessingPrefab, blessingParent);
                go.GetComponent<Image>().sprite = blessing.Skill.Icon;
                go.GetComponent<GeneralButtonController>().OnClicked += BlessingClicked;
            }
          
        }
        if (unit.Curses != null)
        {
            foreach (var curse in unit.Curses)
            {
                var go = Instantiate(cursePrefab, curseParent);
                go.GetComponent<Image>().sprite = curse.Skill.Icon;
                go.GetComponent<GeneralButtonController>().OnClicked += CurseClicked;
            }
          
        }
       

        equipmentController.Show(unit);
        IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
    }

    void BlessingClicked(RectTransform clickedTransform)
    {
        ToolTipSystem.Show(unit.Blessings[0], clickedTransform.position);
    }
    void CurseClicked(RectTransform clickedTransform)
    {
        ToolTipSystem.Show(unit.Curses[0], clickedTransform.position);
    }

}

