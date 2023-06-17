using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class BottomUILeft : BottomUIBase
    {

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
        [SerializeField] private SmithingSlot RelicSlot2;
        [SerializeField] private Image blessingSprite;
        [SerializeField] private Image curseSprite;
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
            RelicSlot1.Show(unit.EquippedRelic1);
            RelicSlot2.Show(unit.EquippedRelic2);
            // if (unit.Blessing != null)
            // {
            //     blessingSprite.enabled=true;
            //     blessingSprite.sprite = unit.Blessing.Skill.Icon;
            // }
            // else
            // {
            //     blessingSprite.enabled=false;
            // }

            // if (unit.Curse!=null)
            // {
            //     curseSprite.enabled = true;
            //     curseSprite.sprite = unit.Curse.Skill.Icon;
            // }
            // else
            // {
            //     curseSprite.enabled=false;
            // }

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
                var go =Instantiate(skillprefab, skillContainer);
                go.GetComponent<SkillUI>().SetSkill(skill);
            }
        }

       
    }
}
