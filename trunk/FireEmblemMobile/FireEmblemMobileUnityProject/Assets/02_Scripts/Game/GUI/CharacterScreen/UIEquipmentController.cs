using System;
using Game.GameActors.Items;
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
//    [SerializeField] UICombatItemSlot combatItemSlot2;
    [SerializeField] private UIConvoyController convoy;
    [SerializeField] private bool openConvoyWhenEquipmentClicked = true;

    public Color inActiveColor;
    [FormerlySerializedAs("weaponBp")] public Weapon weapon;
    [NonSerialized] private Relic relic;
    [NonSerialized] private StockedCombatItem combatItem1;
  //  [NonSerialized] private IEquipableCombatItem combatItem2;
    public SmithingSlot selectedSlot = null;
    [SerializeField] private Material blessedMaterial;

    [SerializeField]Image equipmentLineImage;
    public UICombatItemSlot selectedSlotCombatItemSlot = null;

    public void RelicSlotUpperClicked()
    {
        if (relic != null)
        {
            
            ToolTipSystem.Show(new StockedItem(relic,1), RelicSlotUpper.transform.position);
          
        }

        if (!openConvoyWhenEquipmentClicked)
            return;
        // else
        // {
            if (selectedSlot != null)
                selectedSlot.Deselect();
            selectedSlot = RelicSlotUpper;
     
            RelicSlotUpper.Select();
            convoy.Show(typeof(Relic), UIConvoyController.ConvoyContext.SelectRelic);
        // }
    }
    public void CombatItem1Clicked()
    {
        if (combatItem1 != null)
        {
            ToolTipSystem.Show(new StockedItem((Item)combatItem1.item, combatItem1.stock), combatItemSlot1.transform.position);
          
        }
        if (!openConvoyWhenEquipmentClicked)
            return;
        // else
        // {
        if (selectedSlotCombatItemSlot != null)
            selectedSlotCombatItemSlot.Deselect();
        selectedSlotCombatItemSlot = combatItemSlot1;
       // selectedSlotNumber = 1;
       combatItemSlot1.Select();
       if(convoy!=null)
        convoy.Show(typeof(IEquipableCombatItem), UIConvoyController.ConvoyContext.SelectCombatItem);
        // }
    }
    // public void CombatItem2Clicked()
    // {
    //     if (combatItem2 != null)
    //     {
    //         var item = (Item)combatItem2;
    //         ToolTipSystem.Show(item, combatItemSlot2.transform.position);
    //       
    //     }
    //     // else
    //     // {
    //     if (selectedSlotCombatItemSlot != null)
    //         selectedSlotCombatItemSlot.Deselect();
    //     selectedSlotCombatItemSlot = combatItemSlot2;
    //     // selectedSlotNumber = 1;
    //     combatItemSlot2.Select();
    //     convoy.Show(typeof(IEquipableCombatItem), UIConvoyController.ConvoyContext.SelectCombatItem);
    //     // }
    // }
    
    public void WeaponSlotClicked()
    {
        
        ToolTipSystem.Show(weapon,WeaponSlot.transform.position);
    }
    public void Show(Unit unit)
    {
        WeaponSlot.Show(unit, unit.equippedWeapon);
        if (unit.equippedWeapon != null)
        {
          
            weapon = unit.equippedWeapon;
        }
        else
        {
            weapon = null;
        }
        equipmentLineImage.material = unit.Blessing==null?null:blessedMaterial;
        RelicSlotUpper.Show(unit, unit.EquippedRelic);
        relic = unit.EquippedRelic;
        combatItemSlot1.Show(unit.CombatItem1);
        if(unit.CombatItem1!=null)
            combatItem1 = unit.CombatItem1;
        else
        {
            combatItem1 = null;
        }
        // combatItemSlot2.Show(unit.CombatItem2);
        // if (unit.CombatItem2 != null)
        // {
        //     combatItem2 =(IEquipableCombatItem) unit.CombatItem2.item;
        // }
        // else
        // {
        //     combatItem2 = null;
        // }
      
       


    }

    public void Hide()
    {
        if (selectedSlot != null)
        {
            selectedSlot.Deselect();
            selectedSlot = null;
        }
        if (selectedSlotCombatItemSlot != null)
        {
            selectedSlotCombatItemSlot.Deselect();
            selectedSlotCombatItemSlot = null;
        }
    }

    public void HighlightRelicSlots()
    {
        RelicSlotUpper.Highlight();
    }
}

