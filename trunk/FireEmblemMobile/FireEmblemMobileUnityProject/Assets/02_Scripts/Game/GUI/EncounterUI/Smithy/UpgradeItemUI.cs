using Game.DataAndReferences.Data;
using Game.GameActors.Items.Relics;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.EncounterUI.Smithy
{
    public class UpgradeItemUI : BuyItemUI
    {
    
        public TextMeshProUGUI hitAfter;
        public TextMeshProUGUI dmgAfter;
        public TextMeshProUGUI critAfter;
        public TextMeshProUGUI weightAfter;
        public TextMeshProUGUI stoneCost;
        public Image cost2Icon;
        public Sprite stoneIcon;
        public Sprite dragonScaleIcon;


        // Start is called before the first frame update
        public void Show(EquipableItem equip,WeaponUpgradeMode upgradeMode, int upgradegoldCost, int upgradeStoneCost, int dragonScaleCost, bool affordable)
        {
            base.Show(equip, 0,affordable, true);
            cost.text = "" + upgradegoldCost;

            if (upgradeStoneCost != 0)
            {
                stoneCost.gameObject.SetActive(true);
                cost2Icon.gameObject.SetActive(true);
                stoneCost.text = "" + upgradeStoneCost;
                cost2Icon.sprite = stoneIcon;
           
                stoneCost.color = Player.Instance.Party.Storage.GetItemCount(GameBPData.Instance.GetSmithingStone().Name) >= upgradeStoneCost? textNormalColor:tooExpensiveTextColor;
          
            }
            else if (dragonScaleCost != 0)
            {
                stoneCost.gameObject.SetActive(true);
                cost2Icon.gameObject.SetActive(true);
                cost2Icon.sprite = dragonScaleIcon;
                stoneCost.text = "" + dragonScaleCost;
                stoneCost.color = Player.Instance.Party.Storage.GetItemCount(GameBPData.Instance.GetDragonScale().Name) >= dragonScaleCost? textNormalColor:tooExpensiveTextColor;
            }
            else
            {
                stoneCost.gameObject.SetActive(false);
                cost2Icon.gameObject.SetActive(false);
            }

            buyButton.interactable = affordable &&
                                     Player.Instance.Party.Storage.GetItemCount(GameBPData.Instance.GetDragonScale().Name) >=
                                     dragonScaleCost &&
                                     Player.Instance.Party.Storage.GetItemCount(GameBPData.Instance.GetSmithingStone().Name) >=
                                     upgradeStoneCost;
            buttonText.text = buyButton.interactable?"<bounce>Upgrade": "</>Upgrade";
            if (equip is Weapon weapon)
            {
                hitAfter.gameObject.SetActive(upgradeMode== WeaponUpgradeMode.Accuracy);
                critAfter.gameObject.SetActive(upgradeMode== WeaponUpgradeMode.Critical);
                dmgAfter.gameObject.SetActive(upgradeMode== WeaponUpgradeMode.Power);
                critAfter.text = "" +(weapon.GetCrit()+ weapon.GetUpgradeableCrit());
                hitAfter.text = "" + (weapon.GetHit()+weapon.GetUpgradeableHit());
                dmgAfter.text = "" + (weapon.GetDamage()+weapon.GetUpgradeableDmg());
                switch (upgradeMode)
                {
                    case WeaponUpgradeMode.Power:weightAfter.text = "" + (weapon.GetWeight() + weapon.GetUpgradeableWeightPower()); break;
                    case WeaponUpgradeMode.Accuracy:weightAfter.text = "" + (weapon.GetWeight() + weapon.GetUpgradeableWeightAccuracy()); break;
                    case WeaponUpgradeMode.Critical:weightAfter.text = "" + (weapon.GetWeight() + weapon.GetUpgradeableWeightCrit()); break;
                    case WeaponUpgradeMode.Special:weightAfter.text = "" + (weapon.GetWeight() + weapon.GetUpgradeableWeightSpecial()); break;
                }
                
            }
        
        }
    }
}