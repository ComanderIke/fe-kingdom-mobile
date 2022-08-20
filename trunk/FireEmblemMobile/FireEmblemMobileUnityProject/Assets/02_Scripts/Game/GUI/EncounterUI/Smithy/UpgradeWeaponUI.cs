using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWeaponUI : MonoBehaviour
{
    public Image Icon;

    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public TextMeshProUGUI hitCurrent;
    public TextMeshProUGUI hitAfter;
    public TextMeshProUGUI dmgCurrent;
    public TextMeshProUGUI dmgAfter;
    public TextMeshProUGUI critCurrent;

    public TextMeshProUGUI critAfter;

    // public TextMeshProUGUI weightCurrent;
    // public TextMeshProUGUI weightAfter;
    public TextMeshProUGUI effectCurrent;
    public TextMeshProUGUI effectAfter;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI stoneCost;
    public Button upgradeButton;

    private EquipableItem equipable;
    [SerializeField] private GameObject weaponSection;
    [SerializeField] private GameObject relicSection;
    [SerializeField]  TextMeshProUGUI relicEffectCurrent;
    [SerializeField]  TextMeshProUGUI relicEffectAfter;

    // Start is called before the first frame update
    public void Show(EquipableItem equip, bool affordable)
    {
        this.equipable = equip;
        Icon.sprite = equipable.Sprite;
        cost.text = "" + equipable.GetUpgradeCost();
        stoneCost.text = "" + equipable.GetUpgradeSmithingStoneCost();
        description.text = "" + equipable.Description;
        name.text = "" + equipable.name;
        effectAfter.text = "-";
        effectCurrent.text = "-";
        weaponSection.gameObject.SetActive(false);
        relicSection.gameObject.SetActive(false);
        if (equipable is Weapon weapon)
        {
            weaponSection.gameObject.SetActive(true);
            //weightCurrent.text= ""+weapon.GetWeight();
            critCurrent.text = "" + weapon.GetCrit();
            hitCurrent.text = "" + weapon.GetHit();
            dmgCurrent.text = "" + weapon.GetDamage();
            //weightAfter.text =""+ weapon.GetUpgradeableWeight();
            critAfter.text = "" + weapon.GetUpgradeableCrit();
            hitAfter.text = "" + weapon.GetUpgradeableHit();
            dmgAfter.text = "" + weapon.GetUpgradeableDmg();
        }

        if (equipable is Relic relic)
        {
            relicSection.gameObject.SetActive(true);
            relicEffectCurrent.text = relic.GetAttributeDescription();
            relicEffectAfter.text = relic.GetUpgradeAttributeDescription();
            
        }

        upgradeButton.interactable = affordable;
    }
}