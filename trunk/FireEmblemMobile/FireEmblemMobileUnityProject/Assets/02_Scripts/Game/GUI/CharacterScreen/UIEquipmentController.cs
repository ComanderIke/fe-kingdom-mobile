using System;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIEquipmentController:MonoBehaviour
{



    public SmithingSlot WeaponSlot;
    public SmithingSlot RelicSlotUpper;
    [SerializeField] UICombatItemSlot combatItemSlot1;
    [SerializeField] UICombatItemSlot combatItemSlot2;
    [SerializeField] private UIConvoyController convoy;

    public Color inActiveColor;
    [FormerlySerializedAs("weaponBp")] public Weapon weapon;
    [NonSerialized] public Relic relic;
    public SmithingSlot selectedSlot = null;
    public int selectedSlotNumber=-1;

    public void RelicSlotUpperClicked()
    {
        if (relic != null)
        {
            
            ToolTipSystem.Show(relic, RelicSlotUpper.transform.position, relic.Name, relic.Description,
                relic.GetIcon());
          
        }
        // else
        // {
            if (selectedSlot != null)
                selectedSlot.Deselect();
            selectedSlot = RelicSlotUpper;
            selectedSlotNumber = 1;
            RelicSlotUpper.Select();
            convoy.Show(typeof(Relic), UIConvoyController.ConvoyContext.SelectRelic);
        // }
    }
    
    public void WeaponSlotClicked()
    {
        Debug.Log("ClickedWeapobn");
        ToolTipSystem.Show(weapon,WeaponSlot.transform.position, weapon.Name, weapon.Description, weapon.GetIcon());
    }
    public void Show(Unit unit)
    {
        WeaponSlot.Show(unit.equippedWeapon);
        if (unit.equippedWeapon != null)
        {
          
            weapon = unit.equippedWeapon;
        }
        else
        {
            weapon = null;
        }
 
        RelicSlotUpper.Show(unit.EquippedRelic);
        relic = unit.EquippedRelic;
        combatItemSlot1.Show(unit.CombatItem1);
        combatItemSlot2.Show(unit.CombatItem2);


    }

    public void Hide()
    {
        selectedSlotNumber = -1;
        if (selectedSlot != null)
        {
            selectedSlot.Deselect();
            selectedSlot = null;
        }
    }

    public void HighlightRelicSlots()
    {
        RelicSlotUpper.Highlight();
    }
}