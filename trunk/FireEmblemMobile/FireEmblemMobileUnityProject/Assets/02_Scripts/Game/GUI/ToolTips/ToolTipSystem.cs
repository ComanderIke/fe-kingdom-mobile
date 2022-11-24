using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;
using UnityEngine.Serialization;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem instance;
    public ItemToolTip ItemToolTip;
    public WeaponToolTip WeaponToolTip;
    [FormerlySerializedAs("SkillToolTip")] public SkillTreeToolTip skillTreeToolTip;
    public SkillToolTip skillToolTip;
    public AttributeToolTip AttributeToolTip;
    public EncounterToolTip EncounterToolTip;
    public EncounterToolTip EncounterAttackToolTip;

    public void Awake()
    {
        instance = this;
    }

    public static void ShowEncounter(EncounterNode node, Vector3 worldPosition, bool moveable, Action<EncounterNode> moveClicked)
    {
        if (!(node is BattleEncounterNode))
        {
            instance.EncounterToolTip.Updatevalues(node, node.label, worldPosition, moveable, moveClicked);
            instance.EncounterToolTip.gameObject.SetActive(true);
            instance.EncounterAttackToolTip.gameObject.SetActive(false);
        }
        else
        {
            instance.EncounterAttackToolTip.Updatevalues(node, node.label, worldPosition, moveable, moveClicked);
            instance.EncounterAttackToolTip.gameObject.SetActive(true);
            instance.EncounterToolTip.gameObject.SetActive(false);
        }
    }
    public static void Show(Item item, Vector3 position, string header, string description, Sprite icon)
    {
        instance.ItemToolTip.SetValues(item, header,description,icon, position);
        
        instance.ItemToolTip.gameObject.SetActive(true);
    }
    public static void Show(Weapon weapon, Vector3 position, string header, string description, Sprite icon)
    {
        instance.WeaponToolTip.SetValues(weapon, header,description,icon, position);
        
        instance.WeaponToolTip.gameObject.SetActive(true);
    }
    public static void ShowSkill(Skill skill, Vector3 position)
    {
        instance.skillToolTip.SetValues(skill, skill.Name,skill.Description,skill.Icon, position);
        
        instance.skillToolTip.gameObject.SetActive(true);
    }
    public static void ShowSkill(SkillTreeEntryUI skillTreeEntry, Vector3 position, string header, string description, Sprite icon)
    {
        instance.skillTreeToolTip.SetValues(skillTreeEntry, header,description,icon, position);
        
        instance.skillTreeToolTip.gameObject.SetActive(true);
    }
    public static void HideSkill()
    {
        instance.skillTreeToolTip.gameObject.SetActive(false);
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