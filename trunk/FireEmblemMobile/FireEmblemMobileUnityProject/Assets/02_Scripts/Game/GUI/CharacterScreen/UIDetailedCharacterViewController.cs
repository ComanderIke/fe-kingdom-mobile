using Game.GameActors.Units;
using Game.GUI;
using LostGrace;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class UIDetailedCharacterViewController : UICharacterViewController
{
    public TextMeshProUGUI Lv;
    public IStatBar ExpBar;
    public UIEquipmentController equipmentController;
   
    public SkillTreeUI skillTreeUI;
    public Animator IdleAnimation;
    public SkillsUI skillsUI;
   
    
    
    public void SkillTreeClicked()
    {
        skillTreeUI.Show(unit);
    }
    protected override void UpdateUI(Unit unit)
    {
        base.UpdateUI(unit);
        Lv.SetText("Lv. "+unit.ExperienceManager.Level);
        ExpBar.SetValue(unit.ExperienceManager.Exp, unit.ExperienceManager.MaxExp);
        skillsUI.Show(unit.SkillManager.Skills, unit.SkillManager.SkillPoints);

        equipmentController.Show(unit);
        IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
    }

}