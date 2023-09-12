using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : BuyItemUI
{
    
    public TextMeshProUGUI hitAfter;
    public TextMeshProUGUI dmgAfter;
    public TextMeshProUGUI critAfter;
    public TextMeshProUGUI stoneCost;
    public Image cost2Icon;
    public Sprite stoneIcon;
    public Sprite dragonScaleIcon;


    // Start is called before the first frame update
    public void Show(EquipableItem equip,WeaponUpgradeMode upgradeMode, int upgradegoldCost, int upgradeStoneCost, int dragonScaleCost, bool affordable)
    {
        base.Show(equip, affordable, true);
        cost.text = "" + upgradegoldCost;

        if (upgradeStoneCost != 0)
        {
            stoneCost.gameObject.SetActive(true);
            cost2Icon.gameObject.SetActive(true);
            stoneCost.text = "" + upgradeStoneCost;
            cost2Icon.sprite = stoneIcon;
           
            stoneCost.color = Player.Instance.Party.Convoy.GetItemCount(GameBPData.Instance.GetSmithingStone().Name) >= upgradeStoneCost? textNormalColor:tooExpensiveTextColor;
          
        }
        else if (dragonScaleCost != 0)
        {
            stoneCost.gameObject.SetActive(true);
            cost2Icon.gameObject.SetActive(true);
            cost2Icon.sprite = dragonScaleIcon;
            stoneCost.text = "" + dragonScaleCost;
            stoneCost.color = Player.Instance.Party.Convoy.GetItemCount(GameBPData.Instance.GetDragonScale().Name) >= dragonScaleCost? textNormalColor:tooExpensiveTextColor;
        }
        else
        {
            stoneCost.gameObject.SetActive(false);
            cost2Icon.gameObject.SetActive(false);
        }

        buyButton.interactable = affordable &&
                              Player.Instance.Party.Convoy.GetItemCount(GameBPData.Instance.GetDragonScale().Name) >=
                              dragonScaleCost &&
                              Player.Instance.Party.Convoy.GetItemCount(GameBPData.Instance.GetSmithingStone().Name) >=
                              upgradeStoneCost;
        buttonText.text = "Upgrade";
        if (equip is Weapon weapon)
        {
            hitAfter.gameObject.SetActive(upgradeMode== WeaponUpgradeMode.Accuracy);
            critAfter.gameObject.SetActive(upgradeMode== WeaponUpgradeMode.Critical);
            dmgAfter.gameObject.SetActive(upgradeMode== WeaponUpgradeMode.Power);
            critAfter.text = "" +(weapon.GetCrit()+ weapon.GetUpgradeableCrit());
            hitAfter.text = "" + (weapon.GetHit()+weapon.GetUpgradeableHit());
            dmgAfter.text = "" + (weapon.GetDamage()+weapon.GetUpgradeableDmg());
        }
        
    }
}