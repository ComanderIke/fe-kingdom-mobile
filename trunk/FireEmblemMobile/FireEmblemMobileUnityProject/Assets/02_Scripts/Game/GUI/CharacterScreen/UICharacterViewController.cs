using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.OnGameObject;
using Game.GUI;
using Game.Mechanics.Battle;
using Game.WorldMapStuff.Model;
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
    
    public TextMeshProUGUI Atk;
    public TextMeshProUGUI AtkSpeed;
    public TextMeshProUGUI PhysArmor;
    public TextMeshProUGUI MagicArmor;
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
    public Color AttributeTextColor;
    public Color BuffedAttributeTextColor;
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

    public void Show(Unit unit)
    {
        canvas.enabled = true;
        Player.Instance.Party.SetActiveUnit(unit);
        Debug.Log("Showing Character UI for: "+unit.name);
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
      
        Atk.SetText(""+unit.BattleComponent.BattleStats.GetAttackDamage()+"");
        AtkSpeed.SetText(""+unit.BattleComponent.BattleStats.GetAttackSpeed()+"");
        PhysArmor.SetText(""+unit.BattleComponent.BattleStats.GetPhysicalResistance()+"");
        MagicArmor.SetText(""+unit.BattleComponent.BattleStats.GetFaithResistance()+"");
        Hitrate.SetText(""+unit.BattleComponent.BattleStats.GetHitrate()+"%");
        DodgeRate.SetText(""+unit.BattleComponent.BattleStats.GetAvoid()+"%");
        Crit.SetText(""+unit.BattleComponent.BattleStats.GetCrit()+"%");
        CritAvoid.SetText(""+unit.BattleComponent.BattleStats.GetCritAvoid()+"%");

        if (unit.Stats.BonusAttributesFromEffects.STR > 0)
            STR.color = BuffedAttributeTextColor;
        else
        {
            STR.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributesFromEffects.DEX > 0)
            DEX.color = BuffedAttributeTextColor;
        else
        {
            DEX.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributesFromEffects.INT > 0)
            INT.color = BuffedAttributeTextColor;
        else
        {
            INT.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributesFromEffects.AGI > 0)
            AGI.color = BuffedAttributeTextColor;
        else
        {
            AGI.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributesFromEffects.CON > 0)
            CON.color = BuffedAttributeTextColor;
        else
        {
            CON.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributesFromEffects.LCK > 0)
            LCK.color = BuffedAttributeTextColor;
        else
        {
            LCK.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributesFromEffects.DEF > 0)
            DEF.color = BuffedAttributeTextColor;
        else
        {
            DEF.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributesFromEffects.FAITH > 0)
            FTH.color = BuffedAttributeTextColor;
        else
        {
            FTH.color = AttributeTextColor;
        }
        STR.SetText(""+(unit.Stats.CombinedAttributes().STR));
        INT.SetText(""+(unit.Stats.CombinedAttributes().INT));
        DEX.SetText(""+(unit.Stats.CombinedAttributes().DEX));
        AGI.SetText(""+(unit.Stats.CombinedAttributes().AGI));
        CON.SetText(""+(unit.Stats.CombinedAttributes().CON));
        FTH.SetText(""+(unit.Stats.CombinedAttributes().FAITH));
        LCK.SetText(""+(unit.Stats.CombinedAttributes().LCK));
        DEF.SetText(""+(unit.Stats.CombinedAttributes().DEF));
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