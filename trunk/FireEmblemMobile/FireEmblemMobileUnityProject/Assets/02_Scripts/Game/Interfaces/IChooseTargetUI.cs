using System;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;

public interface IChooseTargetUI
{
    event Action OnBackClicked;
    void Show(Unit u, ITargetableObject targetableObject);
    void Hide();
    void ShowSkillPreview(SingleTargetMixin stm, Unit selectedUnit, Unit gridObject);
    void ShowSkillPreview(IPosTargeted posTargetSkill, Unit selectedUnit, Unit gridObject);
    void HideSkillPreview(SingleTargetMixin stm);
    void HideSkillPreview(IPosTargeted posTargetSkill);
    void ShowSkillDialogController(Skill selectedSkill, Action action);
}