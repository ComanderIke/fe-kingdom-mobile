using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.OnGameObject;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UICharacterViewController : MonoBehaviour
{
    public Unit unit;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI Lv;
    public TextMeshProUGUI Exp;
    public TextMeshProUGUI CharClass;
    public UIEquipmentController equipmentController;
    public UIAttributeController attributeController;
    public UIConvoyController convoyController;
    public UISkillsController skillsController;
    public UITalentsController talentsController;
    public TextMeshProUGUI weaponAtk;
    public TextMeshProUGUI Atk;
    public TextMeshProUGUI AS;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI MaxHP;
    public TextMeshProUGUI SP;
    public TextMeshProUGUI Hitrate;
    public TextMeshProUGUI DodgeRate;
    public TextMeshProUGUI Crit;
    public TextMeshProUGUI CritAvoid;
    public TextMeshProUGUI Armor;
    public TextMeshProUGUI MagicResistance;
    public TextMeshProUGUI WeaponName;
    public IStatBar HPBar;
    public ISPBarRenderer SPBars;
    public Image image;

    private void OnEnable()
    {
        Show(unit);
    }

    public void Show(Unit unit)
    {
        this.unit = unit;
       // charName.SetText(unit.name);
        Lv.SetText(""+unit.ExperienceManager.Level);
        Exp.SetText(""+unit.ExperienceManager.Exp);
        CharClass.SetText("[CLASS]");
        image.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
        Atk.SetText(""+unit.BattleComponent.BattleStats.GetDamage());
        AS.SetText(""+unit.BattleComponent.BattleStats.GetAttackSpeed());
        HP.SetText(""+unit.Hp);
        MaxHP.SetText("/"+unit.Stats.MaxHp);
        SP.SetText(""+unit.Sp);
        Armor.SetText(""+unit.Stats.Armor);

        HPBar.SetValue(unit.Hp, unit.Stats.MaxHp);
        SPBars.SetValue(unit.SpBars, unit.MaxSpBars);
        equipmentController.Show(unit);
        attributeController.Show(unit);
        skillsController.Show(unit);
        talentsController.Show(unit);
        convoyController.Show();

    }
}
