using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Items.Gems;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsertGemUI : MonoBehaviour
{
    [SerializeField] private GameObject gemPrefab;
    [SerializeField] private Transform gemParent;
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI gemEffect;
    [SerializeField] Image slotGemImage;
    [SerializeField] GameObject slotGameObject;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button insertButton;
    [SerializeField] private Color insertColor;
    [SerializeField] private Color removeColor;
    private List<SelectableItemController> instantiatedItems;
    private SelectableItemController selected;
    private Relic currentRelic;
    
    public void Show(Relic relic)
    {
        gameObject.SetActive(true);
        this.currentRelic = relic;
        UpdateUI();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    void UpdateUI()
    {
        instantiatedItems = new List<SelectableItemController>();
        this.Icon.sprite = currentRelic.Sprite;
        nameText.text = currentRelic.Name;
        description.text = currentRelic.Description;
        slotGameObject.SetActive(currentRelic.slotCount > 0);
        var equippedGem = currentRelic.GetGem(0);
        if (equippedGem != null)
        {
            slotGemImage.sprite = equippedGem.Sprite;
        }
        else
        {
            slotGemImage.sprite = null;
        }
        gemParent.DeleteAllChildren();
        foreach (var gem in Player.Instance.Party.Convoy.GetAllGems())
        {
            var gemGO=Instantiate(gemPrefab, gemParent);
            var selectableItemUI =gemGO.GetComponent<SelectableItemController>();
            selectableItemUI.SetValues(gem);
            selectableItemUI.onClicked += ItemClicked;
            instantiatedItems.Add(selectableItemUI);
        }

        if (selected == null&&instantiatedItems.Count!=0)
            selected = instantiatedItems[0];
        if (selected != null && selected.item.item == currentRelic.GetGem(0))
        {
            var colors = insertButton.colors;
            colors.normalColor = removeColor;
            colors.highlightedColor = removeColor;
            colors.selectedColor = removeColor;
            colors.pressedColor = removeColor;
            insertButton.colors = colors;
            buttonText.text = "Remove";
        }
        else
        {
            var colors = insertButton.colors;
            colors.normalColor = insertColor;
            colors.highlightedColor = insertColor;
            colors.selectedColor = insertColor;
            colors.pressedColor = insertColor;
            insertButton.colors = colors;
            buttonText.text = "Insert";
        }
    }

    public void InsertClicked()
    {
        if (selected == null)
            return;
        if (selected.item.item == currentRelic.GetGem(0))
        {
            var gem=currentRelic.RemoveGem(0);
            Player.Instance.Party.Convoy.AddItem(gem);
            
        }
        else
        {
            currentRelic.InsertGem((Gem)selected.item.item,0);
        }
    }
    void ItemClicked(SelectableItemController item)
    {
        selected = item;
        foreach (var itemUI in instantiatedItems)
        {
            if(itemUI==selected)
                itemUI.Select();
            else
                itemUI.Deselect();
        }

        UpdateUI();
    }
}