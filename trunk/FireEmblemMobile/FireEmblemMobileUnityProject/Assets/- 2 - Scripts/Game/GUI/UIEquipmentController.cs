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
    public Image RelicSlot;

    public Color inActiveColor;
    public void Show(Human unit)
    {
        Atk.SetText(""+unit.BattleComponent.BattleStats.GetDamage());


        if (unit.EquippedWeapon != null)
        {
            WeaponSlot.sprite = unit.EquippedWeapon.Sprite;
            WeaponSlot.color=Color.white;
        }
        else
        {
            WeaponSlot.color = inActiveColor;
        }
 

        if (unit.EquippedRelic != null)
        {
            RelicSlot.sprite = unit.EquippedRelic.Sprite;
            RelicSlot.color=Color.white;
        }
        else
        {
            RelicSlot.color = inActiveColor;
        }

     
    }
}