using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BuyItemUI : MonoBehaviour
{
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI hitCurrent;
    [SerializeField] TextMeshProUGUI dmgCurrent;
    [SerializeField] TextMeshProUGUI critCurrent;
    [SerializeField] TextMeshProUGUI weightCurrent;
    // public TextMeshProUGUI weightCurrent;
    // public TextMeshProUGUI weightAfter;
    [SerializeField] TextMeshProUGUI effectCurrent;
    [SerializeField] protected TextMeshProUGUI cost;
    [SerializeField] protected Button buyButton;
    [SerializeField] protected Button sellButton;
    [SerializeField] Image buttonBg;
    [SerializeField] private GameObject weaponSection;
    [SerializeField] private GameObject relicSection;
    [SerializeField]  TextMeshProUGUI relicEffectCurrent;
    [SerializeField] protected TextMeshProUGUI buttonText;

    [SerializeField] protected Color textNormalColor;
    [SerializeField] protected Color tooExpensiveTextColor;

  
    [SerializeField] protected float tooExpensiveAlpha;
    // Start is called before the first frame update
    public void Show(Item item, bool affordable, bool buying)
    {
        gameObject.SetActive(true);
        Icon.sprite = item.Sprite;
        cost.text = "" + item.cost;
        description.text = "" + item.Description;
        nameText.text = "" + item.Name;
        buttonText.text = affordable?"BUY": "Underfunded";
        effectCurrent.text = "";
        weaponSection.gameObject.SetActive(false);
        relicSection.gameObject.SetActive(false);
        buyButton.interactable = affordable;
        sellButton.gameObject.SetActive(!buying);
        buyButton.gameObject.SetActive(buying);
        if (affordable)
        {
            //buttonBg.color = buyColor;
            var colors = buyButton.colors;
            buyButton.colors = colors;
            cost.color = textNormalColor;
        }
        else
        {
            cost.color = tooExpensiveTextColor;
        }
        if (!buying)
        {
            cost.color = textNormalColor;
            
        }
        if (item is Weapon weapon)
        {
            weaponSection.gameObject.SetActive(true);
            //weightCurrent.text= ""+weapon.GetWeight();
            critCurrent.text = "" + weapon.GetCrit();
            hitCurrent.text = "" + weapon.GetHit();
            dmgCurrent.text = "" + weapon.GetDamage();

        }
     

        
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}