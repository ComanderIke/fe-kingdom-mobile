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
        CharacterFace.Show(unit);
        charName.SetText(unit.name);//+", "+unit.jobClass);
      
        
        Hitrate.SetText(""+unit.BattleComponent.BattleStats.GetHitrate()+"%");
        DodgeRate.SetText(""+unit.BattleComponent.BattleStats.GetAvoid()+"%");
        Crit.SetText(""+unit.BattleComponent.BattleStats.GetCrit()+"%");
        CritAvoid.SetText(""+unit.BattleComponent.BattleStats.GetCritAvoid()+"%");

        if (unit.Stats.BonusAttributes.STR > 0)
            STR.color = BuffedAttributeTextColor;
        else
        {
            STR.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributes.DEX > 0)
            DEX.color = BuffedAttributeTextColor;
        else
        {
            DEX.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributes.INT > 0)
            INT.color = BuffedAttributeTextColor;
        else
        {
            INT.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributes.AGI > 0)
            AGI.color = BuffedAttributeTextColor;
        else
        {
            AGI.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributes.CON > 0)
            CON.color = BuffedAttributeTextColor;
        else
        {
            CON.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributes.LCK > 0)
            LCK.color = BuffedAttributeTextColor;
        else
        {
            LCK.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributes.DEF > 0)
            DEF.color = BuffedAttributeTextColor;
        else
        {
            DEF.color = AttributeTextColor;
        }
        if (unit.Stats.BonusAttributes.FAITH > 0)
            FTH.color = BuffedAttributeTextColor;
        else
        {
            FTH.color = AttributeTextColor;
        }
        STR.SetText(""+(unit.Stats.BaseAttributes.STR+unit.Stats.BonusAttributes.STR));
        INT.SetText(""+(unit.Stats.BaseAttributes.INT+unit.Stats.BonusAttributes.INT));
        DEX.SetText(""+(unit.Stats.BaseAttributes.DEX+unit.Stats.BonusAttributes.DEX));
        AGI.SetText(""+(unit.Stats.BaseAttributes.AGI+unit.Stats.BonusAttributes.AGI));
        CON.SetText(""+(unit.Stats.BaseAttributes.CON+unit.Stats.BonusAttributes.CON));
        FTH.SetText(""+(unit.Stats.BaseAttributes.FAITH+unit.Stats.BonusAttributes.FAITH));
        LCK.SetText(""+(unit.Stats.BaseAttributes.LCK+unit.Stats.BonusAttributes.LCK));
        DEF.SetText(""+(unit.Stats.BaseAttributes.DEF+unit.Stats.BonusAttributes.DEF));
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
        ToolTipSystem.ShowAttribute("Strength", "Increases ones physical damage output!",unit.Stats.BaseAttributes.STR, STR.transform.position);
    }
    public void INT_Clicked()
    {
        ToolTipSystem.ShowAttribute("Intelligence", "Increases ones magical damage output and something else!",unit.Stats.BaseAttributes.INT,INT.transform.position);
    }
    public void DEX_Clicked()
    {
        ToolTipSystem.ShowAttribute("Dexterity", "Increases ones accuracy and influences critical hitrate!",unit.Stats.BaseAttributes.DEX,DEX.transform.position);
    }
    public void AGI_Clicked()
    {
        ToolTipSystem.ShowAttribute("Agility", "Increases ones ability to dodge as well as attack speed! Having 5 higher than your opponent allows for double attacks",unit.Stats.BaseAttributes.AGI,AGI.transform.position);
    }
    public void DEF_Clicked()
    {
        ToolTipSystem.ShowAttribute("Defense", "Increases your physical damage resistance!",unit.Stats.BaseAttributes.DEF,DEF.transform.position);
    }
    public void LCK_Clicked()
    {
        ToolTipSystem.ShowAttribute("Luck", "Increases ones critical hit rate and many other things!",unit.Stats.BaseAttributes.LCK,LCK.transform.position);
    }
    public void CON_Clicked()
    {
        ToolTipSystem.ShowAttribute("Constitution", "Increases ones maximum Hitpoints and allows to wield heavier weapons!",unit.Stats.BaseAttributes.CON,CON.transform.position);
    }
    public void FTH_Clicked()
    {
        ToolTipSystem.ShowAttribute("Faith", "Increases ones holy and occult damage and increases magical damage resistance!",unit.Stats.BaseAttributes.FAITH,FTH.transform.position);
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