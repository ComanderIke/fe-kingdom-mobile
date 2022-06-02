using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentController:MonoBehaviour
{
    public TextMeshProUGUI Atk;


    public Image WeaponSlot;
    public Image RelicSlotUpper;
    public Image RelicSlotLower;

    public Color inActiveColor;
    public Weapon weapon;
    public Relic relic;
    public Relic relic2;

    public void RelicSlotUpperClicked()
    {
        ToolTipSystem.Show(relic,RelicSlotUpper.transform.position, relic.name, relic.Description, relic.GetIcon());
    }
    public void RelicSlotLowerClicked()
    {
        ToolTipSystem.Show(relic2,RelicSlotLower.transform.position, relic2.name, relic2.Description, relic2.GetIcon());
    }
    public void WeaponSlotClicked()
    {
        Debug.Log("ClickedWeapobn");
        ToolTipSystem.Show(weapon,WeaponSlot.transform.position, weapon.name, weapon.Description, weapon.GetIcon());
    }
    public void Show(Human unit)
    {
        Atk.SetText(""+unit.BattleComponent.BattleStats.GetDamage());

        Debug.Log("Show");
        if (unit.EquippedWeapon != null)
        {
            Debug.Log("Weaponnotnull");
            weapon = unit.EquippedWeapon;
            WeaponSlot.sprite = unit.EquippedWeapon.Sprite;
            WeaponSlot.color=Color.white;
        }
        else
        {
            weapon = null;
            WeaponSlot.color = inActiveColor;
        }
 

        if (unit.EquippedRelic1 != null)
        {
            relic = unit.EquippedRelic1;
            RelicSlotUpper.sprite = unit.EquippedRelic1.Sprite;
            RelicSlotUpper.color=Color.white;
        }
        else
        {
            relic = null;
            RelicSlotUpper.color = inActiveColor;
        }
        if (unit.EquippedRelic2 != null)
        {
            relic2 = unit.EquippedRelic2;
            RelicSlotLower.sprite = unit.EquippedRelic2.Sprite;
            RelicSlotLower.color=Color.white;
        }
        else
        {
            relic2 = null;
            RelicSlotLower.color = inActiveColor;
        }

     
    }
}