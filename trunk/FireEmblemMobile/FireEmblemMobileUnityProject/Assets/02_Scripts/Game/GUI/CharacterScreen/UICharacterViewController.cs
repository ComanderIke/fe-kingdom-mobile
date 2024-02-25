using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.OnGameObject;
using Game.GUI;
using Game.Mechanics;
using Game.Mechanics.Battle;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UICharacterViewController : MonoBehaviour
{
    public Canvas canvas;
    // public DropArea dropArea;
    [FormerlySerializedAs("unitBp")] public Unit unit;
    public TextMeshProUGUI charName;

    

    public ShowAttributeState showAttributeState;
  
    
    public TextMeshProUGUI STR_Label;
    public TextMeshProUGUI INT_Label;
    public TextMeshProUGUI DEX_Label;
    public TextMeshProUGUI AGI_Label;
    public TextMeshProUGUI CON_Label;
    public TextMeshProUGUI FTH_Label;
    public TextMeshProUGUI LCK_Label;
    public TextMeshProUGUI DEF_Label;
    public UIStatText STR;
    public UIStatText INT;
    public UIStatText DEX;
    public UIStatText AGI;
    public UIStatText CON;
    public UIStatText FTH;
    public UIStatText LCK;
    public UIStatText DEF;
    public UICharacterFace CharacterFace;

    public bool IsVisible ()
    {
        if (canvas != null)
            return canvas.enabled;
        return true;
    }

    void OnEnable()
    {
     
        Unit.OnUnitDataChanged += UpdateUI;
    }

    private void OnDisable()
    {
        Unit.OnUnitDataChanged -= UpdateUI;
    }
 
    
    protected int currentFixedIndex;
    public virtual void Show(Unit unit, bool button=false)
    {
        
      
        if(canvas!=null)
            canvas.enabled = true;
        if(!button)
            Player.Instance.Party.SetActiveUnit(unit);
       // Debug.Log("Showing Character UI for: "+unit.name);
        UpdateUI(unit);

    }

    
  
    protected virtual void UpdateUI(Unit unit)
    {
       
        this.unit = unit;
       // Debug.Log("CharFace: "+CharacterFace);
        // Debug.Log("UpdateUI: "+unit);
        if(CharacterFace!=null)
            CharacterFace.Show(unit);
        charName.SetText(unit.name);//+", "+unit.jobClass);
       
        if (showAttributeState==ShowAttributeState.Attributes)
        {
            STR.SetValue(unit.Stats.CombinedAttributes().STR,unit.Stats.GetAttributeBonusState(AttributeType.STR));
            INT.SetValue(unit.Stats.CombinedAttributes().INT,unit.Stats.GetAttributeBonusState(AttributeType.INT));
            DEX.SetValue(unit.Stats.CombinedAttributes().DEX,unit.Stats.GetAttributeBonusState(AttributeType.DEX));
            AGI.SetValue(unit.Stats.CombinedAttributes().AGI,unit.Stats.GetAttributeBonusState(AttributeType.AGI));
            CON.SetValue(unit.Stats.CombinedAttributes().MaxHp,unit.Stats.GetAttributeBonusState(AttributeType.CON));
            FTH.SetValue(unit.Stats.CombinedAttributes().FAITH,unit.Stats.GetAttributeBonusState(AttributeType.FTH));
            LCK.SetValue(unit.Stats.CombinedAttributes().LCK,unit.Stats.GetAttributeBonusState(AttributeType.LCK));
            DEF.SetValue(unit.Stats.CombinedAttributes().DEF,unit.Stats.GetAttributeBonusState(AttributeType.DEF));
            STR_Label.text = Attributes.GetAsLongText(0);
            DEX_Label.text = Attributes.GetAsLongText(1);
            INT_Label.text = Attributes.GetAsLongText(2);
            AGI_Label.text = Attributes.GetAsLongText(3);
            LCK_Label.text = Attributes.GetAsLongText(5);
            CON_Label.text = Attributes.GetAsLongText(4);
            DEF_Label.text = Attributes.GetAsLongText(6);
            FTH_Label.text = Attributes.GetAsLongText(7);
            
        }
        else if(showAttributeState==ShowAttributeState.Growths)
        {
            STR.SetValue(unit.Stats.CombinedGrowths().STR,unit.Stats.GetGrowthBonusState(AttributeType.STR));
            INT.SetValue(unit.Stats.CombinedGrowths().INT,unit.Stats.GetGrowthBonusState(AttributeType.INT));
            DEX.SetValue(unit.Stats.CombinedGrowths().DEX,unit.Stats.GetGrowthBonusState(AttributeType.DEX));
            AGI.SetValue(unit.Stats.CombinedGrowths().AGI,unit.Stats.GetGrowthBonusState(AttributeType.AGI));
            CON.SetValue(unit.Stats.CombinedGrowths().MaxHp,unit.Stats.GetGrowthBonusState(AttributeType.CON));
            FTH.SetValue(unit.Stats.CombinedGrowths().FAITH,unit.Stats.GetGrowthBonusState(AttributeType.FTH));
            LCK.SetValue(unit.Stats.CombinedGrowths().LCK,unit.Stats.GetGrowthBonusState(AttributeType.LCK));
            DEF.SetValue(unit.Stats.CombinedGrowths().DEF,unit.Stats.GetGrowthBonusState(AttributeType.DEF));
            STR_Label.text = Attributes.GetAsLongText(0);
            DEX_Label.text = Attributes.GetAsLongText(1);
            INT_Label.text = Attributes.GetAsLongText(2);
            AGI_Label.text = Attributes.GetAsLongText(3);
            LCK_Label.text = Attributes.GetAsLongText(5);
            CON_Label.text = Attributes.GetAsLongText(4);
            DEF_Label.text = Attributes.GetAsLongText(6);
            FTH_Label.text = Attributes.GetAsLongText(7);
        }
        else
        {
            bool physical = unit.equippedWeapon.DamageType == DamageType.Physical;
            int sumBonuses = unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Attack,physical);
            STR.SetValue(unit.BattleComponent.BattleStats.GetDamage(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
            sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.AttackSpeed,physical);
            INT.SetValue(unit.BattleComponent.BattleStats.GetAttackSpeed(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
            sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Protection,physical);
            DEF.SetValue(unit.BattleComponent.BattleStats.GetPhysicalResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
            sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Resistance,physical);
            FTH.SetValue(unit.BattleComponent.BattleStats.GetFaithResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
            sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Hit,physical);
            DEX.SetValue(unit.BattleComponent.BattleStats.GetHitrate(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
            sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Avoid,physical);
            AGI.SetValue(unit.BattleComponent.BattleStats.GetAvoid(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
            sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.Crit,physical);
            LCK.SetValue(unit.BattleComponent.BattleStats.GetCrit(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
            sumBonuses= unit.Stats.GetCombatStatBonuses(unit,CombatStats.CombatStatType.CurseResistance,physical);
            CON.SetValue(unit.BattleComponent.BattleStats.GetCurseResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
            STR_Label.text = CombatStats.GetAsText(CombatStats.CombatStatType.Attack);
            DEX_Label.text = CombatStats.GetAsText(CombatStats.CombatStatType.Hit);
            INT_Label.text = CombatStats.GetAsText(CombatStats.CombatStatType.AttackSpeed);
            AGI_Label.text = CombatStats.GetAsText(CombatStats.CombatStatType.Avoid);
            LCK_Label.text = CombatStats.GetAsText(CombatStats.CombatStatType.Crit);
            CON_Label.text = CombatStats.GetAsText(CombatStats.CombatStatType.CurseResistance);
            DEF_Label.text = CombatStats.GetAsText(CombatStats.CombatStatType.Protection);
            FTH_Label.text = CombatStats.GetAsText(CombatStats.CombatStatType.Resistance);
        }
        //Debug.Log("FTH: "+unit.Stats.BaseAttributes.FAITH+" "+unit.Stats.CombinedAttributes().FAITH+" "+unit.Stats.BonusAttributesFromFood.FAITH+" "+unit.Stats.BonusAttributesFromEffects.FAITH+" "+unit.Stats.BonusAttributesFromEquips.FAITH+" "+unit.Stats.BonusAttributesFromWeapon.FAITH+" "+unit.Stats.BaseAttributesAndWeapons().FAITH+unit.Stats.GetAttributeBonusState(AttributeType.FTH));
    }


    public void ToggleAttributeGrowths()
    {
        switch (showAttributeState)
        {
            case ShowAttributeState.Attributes:
                showAttributeState = ShowAttributeState.Growths; break;
            case ShowAttributeState.Growths: 
                showAttributeState = ShowAttributeState.CombatStats;break;
            case ShowAttributeState.CombatStats: 
                showAttributeState = ShowAttributeState.Attributes;break;
        }
        UpdateUI(unit);
    }
    public void STR_Clicked()
    {
        Debug.Log("STR CLICKED");
        if(showAttributeState==ShowAttributeState.Attributes)
            ToolTipSystem.ShowAttributeValue(unit, AttributeType.STR, STR.transform.position);
        else if(showAttributeState==ShowAttributeState.CombatStats)
            ToolTipSystem.ShowCombatStatValue(unit, CombatStats.CombatStatType.Attack, STR.transform.position);
        //ToolTipSystem.ShowAttribute("Strength", "Increases ones physical damage output!",unit.Stats.BaseAttributes.STR, STR.transform.position);
    }
    public void INT_Clicked()
    {
        Debug.Log("INT CLICKED");
        if(showAttributeState==ShowAttributeState.Attributes)
            ToolTipSystem.ShowAttributeValue(unit, AttributeType.INT, INT.transform.position);
        else if(showAttributeState==ShowAttributeState.CombatStats)
            ToolTipSystem.ShowCombatStatValue(unit, CombatStats.CombatStatType.AttackSpeed, INT.transform.position);
        //ToolTipSystem.ShowAttribute("Intelligence", "Increases ones magical damage output and something else!",unit.Stats.BaseAttributes.INT,INT.transform.position);
    }
    public void DEX_Clicked()
    {
        if(showAttributeState==ShowAttributeState.Attributes)
            ToolTipSystem.ShowAttributeValue(unit, AttributeType.DEX, DEX.transform.position);
        else if(showAttributeState==ShowAttributeState.CombatStats)
            ToolTipSystem.ShowCombatStatValue(unit, CombatStats.CombatStatType.Hit, DEX.transform.position);
        //ToolTipSystem.ShowAttribute("Dexterity", "Increases ones accuracy and influences critical hitrate!",unit.Stats.BaseAttributes.DEX,DEX.transform.position);
    }
    public void AGI_Clicked()
    {
        if(showAttributeState==ShowAttributeState.Attributes)
            ToolTipSystem.ShowAttributeValue(unit, AttributeType.AGI, AGI.transform.position);
        else if(showAttributeState==ShowAttributeState.CombatStats)
            ToolTipSystem.ShowCombatStatValue(unit, CombatStats.CombatStatType.Avoid, AGI.transform.position);

       // ToolTipSystem.ShowAttribute("Agility", "Increases ones ability to dodge as well as attack speed! Having 5 higher than your opponent allows for double attacks",unit.Stats.BaseAttributes.AGI,AGI.transform.position);
    }
    public void DEF_Clicked()
    {
        if(showAttributeState==ShowAttributeState.Attributes)
            ToolTipSystem.ShowAttributeValue(unit, AttributeType.DEF, DEF.transform.position);
        else if(showAttributeState==ShowAttributeState.CombatStats)
            ToolTipSystem.ShowCombatStatValue(unit, CombatStats.CombatStatType.Protection, DEF.transform.position);

        //ToolTipSystem.ShowAttribute("Defense", "Increases your physical damage resistance!",unit.Stats.BaseAttributes.DEF,DEF.transform.position);
    }
    public void LCK_Clicked()
    {
        if(showAttributeState==ShowAttributeState.Attributes)
            ToolTipSystem.ShowAttributeValue(unit, AttributeType.LCK, LCK.transform.position);
        else if(showAttributeState==ShowAttributeState.CombatStats)
            ToolTipSystem.ShowCombatStatValue(unit, CombatStats.CombatStatType.Crit, LCK.transform.position);
        //ToolTipSystem.ShowAttribute("Luck", "Increases ones critical hit rate and many other things!",unit.Stats.BaseAttributes.LCK,LCK.transform.position);
    }
    public void CON_Clicked()
    {
        if(showAttributeState==ShowAttributeState.Attributes)
            ToolTipSystem.ShowAttributeValue(unit, AttributeType.CON, CON.transform.position);
        else if(showAttributeState==ShowAttributeState.CombatStats)
            ToolTipSystem.ShowCombatStatValue(unit, CombatStats.CombatStatType.CurseResistance, CON.transform.position);
        //ToolTipSystem.ShowAttribute("Constitution", "Increases ones maximum Hitpoints and allows to wield heavier weapons!",unit.Stats.BaseAttributes.CON,CON.transform.position);
    }
    public void FTH_Clicked()
    {
        if(showAttributeState==ShowAttributeState.Attributes)
            ToolTipSystem.ShowAttributeValue(unit, AttributeType.FTH, FTH.transform.position);
        else if(showAttributeState==ShowAttributeState.CombatStats)
            ToolTipSystem.ShowCombatStatValue(unit, CombatStats.CombatStatType.Resistance, FTH.transform.position);
       // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
   
   
   
    
   
  
   
   
    
    public virtual void Hide()
    {
        if(canvas!=null)
            canvas.enabled = false;
    }
    public void UpdateUnit(Unit unit1)
    {
        MyDebug.LogTest(gameObject);
       UpdateUI(unit1);
    }
}