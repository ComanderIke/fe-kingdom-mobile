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

    // public TextMeshProUGUI weightCurrent;
    // public TextMeshProUGUI weightAfter;
    [SerializeField] protected TextMeshProUGUI cost;
    [SerializeField] protected Button buyButton;
    [SerializeField] protected Button sellButton;
    [SerializeField] private GameObject weaponSection;
    [SerializeField] protected TextMeshProUGUI buttonText;

    [SerializeField] protected Color textNormalColor;
    [SerializeField] protected Color tooExpensiveTextColor;

    
    // Start is called before the first frame update
    public void Show(Item item, bool affordable, bool buying)
    {
        gameObject.SetActive(true);
        Icon.sprite = item.Sprite;
        cost.text = "" + item.cost;
        description.text = "" + item.Description;
        nameText.text = "" + item.Name;
        buttonText.text = affordable?"<bounce>BUY": "Underfunded";
        weaponSection.gameObject.SetActive(false);
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