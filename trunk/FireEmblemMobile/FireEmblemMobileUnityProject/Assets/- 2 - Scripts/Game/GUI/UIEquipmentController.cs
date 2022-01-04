using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentController:MonoBehaviour
{
    public TextMeshProUGUI Atk;
    public TextMeshProUGUI Armor;
    public Image ArmorSlot;
    public Image WeaponSlot;
    public Image RelicSlot;
    public Image ConsumeableSlot;
    public Color inActiveColor;
    public void Show(Human unit)
    {
        Atk.SetText(""+unit.BattleComponent.BattleStats.GetDamage());
        Armor.SetText(""+unit.BattleComponent.BattleStats.GetArmor());

        if (unit.EquippedWeapon != null)
        {
            WeaponSlot.sprite = unit.EquippedWeapon.Sprite;
            WeaponSlot.color=Color.white;
        }
        else
        {
            WeaponSlot.color = inActiveColor;
        }
        if(unit.EquippedArmor!=null){
            ArmorSlot.sprite = unit.EquippedArmor.Sprite;
            ArmorSlot.color=Color.white;
        }
        else
        {
            ArmorSlot.color = inActiveColor;
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

        if (unit.EquippedConsumable != null)
        {
            ConsumeableSlot.sprite = unit.EquippedConsumable.Sprite;
            ConsumeableSlot.color=Color.white;
        }
        else
        {
            ConsumeableSlot.color = inActiveColor;
        }
    }
}