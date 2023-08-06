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


    public GameObject baseAttributePanel;
    public GameObject combatStatsPanel;
    public GameObject baseAttributeButton;
    public GameObject combatStatsButton;
    
    public UIStatText Atk;
    public UIStatText AtkSpeed;
    public UIStatText PhysArmor;
    public UIStatText MagicArmor;
    public UIStatText Hitrate;
    public UIStatText DodgeRate;
    public UIStatText Crit;
    public UIStatText CritAvoid;
    
    public UIStatText STR;
    public UIStatText INT;
    public UIStatText DEX;
    public UIStatText AGI;
    public UIStatText CON;
    public UIStatText FTH;
    public UIStatText LCK;
    public UIStatText DEF;
    public UICharacterFace CharacterFace;

    public bool IsVisible => canvas.enabled;

    void OnEnable()
    {
     
        Unit.OnUnitDataChanged += UpdateUI;
    }

    private void OnDisable()
    {
        Unit.OnUnitDataChanged -= UpdateUI;
    }
    protected bool useFixedUnitList;
    protected List<Unit> availableUnits;
    protected int currentFixedIndex;
    public void Show(Unit unit, bool useFixedUnitList=false, List<Unit> availableUnits=null)
    {
        this.availableUnits = availableUnits;
        this.useFixedUnitList = useFixedUnitList;
        currentFixedIndex = 0;
        canvas.enabled = true;
        Player.Instance.Party.SetActiveUnit(unit);
        Debug.Log("Showing Character UI for: "+unit.name);
        if (useFixedUnitList)
        {
            Debug.Log(availableUnits.Count + " " + currentFixedIndex);
            UpdateUnit(availableUnits[currentFixedIndex]);
        }
        else
            UpdateUI(unit);

    }

    
  
    protected virtual void UpdateUI(Unit unit)
    {
      
        this.unit = unit;
       // Debug.Log("CharFace: "+CharacterFace);
        Debug.Log("ShowUnitCharView: "+unit);
        if(CharacterFace!=null)
            CharacterFace.Show(unit);
        charName.SetText(unit.name);//+", "+unit.jobClass);
        bool physical = unit.equippedWeapon.DamageType == DamageType.Physical;
        int sumBonuses = unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Attack,physical);
        Atk.SetValue(unit.BattleComponent.BattleStats.GetDamage(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.AttackSpeed,physical);
        AtkSpeed.SetValue(unit.BattleComponent.BattleStats.GetAttackSpeed(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.PhysicalResistance,physical);
        PhysArmor.SetValue(unit.BattleComponent.BattleStats.GetPhysicalResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.MagicResistance,physical);
        MagicArmor.SetValue(unit.BattleComponent.BattleStats.GetFaithResistance(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Hit,physical);
        Hitrate.SetValue(unit.BattleComponent.BattleStats.GetHitrate(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Avoid,physical);
        DodgeRate.SetValue(unit.BattleComponent.BattleStats.GetAvoid(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Crit,physical);
        Crit.SetValue(unit.BattleComponent.BattleStats.GetCrit(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);
        sumBonuses= unit.Stats.GetCombatStatBonuses(unit,BonusStats.CombatStatType.Critavoid,physical);
        CritAvoid.SetValue(unit.BattleComponent.BattleStats.GetCritAvoid(), sumBonuses > 0 ?AttributeBonusState.Increasing: sumBonuses<0? AttributeBonusState.Decreasing: AttributeBonusState.Same);

        
        STR.SetValue(unit.Stats.CombinedAttributes().STR,unit.Stats.GetAttributeBonusState(AttributeType.STR));
        INT.SetValue(unit.Stats.CombinedAttributes().INT,unit.Stats.GetAttributeBonusState(AttributeType.INT));
        DEX.SetValue(unit.Stats.CombinedAttributes().DEX,unit.Stats.GetAttributeBonusState(AttributeType.DEF));
        AGI.SetValue(unit.Stats.CombinedAttributes().AGI,unit.Stats.GetAttributeBonusState(AttributeType.AGI));
        CON.SetValue(unit.Stats.CombinedAttributes().CON,unit.Stats.GetAttributeBonusState(AttributeType.CON));
        FTH.SetValue(unit.Stats.CombinedAttributes().FAITH,unit.Stats.GetAttributeBonusState(AttributeType.FTH));
        LCK.SetValue(unit.Stats.CombinedAttributes().LCK,unit.Stats.GetAttributeBonusState(AttributeType.LCK));
        DEF.SetValue(unit.Stats.CombinedAttributes().DEF,unit.Stats.GetAttributeBonusState(AttributeType.DEF));
    }

    public void CombatStatsButtonClicked()
    {
        baseAttributeButton.GetComponent<CanvasGroup>().alpha = 1;
        combatStatsButton.GetComponent<CanvasGroup>().alpha = .6f;
        baseAttributePanel.SetActive(false);
        combatStatsPanel.SetActive(true);
    }
    public void BaseAttributeButtonClicked()
    {
        baseAttributeButton.GetComponent<CanvasGroup>().alpha = .6f;
        combatStatsButton.GetComponent<CanvasGroup>().alpha = 1f;
        baseAttributePanel.SetActive(true);
        combatStatsPanel.SetActive(false);
    }
    public void STR_Clicked()
    {
        Debug.Log("STR CLICKED");
        ToolTipSystem.ShowAttributeValue(unit, AttributeType.STR, STR.transform.position);
        //ToolTipSystem.ShowAttribute("Strength", "Increases ones physical damage output!",unit.Stats.BaseAttributes.STR, STR.transform.position);
    }
    public void INT_Clicked()
    {
        Debug.Log("INT CLICKED");
        ToolTipSystem.ShowAttributeValue(unit, AttributeType.INT, INT.transform.position);
        //ToolTipSystem.ShowAttribute("Intelligence", "Increases ones magical damage output and something else!",unit.Stats.BaseAttributes.INT,INT.transform.position);
    }
    public void DEX_Clicked()
    {
        
        ToolTipSystem.ShowAttributeValue(unit, AttributeType.DEX, DEX.transform.position);
        //ToolTipSystem.ShowAttribute("Dexterity", "Increases ones accuracy and influences critical hitrate!",unit.Stats.BaseAttributes.DEX,DEX.transform.position);
    }
    public void AGI_Clicked()
    {
        ToolTipSystem.ShowAttributeValue(unit, AttributeType.AGI, AGI.transform.position);
       // ToolTipSystem.ShowAttribute("Agility", "Increases ones ability to dodge as well as attack speed! Having 5 higher than your opponent allows for double attacks",unit.Stats.BaseAttributes.AGI,AGI.transform.position);
    }
    public void DEF_Clicked()
    {
        ToolTipSystem.ShowAttributeValue(unit, AttributeType.DEF, DEF.transform.position);
        //ToolTipSystem.ShowAttribute("Defense", "Increases your physical damage resistance!",unit.Stats.BaseAttributes.DEF,DEF.transform.position);
    }
    public void LCK_Clicked()
    {
        ToolTipSystem.ShowAttributeValue(unit, AttributeType.LCK, LCK.transform.position);
        //ToolTipSystem.ShowAttribute("Luck", "Increases ones critical hit rate and many other things!",unit.Stats.BaseAttributes.LCK,LCK.transform.position);
    }
    public void CON_Clicked()
    {
        ToolTipSystem.ShowAttributeValue(unit, AttributeType.CON, CON.transform.position);
        //ToolTipSystem.ShowAttribute("Constitution", "Increases ones maximum Hitpoints and allows to wield heavier weapons!",unit.Stats.BaseAttributes.CON,CON.transform.position);
    }
    public void FTH_Clicked()
    {
        ToolTipSystem.ShowAttributeValue(unit, AttributeType.FTH, FTH.transform.position);
       // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
    public void Attack_Clicked()
    {
        ToolTipSystem.ShowCombatStatValue(unit, BonusStats.CombatStatType.Attack, Atk.transform.position);
        // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
    public void Hit_Clicked()
    {
        ToolTipSystem.ShowCombatStatValue(unit, BonusStats.CombatStatType.Hit, Hitrate.transform.position);
        // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
    public void Avoid_Clicked()
    {
        ToolTipSystem.ShowCombatStatValue(unit, BonusStats.CombatStatType.Avoid, DodgeRate.transform.position);
        // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
    public void Crit_Clicked()
    {
        ToolTipSystem.ShowCombatStatValue(unit, BonusStats.CombatStatType.Crit, Crit.transform.position);
        // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
    public void CritAvoid_Clicked()
    {
        ToolTipSystem.ShowCombatStatValue(unit, BonusStats.CombatStatType.Critavoid, CritAvoid.transform.position);
        // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
    public void PhysResistance_Clicked()
    {
        ToolTipSystem.ShowCombatStatValue(unit, BonusStats.CombatStatType.PhysicalResistance, PhysArmor.transform.position);
        // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
    public void MagResistance_Clicked()
    {
        ToolTipSystem.ShowCombatStatValue(unit, BonusStats.CombatStatType.MagicResistance, MagicArmor.transform.position);
        // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
    public void AttackSpeed_Clicked()
    {
        ToolTipSystem.ShowCombatStatValue(unit, BonusStats.CombatStatType.AttackSpeed, AtkSpeed.transform.position);
        // ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
    }
    
    public virtual void Hide()
    {
        canvas.enabled = false;
    }
    public void UpdateUnit(Unit unit1)
    {
       UpdateUI(unit1);
    }
}