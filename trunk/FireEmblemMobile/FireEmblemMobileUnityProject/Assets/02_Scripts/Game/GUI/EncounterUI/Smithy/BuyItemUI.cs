using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BuyItemUI : MonoBehaviour
{
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI hitCurrent;
    [SerializeField] TextMeshProUGUI dmgCurrent;
    [SerializeField] TextMeshProUGUI critCurrent;
    // public TextMeshProUGUI weightCurrent;
    // public TextMeshProUGUI weightAfter;
    [SerializeField] TextMeshProUGUI effectCurrent;
    [SerializeField] protected TextMeshProUGUI cost;
    [SerializeField] Button button;
    [SerializeField] Image buttonBg;
    [SerializeField] private GameObject weaponSection;
    [SerializeField] private GameObject relicSection;
    [SerializeField]  TextMeshProUGUI relicEffectCurrent;
    [SerializeField] protected TextMeshProUGUI buttonText;
    [SerializeField]  Color sellColor;
    [SerializeField] private Color buyColor;

    // Start is called before the first frame update
    public void Show(Item item, bool affordable, bool buying)
    {
        gameObject.SetActive(true);
        Icon.sprite = item.Sprite;
        cost.text = "" + item.cost;
        description.text = "" + item.Description;
        name.text = "" + item.Name;
        buttonText.text = "BUY";
        effectCurrent.text = "";
        weaponSection.gameObject.SetActive(false);
        relicSection.gameObject.SetActive(false);
        buttonBg.color = buyColor;
        if (affordable)
        {
            buttonBg.color = buyColor;
        }
        else
        {
            buttonBg.color = sellColor;
        }
        if (!buying)
        {
            buttonText.text = "SELL";
            buttonBg.color = sellColor;
        }
        if (item is Weapon weapon)
        {
            weaponSection.gameObject.SetActive(true);
            //weightCurrent.text= ""+weapon.GetWeight();
            critCurrent.text = "" + weapon.GetCrit();
            hitCurrent.text = "" + weapon.GetHit();
            dmgCurrent.text = "" + weapon.GetDamage();

        }
        if (item is Relic relic)
        {
            relicSection.gameObject.SetActive(true);
            relicEffectCurrent.text = relic.GetAttributeDescription();
        }

        button.interactable = affordable;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}