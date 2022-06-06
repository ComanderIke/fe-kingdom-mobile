using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units.Skills;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem instance;
    public ItemToolTip ItemToolTip;
    public SkillToolTip SkillToolTip;
    public AttributeToolTip AttributeToolTip;
    public EncounterToolTip EncounterToolTip;

    public void Awake()
    {
        instance = this;
    }

    public static void ShowEncounter(EncounterNode node, Vector3 worldPosition, bool moveable, Action<EncounterNode> moveClicked)
    {
        instance.EncounterToolTip.Updatevalues(node, node.ToString(), worldPosition, moveable, moveClicked);
        instance.EncounterToolTip.gameObject.SetActive(true);
    }
    public static void Show(Item item, Vector3 position, string header, string description, Sprite icon)
    {
        instance.ItemToolTip.SetValues(item, header,description,icon, position);
        
        instance.ItemToolTip.gameObject.SetActive(true);
    }
    public static void ShowSkill(SkillUI skill, Vector3 position, string header, string description, Sprite icon)
    {
        instance.SkillToolTip.SetValues(skill, header,description,icon, position);
        
        instance.SkillToolTip.gameObject.SetActive(true);
    }
    public static void HideSkill()
    {
        instance.SkillToolTip.gameObject.SetActive(false);
    }

    public static void Hide()
    {
        instance.ItemToolTip.gameObject.SetActive(false);
    }

    public static void ShowAttribute(string header, string description, int value, Vector3 position)
    {
        instance.AttributeToolTip.gameObject.SetActive(true);
        instance.AttributeToolTip.SetValues(header,description, value, position);
    }
    public static void HideAttribute()
    {
        instance.AttributeToolTip.gameObject.SetActive(false);
    }
}