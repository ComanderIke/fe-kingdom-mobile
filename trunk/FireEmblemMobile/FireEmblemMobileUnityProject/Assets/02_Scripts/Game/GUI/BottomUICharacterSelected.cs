using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.Mechanics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class BottomUICharacterSelected : BottomUIBase
    {
        [SerializeField] UseItemDialogController useItemDialogController;
        [SerializeField] UseSkillDialogController useSkillDialogController;
        [SerializeField] private Image faceSprite;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI lvl;
        [SerializeField] private TextMeshProUGUI exp;
        [SerializeField] private TextMeshProUGUI weaponType;
        [SerializeField] private Image weaponTypeIcon;
        [SerializeField] private TextMeshProUGUI move;
        [SerializeField] private Image moveIcon;
        [SerializeField] private SmithingSlot weaponSlot;
        [SerializeField] private SmithingSlot RelicSlot1;
        [SerializeField] private UICombatItemSlot combatItem1;
        // [SerializeField] private UICombatItemSlot combatItem2;
        [SerializeField] private TextMeshProUGUI hp;
        [SerializeField] private UIBattleStatsBottom battleStatsController;
        [SerializeField] private Transform skillContainer;
        [SerializeField] private GameObject skillprefab;
        [SerializeField] private GameObject activeSkillprefab;
        [SerializeField] private GameObject combatSkillprefab;

        private bool interactableForPlayer;
        private void Start()
        {
            UiSystem.OnShowAttackPreview += ShowSelectableCombatSkills;
            UiSystem.OnHideAttackPreview += HideSelectableCombatSkills;
            UnitSelectionSystem.OnSkillSelected += SkillSelected;
            UnitSelectionSystem.OnSkillDeselected += SkillDeselected;
        }

        private void OnDestroy()
        {
            UiSystem.OnShowAttackPreview -= ShowSelectableCombatSkills;
            UiSystem.OnHideAttackPreview -= HideSelectableCombatSkills;
            UnitSelectionSystem.OnSkillSelected -= SkillSelected;
            UnitSelectionSystem.OnSkillDeselected -= SkillDeselected;
        }

       
        private void HideSelectableCombatSkills()
        {
            if (!interactableForPlayer)
                return;
        
            DeselectCombatSkill();
            if(instantiatedSkills!=null)
                foreach (var skillUI in instantiatedSkills)
                {
                    if (skillUI.Skill.CombatSkillMixin != null)
                    {
                        skillUI.HideSelectable();
                    }
                }
        }

        

        private SkillUI selectedCombatSkill;
        private void ShowSelectableCombatSkills()
        {
            if (!interactableForPlayer)
                return;
            // canSelectCombatSkillsRightNow = true;
            selectedCombatSkill = null;
            attackStarted = false;
            foreach (var skillUI in instantiatedSkills)
            {
                if (skillUI.Skill.CombatSkillMixin != null)
                {
                    skillUI.ShowSelectable();
                    
                }
                else
                {
                    skillUI.HideSelectable();
                }
            }
        }

        
        private List<SkillUI> instantiatedSkills;
        public void UpdateUI()
        {
            if (unit == null)
                return;
            faceSprite.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
            nameText.text = unit.Name;
            lvl.text = "" + unit.ExperienceManager.Level;
            exp.text = unit.ExperienceManager.Exp + "/" + ExperienceManager.MAX_EXP;
            //weaponType.text = unit.equippedWeapon.WeaponType.WeaponName;
            //weaponTypeIcon.sprite = unit.equippedWeapon.WeaponType.Icon;
            //move.text = "Mov " + unit.MovementRange;
            //moveIcon.sprite = unit.MoveType.icon;
            // if(moveIcon.sprite==null)
            //     moveIcon.gameObject.SetActive(false);
            // else
            // {
            //     moveIcon.gameObject.SetActive(true);
            // }
            weaponSlot.Show(unit.equippedWeapon);
            RelicSlot1.Show(unit.EquippedRelic);
            combatItem1.Show(unit.CombatItem1);
            combatItem1.OnClicked += CombatItemClicked;
            // combatItem2.Show(unit.CombatItem2);
            // combatItem2.OnClicked += CombatItemClicked;

            hp.text = unit.Hp + "/" + unit.MaxHp;
            battleStatsController.Show(unit);
            skillContainer.DeleteAllChildren();
            instantiatedSkills = new List<SkillUI>();
            foreach (var skill in unit.SkillManager.Skills)
            {
                var prefab = skillprefab;
                if (skill.activeMixins.Count > 0)
                    prefab = activeSkillprefab;
                else if (skill.CombatSkillMixin != null)
                    prefab = combatSkillprefab;
                var go =Instantiate(prefab, skillContainer);
                var skillUI =  go.GetComponent<SkillUI>();
                skillUI.SetSkill(skill, true, unit.Blessing!=null);
                skillUI.OnClicked += SkillClicked;
                instantiatedSkills.Add(skillUI);

            }

        }

        private Unit unit;
        public void Show(Unit unit, bool interactableForPlayer=true)
        {
            base.Show();
            this.unit = unit;
            this.interactableForPlayer = interactableForPlayer;
           
           UpdateUI();
        }

        public void CombatItemClicked(UICombatItemSlot  clickedCombatItemUI)
        {
         
            var combatItem = clickedCombatItemUI.GetCombatItem();
            Debug.Log("CLICKED COMBAT ITEM: " +combatItem.item.GetName());
            ToolTipSystem.Show(new StockedItem((Item)combatItem.item, combatItem.stock), clickedCombatItemUI.transform.position);
            if (!interactableForPlayer)
                return;
            useItemDialogController.Show((Item)combatItem.item,()=>new GameplayCommands().SelectItem((Item)combatItem.item));
        }
        public void SkillClicked(SkillUI skillUI)
        {
         
           
            Debug.Log("CLICKED Skill: " +skillUI.Skill.Name);
            ToolTipSystem.Show(skillUI.Skill, skillUI.transform.position);
            if (!interactableForPlayer)
                return;
            if (skillUI.Skill.activeMixins.Count > 0)
            {
                if(ServiceProvider.Instance.GetSystem<UnitSelectionSystem>().SelectedSkill==null)
                    useSkillDialogController.Show(skillUI.Skill, () => new GameplayCommands().SelectSkill(skillUI.Skill));
                else
                {
                    new GameplayCommands().DeselectSkill();
                }
            }
            else if (skillUI.Skill.CombatSkillMixin != null)
            {
               // Debug.Log("CombatSkill Clicked!");
                // if (canSelectCombatSkillsRightNow)
                // {
                   // Debug.Log("Can Select Right now");
                    if (selectedCombatSkill == skillUI)
                    {
                       // Debug.Log("Deselect SKILL "+ skillUI.Skill.Name);
                        
                      
                        DeselectCombatSkill();
                        ServiceProvider.Instance.GetSystem<UnitActionSystem>().UpdateAttackpreview();
                    }
                    else
                    {
                        SelectSkill(skillUI);
                    }

                    
                // }
            }

        }

        void DeselectCombatSkill()
        {
            if (!interactableForPlayer)
                return;
            if (selectedCombatSkill != null)
            {
                selectedCombatSkill.Deselect();
                selectedCombatSkill.ShowSelectable();
                selectedCombatSkill.SetActiveCombatSkill(false);
                selectedCombatSkill.Skill.CombatSkillMixin.Deactivate();
                if(!attackStarted)
                    selectedCombatSkill.Skill.CombatSkillMixin.DeactivateForNextCombat();
                selectedCombatSkill = null;
                BattleState.OnStartBattle -= OnStartAttack;
               
            }
        }
        private void SkillDeselected(Skill skill)
        {
            if (!interactableForPlayer)
                return;
            foreach (var skillUI in instantiatedSkills)
            {
                if (skillUI.Skill.Equals(skill))
                {
                    skillUI.Deselect();
                }
            }
        }
        private void SkillSelected(Skill skill)
        {
            if (!interactableForPlayer)
                return;
            foreach (var skillUI in instantiatedSkills)
            {
                if (skillUI.Skill.Equals(skill))
                {
                    skillUI.ShowSelectable();
                }
            }
        }
        public void SelectSkill(SkillUI skillUI)
        {
            if (!interactableForPlayer)
                return;
            Debug.Log("SELECT SKILL "+ skillUI.Skill.Name);
            DeselectCombatSkill();
            skillUI.Select();
            selectedCombatSkill = skillUI;
            skillUI.SetActiveCombatSkill(true);
            skillUI.Skill.CombatSkillMixin.Activate(skillUI.Skill.owner, null);
            BattleState.OnStartBattle -= OnStartAttack;
            BattleState.OnStartBattle += OnStartAttack;
            ServiceProvider.Instance.GetSystem<UnitActionSystem>().UpdateAttackpreview();
        }

        private bool attackStarted = false;
        void OnStartAttack(IBattleActor attacker, IAttackableTarget target)
        {
            if (!interactableForPlayer)
                return;
            selectedCombatSkill.Skill.CombatSkillMixin.ActivateForNextCombat();
            attackStarted = true;
        }

        
    }
}
