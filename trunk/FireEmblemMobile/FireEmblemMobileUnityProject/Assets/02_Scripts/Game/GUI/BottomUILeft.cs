using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameInput;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class BottomUILeft : BottomUIBase
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
        [SerializeField] private UICombatItemSlot combatItem2;
        [SerializeField] private TextMeshProUGUI hp;
        [SerializeField] private TextMeshProUGUI atk;
        [SerializeField] private TextMeshProUGUI spd;
        [SerializeField] private TextMeshProUGUI hit;
        [SerializeField] private TextMeshProUGUI avo;
        [SerializeField] private TextMeshProUGUI def;
        [SerializeField] private TextMeshProUGUI mdef;
        [SerializeField] private TextMeshProUGUI crit;
        [SerializeField] private TextMeshProUGUI critavo;
        [SerializeField] private Transform skillContainer;
        [SerializeField] private GameObject skillprefab;
        [SerializeField] private GameObject activeSkillprefab;

        

        public void Show(Unit unit)
        {
            base.Show();
            faceSprite.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
            nameText.text = unit.Name + ", " + unit.rpgClass;
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
            combatItem2.Show(unit.CombatItem2);
            combatItem2.OnClicked += CombatItemClicked;

            hp.text = unit.Hp + "/" + unit.MaxHp;
            atk.text = ""+unit.BattleComponent.BattleStats.GetAttackDamage();
            hit.text = ""+unit.BattleComponent.BattleStats.GetHitrate();
            crit.text = ""+unit.BattleComponent.BattleStats.GetCrit();
            def.text = ""+unit.BattleComponent.BattleStats.GetPhysicalResistance();
            spd.text = ""+unit.BattleComponent.BattleStats.GetAttackSpeed();
            avo.text = ""+unit.BattleComponent.BattleStats.GetAvoid();
            critavo.text = ""+unit.BattleComponent.BattleStats.GetCritAvoid();
            mdef.text = ""+unit.BattleComponent.BattleStats.GetFaithResistance();
            skillContainer.DeleteAllChildren();
            foreach (var skill in unit.SkillManager.Skills)
            {
                var go =Instantiate(skill is ActivatedSkill?activeSkillprefab:skillprefab, skillContainer);
                var skillUI =  go.GetComponent<SkillUI>();
               skillUI.SetSkill(skill, true);
               skillUI.OnClicked += ActiveSkillClicked;

            }
        }

        public void CombatItemClicked(UICombatItemSlot  clickedCombatItemUI)
        {
         
            var combatItem = clickedCombatItemUI.GetCombatItem();
            Debug.Log("CLICKED COMBAT ITEM: " +combatItem.item.Name);
            ToolTipSystem.Show(combatItem.item, clickedCombatItemUI.transform.position);
            useItemDialogController.Show(combatItem.item,()=>new GameplayCommands().SelectItem(combatItem.item));
        }
        public void ActiveSkillClicked(SkillUI skillUI)
        {
         
           
            Debug.Log("CLICKED Skill: " +skillUI.Skill);
            ToolTipSystem.Show(skillUI.Skill, skillUI.transform.position);
            if(skillUI.Skill is ActivatedSkill activatedSkill)
                useSkillDialogController.Show(activatedSkill,()=>new GameplayCommands().SelectSkill(activatedSkill));

        }
    }
}
