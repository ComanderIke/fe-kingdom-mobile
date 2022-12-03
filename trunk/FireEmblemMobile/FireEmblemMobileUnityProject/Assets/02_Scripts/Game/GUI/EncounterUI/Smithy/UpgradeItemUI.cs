using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using TMPro;
using UnityEngine;

public class UpgradeItemUI : BuyItemUI
{

    public TextMeshProUGUI hitAfter;
    public TextMeshProUGUI dmgAfter;
    public TextMeshProUGUI critAfter;
    public TextMeshProUGUI effectAfter;
    public TextMeshProUGUI stoneCost;
    [SerializeField]  TextMeshProUGUI relicEffectAfter;
   

    // Start is called before the first frame update
    public void Show(EquipableItem equip, bool affordable)
    {
        base.Show(equip, affordable, affordable);
        cost.text = "" + equip.GetUpgradeCost();
        stoneCost.text = "" + equip.GetUpgradeSmithingStoneCost();
        effectAfter.text = "-";
        buttonText.text = "Upgrade";
        if (equip is Weapon weapon)
        {
            critAfter.text = "" + weapon.GetUpgradeableCrit();
            hitAfter.text = "" + weapon.GetUpgradeableHit();
            dmgAfter.text = "" + weapon.GetUpgradeableDmg();
        }

        if (equip is Relic relic)
        {
            relicEffectAfter.text = relic.GetUpgradeAttributeDescription();
        }
    }
}