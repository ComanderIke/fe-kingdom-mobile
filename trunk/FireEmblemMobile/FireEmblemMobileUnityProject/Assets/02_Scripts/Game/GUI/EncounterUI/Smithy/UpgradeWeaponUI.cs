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
    private Weapon weapon;
    // Start is called before the first frame update
    public void Show(Weapon weapon, bool affordable)
    {
        this.weapon = weapon;
        Icon.sprite = weapon.Sprite;
        cost.text = ""+weapon.GetUpgradeCost();
        stoneCost.text = "" + weapon.GetUpgradeSmithingStoneCost();
        description.text = "" + weapon.Description;
        name.text = "" + weapon.name;
        effectAfter.text = "-";
        effectCurrent.text = "-";
        //weightCurrent.text= ""+weapon.GetWeight();
        critCurrent.text = ""+weapon.GetCrit();
        hitCurrent.text = ""+weapon.GetHit();
        dmgCurrent.text = ""+weapon.GetDamage();
        //weightAfter.text =""+ weapon.GetUpgradeableWeight();
        critAfter.text = ""+weapon.GetUpgradeableCrit();
        hitAfter.text =""+ weapon.GetUpgradeableHit();
        dmgAfter.text =""+ weapon.GetUpgradeableDmg();
        
        upgradeButton.interactable = affordable;
    }
    
}
