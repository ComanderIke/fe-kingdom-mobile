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
    [SerializeField] TextMeshProUGUI weightCurrent;
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
    [SerializeField] private Color sellColorPressed;
    [SerializeField] private Color buyColor;
    [SerializeField] private Color buyColorPressed;

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
        button.interactable = affordable;
        if (affordable)
        {
            //buttonBg.color = buyColor;
            var colors = button.colors;
            colors.normalColor = buyColor;
            colors.highlightedColor = buyColor;
            colors.pressedColor = buyColorPressed;
            colors.selectedColor = buyColor;
            button.colors = colors;
        }
        else
        {
            var colors = button.colors;
            colors.normalColor = sellColor;
            colors.pressedColor = sellColorPressed;
            colors.selectedColor = sellColor;
            button.colors = colors;
        }
        if (!buying)
        {
            
            buttonText.text = "SELL";
            var colors = button.colors;
            colors.normalColor = sellColor;
            colors.pressedColor = sellColorPressed;
            colors.selectedColor = sellColor;
            button.colors = colors;
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