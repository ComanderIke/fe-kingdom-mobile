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
    public Party party;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI Lv;
    public IStatBar ExpBar;
    public UIEquipmentController equipmentController;
    public UISkillsController skillsController;
    
     public TextMeshProUGUI PartyGold;

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

    void Awake(){
        // if(dropArea!=null)
        //     dropArea.OnDropHandler += OnItemDropped;
    }

    // private void OnItemDropped(UIDragable dragable)
    // {
    //     Debug.Log("Item Dropped!");
    // }

    // private void OnEnable()
    // {
    //     Show(party);
    // }

    public void Show(Party party)
    {
        canvas.enabled = true;



        UpdateUI(party);

    }

    void UpdateUI(Party party)
    {
        this.party = party;
        Debug.Log("Up");
        this.unit = party.members[party.ActiveUnitIndex];
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
        
        PartyGold.SetText(""+party.Money);
        HPBar.SetValue(unit.Hp, unit.Stats.MaxHp);
        equipmentController.Show((Human)unit);
        skillsController.Show(unit);
    }
    public void NextCharacterClicked()
    {
       
        party.ActiveUnitIndex =  party.ActiveUnitIndex >= party.members.Count-1 ? 0 :  party.ActiveUnitIndex+1;
        Debug.Log("Unit Index: " +  party.ActiveUnitIndex);
        Show(party);
    }
    public void PreviousCharacterClicked()
    {
        party.ActiveUnitIndex =  party.ActiveUnitIndex <= 0 ? party.members.Count-1 :  party.ActiveUnitIndex-1;
        Debug.Log("Unit Index: " +  party.ActiveUnitIndex);
        Show(party);
    }

    public void Hide()
    {
        canvas.enabled = false;
    }

    public void UpdateParty(Party party)
    {
       
        UpdateUI(party);
    }
}