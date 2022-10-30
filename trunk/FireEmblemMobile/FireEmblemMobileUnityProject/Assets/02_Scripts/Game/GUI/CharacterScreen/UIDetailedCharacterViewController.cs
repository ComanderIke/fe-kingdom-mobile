using Game.GameActors.Units;
using Game.GUI;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class UIDetailedCharacterViewController : UICharacterViewController
{
    public TextMeshProUGUI Lv;
    public IStatBar ExpBar;
    public UIEquipmentController equipmentController;
    public IStatBar HPBar;
    public SkillTreeUI skillTreeUI;
    public Animator IdleAnimation;
    public GameObject skillPointPreview;
    public TextMeshProUGUI skillPointText;
    
    
    public void SkillTreeClicked()
    {
        skillTreeUI.Show(unit);
    }
    protected override void UpdateUI(Unit unit)
    {
        base.UpdateUI(unit);
        Lv.SetText("Lv. "+unit.ExperienceManager.Level);
        ExpBar.SetValue(unit.ExperienceManager.Exp, unit.ExperienceManager.MaxExp);
        skillPointText.SetText(""+unit.SkillManager.SkillPoints);
        if(unit.SkillManager.SkillPoints >=1)
            skillPointPreview.gameObject.SetActive(true);
        else
        {
            skillPointPreview.gameObject.SetActive(false);
        }
        HPBar.SetValue(unit.Hp, unit.MaxHp);
        equipmentController.Show(unit);
        IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
    }

}