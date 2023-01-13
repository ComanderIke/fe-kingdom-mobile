using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units.Skills;
using Game.Utility;
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
    public BlessingToolTip blessingToolTip;
    public RelicToolTip relicToolTip;
    public void Awake()
    {
        instance = this;
    }

    static void CloseAllToolTips()
    {
        instance.blessingToolTip.gameObject.SetActive(false);
        instance.EncounterAttackToolTip.gameObject.SetActive(false);
        instance.relicToolTip.gameObject.SetActive(false);
        instance.WeaponToolTip.gameObject.SetActive(false);
        instance.EncounterToolTip.gameObject.SetActive(false);
       // instance.skillToolTip.gameObject.SetActive(false);
        instance.AttributeToolTip.gameObject.SetActive(false);
        instance.skillTreeToolTip.gameObject.SetActive(false);
        instance.ItemToolTip.gameObject.SetActive(false);
    }
    static void CloseAllToolTipsExceptEncounter()
    {
        instance.blessingToolTip.gameObject.SetActive(false);
       
        instance.relicToolTip.gameObject.SetActive(false);
        instance.WeaponToolTip.gameObject.SetActive(false);
       
        // instance.skillToolTip.gameObject.SetActive(false);
        instance.AttributeToolTip.gameObject.SetActive(false);
        instance.skillTreeToolTip.gameObject.SetActive(false);
        instance.ItemToolTip.gameObject.SetActive(false);
    }
    static void CloseAllEncounterTooltips()
    {
        instance.EncounterAttackToolTip.gameObject.SetActive(false);
        instance.EncounterToolTip.gameObject.SetActive(false);
    }
    public static void ShowEncounter(EncounterNode node, Vector3 worldPosition, bool moveable, Action<EncounterNode> moveClicked)
    {
        CloseAllToolTips();
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
    public static void Show(Relic relic, Vector3 position, string header, string description, Sprite icon)
    {
        CloseAllToolTips();
        Debug.Log("Show Relic Tooltip");
        instance.relicToolTip.SetValues(relic, header,description,icon, Camera.main.WorldToScreenPoint(position));
        
        instance.relicToolTip.gameObject.SetActive(true);
    }
    public static void Show(Item item, Vector3 position, string header, string description, Sprite icon)
    {
        CloseAllToolTips();
        if (item is Relic relic)
        {
            Show(relic, position, header, description, icon);
            return;
        }

        instance.ItemToolTip.SetValues(item, header,description,icon, Camera.main.WorldToScreenPoint(position));
        
        instance.ItemToolTip.gameObject.SetActive(true);
    }
    public static void Show(Weapon weapon, Vector3 position, string header, string description, Sprite icon)
    {
        CloseAllToolTips();
        instance.WeaponToolTip.SetValues(weapon, header,description,icon, Camera.main.WorldToScreenPoint(position));
        
        instance.WeaponToolTip.gameObject.SetActive(true);
    }
    public static void Show(CurseBlessBase blessing, Vector3 position)
    {
        CloseAllToolTips();
    
        instance.blessingToolTip.SetValues(blessing, blessing.Name,blessing.Skill.Description,blessing.Skill.Icon, Camera.main.WorldToScreenPoint(position));
        
        instance.blessingToolTip.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!UIHelper.IsPointerOverUIObject())
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    CloseAllToolTips();
                }
            }
        }

    }

    public static void ShowSkill(Skill skill, Vector3 position)
    {
        CloseAllToolTips();
        Debug.Log(skill.Name);
        instance.skillToolTip.SetValues(skill, skill.Name,skill.Description,skill.Icon, Camera.main.WorldToScreenPoint(position));
        
        instance.skillToolTip.gameObject.SetActive(true);
    }
    public static void ShowSkill(SkillTreeEntryUI skillTreeEntry, Vector3 position, string header, string description, Sprite icon)
    {
        CloseAllToolTips();
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
        CloseAllToolTips();
        Debug.Log("transformPos: "+position+" ScreenPos"+Camera.main.WorldToScreenPoint(position));
        instance.AttributeToolTip.gameObject.SetActive(true);
        instance.AttributeToolTip.SetValues(header,description, value, Camera.main.WorldToScreenPoint(position));
    }
    public static void HideAttribute()
    {
        instance.AttributeToolTip.gameObject.SetActive(false);
    }

    
}