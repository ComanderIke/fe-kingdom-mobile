using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemUI : MonoBehaviour
{
    public Image Icon;

    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;
    public Button buyButton;
    private ShopItem item;
    // Start is called before the first frame update
    public void Show(ShopItem item, bool affordable)
    {
        gameObject.SetActive(true);
        this.item = item;
        Icon.sprite = item.sprite;
        cost.text = ""+item.cost;
        description.text = "" + item.description;
        name.text = "" + item.name;
        buyButton.interactable = affordable;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
