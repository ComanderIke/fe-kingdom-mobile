using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : BuyItemUI
{

    public TextMeshProUGUI weightAfter;
    public TextMeshProUGUI hitAfter;
    public TextMeshProUGUI dmgAfter;
    public TextMeshProUGUI critAfter;
    public TextMeshProUGUI effectAfter;
    public TextMeshProUGUI stoneCost;
    public Image cost2Icon;
    public Sprite stoneIcon;
    public Sprite dragonScaleIcon;
    [SerializeField]  TextMeshProUGUI relicEffectAfter;
   

    // Start is called before the first frame update
    public void Show(EquipableItem equip,int upgradegoldCost, int upgradeStoneCost, int dragonScaleCost, bool affordable)
    {
        base.Show(equip, affordable, affordable);
        cost.text = "" + upgradegoldCost;

        if (upgradeStoneCost != 0)
        {
            stoneCost.gameObject.SetActive(true);
            cost2Icon.gameObject.SetActive(true);
            stoneCost.text = "" + upgradeStoneCost;
            cost2Icon.sprite = stoneIcon;
        }
        else if (dragonScaleCost != 0)
        {
            stoneCost.gameObject.SetActive(true);
            cost2Icon.gameObject.SetActive(true);
            cost2Icon.sprite = dragonScaleIcon;
            stoneCost.text = "" + dragonScaleCost;
        }
        else
        {
            stoneCost.gameObject.SetActive(false);
            cost2Icon.gameObject.SetActive(false);
        }

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