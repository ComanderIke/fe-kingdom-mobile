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
    public void Show(Human unit)
    {
        Atk.SetText(""+unit.BattleComponent.BattleStats.GetDamage());
        Armor.SetText(""+unit.Stats.Armor);

        if(unit.EquippedWeapon!=null)
            WeaponSlot.sprite = unit.EquippedWeapon.Sprite;
        if(unit.EquippedArmor!=null)
            ArmorSlot.sprite = unit.EquippedArmor.Sprite;
        if(unit.EquippedRelic!=null)
            RelicSlot.sprite = unit.EquippedRelic.Sprite;
        if(unit.EquippedConsumable!=null)
            ConsumeableSlot.sprite = unit.EquippedConsumable.Sprite;
    }
}