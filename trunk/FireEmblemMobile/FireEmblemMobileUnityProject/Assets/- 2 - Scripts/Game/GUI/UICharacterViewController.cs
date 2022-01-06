using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.OnGameObject;
using Game.GUI;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UICharacterViewController : MonoBehaviour
{
    public Canvas canvas;
    public Unit unit;
    public Party party;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI Lv;
    public IStatBar ExpBar;
    public UIEquipmentController equipmentController;
    public UISkillsController skillsController;
    

    public TextMeshProUGUI Hitrate;
    public TextMeshProUGUI DodgeRate;
    public TextMeshProUGUI Crit;
    public TextMeshProUGUI CritAvoid;
    
    public TextMeshProUGUI STR;
    public TextMeshProUGUI INT;
    public TextMeshProUGUI DEX;
    public TextMeshProUGUI AGI;
    public TextMeshProUGUI CON;
    public TextMeshProUGUI FTH;
    
    public IStatBar HPBar;
    public TextMeshProUGUI hpText;
    
    public Image image;

    private int currentUnitIndex = 0;

    // private void OnEnable()
    // {
    //     Show(party);
    // }

    public void Show(Party player)
    {
        canvas.enabled = true;
        this.unit = party.members[currentUnitIndex];
        this.party = party;
        
        charName.SetText(unit.name +", "+unit.jobClass);
        Lv.SetText("Lv. "+unit.ExperienceManager.Level);
        ExpBar.SetValue(unit.ExperienceManager.Exp, unit.ExperienceManager.MaxExp);
        image.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
        
        Hitrate.SetText(""+unit.BattleComponent.BattleStats.GetHitrate()+"%");
        DodgeRate.SetText(""+unit.BattleComponent.BattleStats.GetAvoid()+"%");
        Crit.SetText(""+unit.BattleComponent.BattleStats.GetCrit()+"%");
        CritAvoid.SetText(""+unit.BattleComponent.BattleStats.GetCritAvoid()+"%");
        
        STR.SetText(""+unit.Stats.Attributes.STR);
        INT.SetText(""+unit.Stats.Attributes.INT);
        DEX.SetText(""+unit.Stats.Attributes.DEX);
        AGI.SetText(""+unit.Stats.Attributes.AGI);
        CON.SetText(""+unit.Stats.Attributes.CON);
        FTH.SetText(""+unit.Stats.Attributes.FAITH);
        
        HPBar.SetValue(unit.Hp, unit.Stats.MaxHp);
        equipmentController.Show((Human)unit);
        skillsController.Show(unit);
       

    }

    public void NextCharacterClicked()
    {
        
        currentUnitIndex = currentUnitIndex >= party.members.Count-1 ? 0 : currentUnitIndex+1;
        Debug.Log("Unit Index: " + currentUnitIndex);
        Show(party);
    }
    public void PreviousCharacterClicked()
    {
        currentUnitIndex = currentUnitIndex <= 0 ? party.members.Count-1 : currentUnitIndex-1;
        Debug.Log("Unit Index: " + currentUnitIndex);
        Show(party);
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
}