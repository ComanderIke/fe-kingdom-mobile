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
    public Image blessingImage;
   
    
    
    public void SkillTreeClicked()
    {
        skillTreeUI.Show(unit);
    }

    public void BlessingClicked()
    {
        if (unit.Blessing == null)
            return;
        ToolTipSystem.Show(unit.Blessing, blessingImage.transform.position);
    }
    protected override void UpdateUI(Unit unit)
    {
        base.UpdateUI(unit);
        Lv.SetText("Lv. "+unit.ExperienceManager.Level);
        ExpBar.SetValue(unit.ExperienceManager.Exp, unit.ExperienceManager.MaxExp);
        skillsUI.Show(unit.SkillManager.Skills, unit.SkillManager.SkillPoints);
        if (unit.Blessing != null)
        {
            blessingImage.gameObject.SetActive(true);
            blessingImage.sprite = unit.Blessing.Skill.Icon;
        }
        else
        {
            blessingImage.gameObject.SetActive(false);
        }

        equipmentController.Show(unit);
        IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
    }

}

