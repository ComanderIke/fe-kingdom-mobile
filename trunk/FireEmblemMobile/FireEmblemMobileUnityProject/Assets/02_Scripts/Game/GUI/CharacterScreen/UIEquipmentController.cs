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

    public Color inActiveColor;
    [FormerlySerializedAs("weaponBp")] public Weapon weapon;
    [FormerlySerializedAs("relicBp")] public Relic relic;
    public Relic relic2;

    public void RelicSlotUpperClicked()
    {
        ToolTipSystem.Show(relic,RelicSlotUpper.transform.position, relic.Name, relic.Description, relic.GetIcon());
    }
    public void RelicSlotLowerClicked()
    {
        ToolTipSystem.Show(relic2,RelicSlotLower.transform.position, relic2.Name, relic2.Description, relic2.GetIcon());
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
}