using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using Game.Mechanics.Battle;
using Game.Utility;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem instance;
    public ItemToolTip ItemToolTip;
    public WeaponToolTip WeaponToolTip;
    public SkillToolTip skillToolTip;

    public AttributeValueTooltipUI AttributeValueTooltipUI;
    public CombatStatValueTooltipUI CombatStatalueTooltipUI;
   
    public UITimeOfDayTooltip TimeOfDayTooltip;
    public UIMoralityBarTooltip MoralityBarTooltip;
    public void Awake()
    {
        instance = this;
    }

    static void CloseAllToolTips()
    {
       
      
        instance.WeaponToolTip.gameObject.SetActive(false);

        if(instance.MoralityBarTooltip!=null)
            instance.MoralityBarTooltip.gameObject.SetActive(false);
        instance.skillToolTip.gameObject.SetActive(false);
      
        //instance.skillTreeToolTip.gameObject.SetActive(false);
        instance.ItemToolTip.gameObject.SetActive(false);
        instance.CombatStatalueTooltipUI.gameObject.SetActive(false);
        instance.TimeOfDayTooltip.gameObject.SetActive(false);
        instance.AttributeValueTooltipUI.gameObject.SetActive(false);
        
    }
    
    public static void Show(Item item, Vector3 position, bool screenPos=false, bool exactPos=false)
    {
        Show(new StockedItem(item, 1), position, screenPos, exactPos);
    }
    public static void Show(StockedItem item, Vector3 position, bool screenPos=false, bool exactPos=false)
    {
        MyDebug.LogInput("Show StockedItem Tooltip" + item.item.Name);
        instance.tooltipShownThisFrame = true;
        CloseAllToolTips();

        instance.ItemToolTip.SetValues(item, screenPos?position:Camera.main.WorldToScreenPoint(position), exactPos);
        
        instance.ItemToolTip.gameObject.SetActive(true);
    }
    public static void Show(Weapon weapon, Vector3 position)
    {
        MyDebug.LogInput("Show Weapon Tooltip");
        instance.tooltipShownThisFrame = true;
        CloseAllToolTips();
        instance.WeaponToolTip.SetValues(weapon, Camera.main.WorldToScreenPoint(position));
        
        instance.WeaponToolTip.gameObject.SetActive(true);
    }
   
    private bool tooltipShownThisFrame = false;
    private void Update()
    {
     
        if(InputUtility.TouchEnd()&&!tooltipShownThisFrame)
            CloseAllToolTips();
        
        instance.tooltipShownThisFrame = false;
    }

    public static void Show(Skill skill, bool blessed,Vector3 position)
    {
        instance.tooltipShownThisFrame = true;
        CloseAllToolTips();
        //Debug.Log(skill.Name);
        //Debug.Log("TooltipPosition: "+GameObject.FindWithTag("UICamera").GetComponent<Camera>().WorldToScreenPoint(position));
        instance.skillToolTip.SetValues(skill, blessed, false,Camera.main.WorldToScreenPoint(position));
        
        instance.skillToolTip.gameObject.SetActive(true);
    }
    
   

    public static void Hide()
    {
        instance.ItemToolTip.gameObject.SetActive(false);
    }
    
    public static void ShowAttributeValue(Unit unit,AttributeType attributeType, Vector3 position)
    {
        instance.tooltipShownThisFrame = true;
        CloseAllToolTips();
        // Debug.Log("transformPos: "+position+" ScreenPos"+Camera.main.WorldToScreenPoint(position));
        instance.AttributeValueTooltipUI.gameObject.SetActive(true);
        instance.AttributeValueTooltipUI.Show(unit,  attributeType, Camera.main.WorldToScreenPoint(position));
    }
    public static void ShowCombatStatValue(Unit unit,CombatStats.CombatStatType combatStatType, Vector3 position)
    {
        instance.tooltipShownThisFrame = true;
        CloseAllToolTips();
        // Debug.Log("transformPos: "+position+" ScreenPos"+Camera.main.WorldToScreenPoint(position));
        instance.CombatStatalueTooltipUI.gameObject.SetActive(true);
        instance.CombatStatalueTooltipUI.Show(unit,  combatStatType, Camera.main.WorldToScreenPoint(position));
    }


    public static void ShowTimeOfDay(float hour, TimeOfDayBonuses bonus)
    {
        instance.tooltipShownThisFrame = true;
        CloseAllToolTips();
        instance.TimeOfDayTooltip.Show((int) hour, bonus);
        instance.TimeOfDayTooltip.gameObject.SetActive(true);
    }

    public static void ShowMorality(float morality)
    {
        instance.tooltipShownThisFrame = true;
        CloseAllToolTips();
        instance.MoralityBarTooltip.Show(morality);
        instance.MoralityBarTooltip.gameObject.SetActive(true);
    }
}