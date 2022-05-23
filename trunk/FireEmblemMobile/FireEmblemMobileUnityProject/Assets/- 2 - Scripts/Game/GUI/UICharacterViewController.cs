using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
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
    // public DropArea dropArea;
    public Unit unit;
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
    public TextMeshProUGUI LCK;
    public TextMeshProUGUI DEF;
    
    public IStatBar HPBar;
    public TextMeshProUGUI hpText;
    public SkillTreeUI skillTreeUI;
    public Image image;

    void Awake(){
        // if(dropArea!=null)
        //     dropArea.OnDropHandler += OnItemDropped;
    }

    public void SkillTreeClicked()
    {
        skillTreeUI.Show(unit);
    }
    // private void OnItemDropped(UIDragable dragable)
    // {
    //     Debug.Log("Item Dropped!");
    // }

    // private void OnEnable()
    // {
    //     Show(party);
    // }

    
    public void Show(Unit unit)
    {
        canvas.enabled = true;

        UpdateUI(unit);

    }


  
    void UpdateUI(Unit unit)
    {
  
        this.unit = unit;
        charName.SetText(unit.name);//+", "+unit.jobClass);
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
        LCK.SetText(""+unit.Stats.Attributes.LCK);
        DEF.SetText(""+unit.Stats.Attributes.DEF);
        
        HPBar.SetValue(unit.Hp, unit.Stats.MaxHp);
        //equipmentController.Show((Human)unit);
        skillsController.Show(unit);
    }
 

    public void Hide()
    {
        canvas.enabled = false;
    }


    public void UpdateUnit(Unit unit1)
    {
       UpdateUI(unit1);
    }
}