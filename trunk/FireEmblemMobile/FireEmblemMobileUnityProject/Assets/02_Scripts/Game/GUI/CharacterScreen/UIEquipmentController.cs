using System;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIEquipmentController:MonoBehaviour
{



    public SmithingSlot WeaponSlot;
    public SmithingSlot RelicSlotUpper;
    public SmithingSlot RelicSlotLower;
    [SerializeField] private UIConvoyController convoy;

    public Color inActiveColor;
    [FormerlySerializedAs("weaponBp")] public Weapon weapon;
    [NonSerialized] public Relic relic;
    [NonSerialized]public Relic relic2;
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
    public void RelicSlotLowerClicked()
    {
        if (relic2 != null)
        {

            ToolTipSystem.Show(relic2, RelicSlotLower.transform.position, relic2.Name, relic2.Description,
                relic2.GetIcon());
         }
        // else
        // {
            if (selectedSlot != null)
                selectedSlot.Deselect();
            selectedSlot = RelicSlotLower;
            selectedSlotNumber = 2;
            RelicSlotLower.Select();
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
 
        RelicSlotUpper.Show(unit.EquippedRelic1);
        relic = unit.EquippedRelic1;
        RelicSlotLower.Show(unit.EquippedRelic2);
        relic2 = unit.EquippedRelic2;
        

     
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
        RelicSlotLower.Highlight();
    }
}