using System;
using Game.GameActors.Items.Consumables;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.GUI.Interface;

namespace Game.Interfaces
{
    public interface IChooseTargetUI
    {
        event Action OnBackClicked;
        void Show(Unit u, ITargetableObject targetableObject);
        void Hide();
        void ShowSkillPreview(SingleTargetMixin stm, Unit selectedUnit, Unit gridObject);
        void ShowSkillPreview(IPosTargeted posTargetSkill, Unit selectedUnit, Unit gridObject);
        void HideSkillPreview(SingleTargetMixin stm);
        void HideSkillPreview(IPosTargeted posTargetSkill);
        void ShowSkillDialogController(Skill selectedSkill, Action action, Action cancelAction=null);
    }
}