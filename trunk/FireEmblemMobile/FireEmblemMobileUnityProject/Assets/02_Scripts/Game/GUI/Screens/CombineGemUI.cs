using System.Collections.Generic;
using System.Linq;
using __2___Scripts.Game.Utility;
using Game.GameActors.Items.Gems;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CombineGemUI : MonoBehaviour
{
    [SerializeField] private GameObject gemPrefab;
    [SerializeField] private Transform gemParent;
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI gemEffect;
    [SerializeField] TextMeshProUGUI requiredText;
    [SerializeField] Image smallIcon;
    [SerializeField] private Button combineButton;

    [SerializeField] private int combineCount = 3;
private List<SelectableItemController> instantiatedItems;
    private StockedItem selected;
    
    public void Show()
    {
        gameObject.SetActive(true);
        UpdateUI();
    }

    void UpdateUI()
    {
        instantiatedItems = new List<SelectableItemController>();
        combineButton.interactable = false;
     
        gemParent.DeleteAllChildren();
       Debug.Log("GemCount: "+Player.Instance.Party.Convoy.GetAllGems().Count());
        foreach (var gem in Player.Instance.Party.Convoy.GetAllGems())
        {
            var gemGO=Instantiate(gemPrefab, gemParent);
            var selectableItemUI =gemGO.GetComponent<SelectableItemController>();
            selectableItemUI.SetValues(gem);
            if(selected==gem)
                selectableItemUI.Select();
            selectableItemUI.onClicked += ItemClicked;
            instantiatedItems.Add(selectableItemUI);
        }
        if (selected == null)
        {
            if(instantiatedItems.Count!=0)
                selected = instantiatedItems[0].item;
        }
        if (selected != null)
        {
            var gem = (Gem)selected.item;
            Icon.sprite = selected.item.Sprite;
            nameText.text = selected.item.Name;
            description.text = selected.item.Description;
            int count = Player.Instance.Party.Convoy.GetGemCount((Gem)selected.item);
            smallIcon.sprite = selected.item.Sprite;
            if (gem.HasUpgrade())
            {
                requiredText.text = "Required " + count + "/" + combineCount;
            }
            else
            {
                requiredText.text = "Maxed Out";
            }

            if (count >= combineCount&& gem.HasUpgrade() )
            {
                combineButton.interactable = true;
            }
            else
            {
                combineButton.interactable = false;
            }
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void CombineClicked()
    {
        for (int i = 0; i < combineCount; i++)
        {
            Player.Instance.Party.RemoveItem(selected.item);
        }
        Player.Instance.Party.AddItem(((Gem)selected.item).GetUpgradedGem());

        UpdateUI();
    }
    void ItemClicked(SelectableItemController item)
    {
        selected = item.item;
        foreach (var itemUI in instantiatedItems)
        {
            if(itemUI==item)
                itemUI.Select();
            else
                itemUI.Deselect();
        }
        UpdateUI();
    }
}