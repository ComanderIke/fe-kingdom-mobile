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
    public Animator IdleAnimation;
    public GameObject skillPointPreview;
    public TextMeshProUGUI skillPointText;
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
        skillPointText.SetText(""+unit.SkillManager.SkillPoints);
        if(unit.SkillManager.SkillPoints >=1)
            skillPointPreview.gameObject.SetActive(true);
        else
        {
            skillPointPreview.gameObject.SetActive(false);
        }
        HPBar.SetValue(unit.Hp, unit.MaxHp);
        equipmentController.Show(unit);
        IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
    }

    public void STR_Clicked()
    {
        ToolTipSystem.ShowAttribute("Strength", "Increases ones physical damage output!",unit.Stats.Attributes.STR, STR.transform.position);
    }
    public void INT_Clicked()
    {
        ToolTipSystem.ShowAttribute("Intelligence", "Increases ones magical damage output and something else!",unit.Stats.Attributes.INT,INT.transform.position);
    }
    public void DEX_Clicked()
    {
        ToolTipSystem.ShowAttribute("Dexterity", "Increases ones accuracy and influences critical hitrate!",unit.Stats.Attributes.DEX,DEX.transform.position);
    }
    public void AGI_Clicked()
    {
        ToolTipSystem.ShowAttribute("Agility", "Increases ones ability to dodge as well as attack speed! Having 5 higher than your opponent allows for double attacks",unit.Stats.Attributes.AGI,AGI.transform.position);
    }
    public void DEF_Clicked()
    {
        ToolTipSystem.ShowAttribute("Defense", "Increases your physical damage resistance!",unit.Stats.Attributes.DEF,DEF.transform.position);
    }
    public void LCK_Clicked()
    {
        ToolTipSystem.ShowAttribute("Luck", "Increases ones critical hit rate and many other things!",unit.Stats.Attributes.LCK,LCK.transform.position);
    }
    public void CON_Clicked()
    {
        ToolTipSystem.ShowAttribute("Constitution", "Increases ones maximum Hitpoints and allows to wield heavier weapons!",unit.Stats.Attributes.CON,CON.transform.position);
    }
   
    public void FTH_Clicked()
    {
        ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.Attributes.FAITH,FTH.transform.position);
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