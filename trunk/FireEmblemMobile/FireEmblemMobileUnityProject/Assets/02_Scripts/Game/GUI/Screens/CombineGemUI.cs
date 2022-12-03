using System.Collections.Generic;
using System.Linq;
using __2___Scripts.Game.Utility;
using Game.GameActors.Items.Gems;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
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
    private List<SelectableItemController> instantiatedItems;
    private SelectableItemController selectedGem;
    
    public void Show()
    {
        gameObject.SetActive(true);
        UpdateUI();
    }

    void UpdateUI()
    {
        instantiatedItems = new List<SelectableItemController>();
        
     
        gemParent.DeleteAllChildren();
       Debug.Log("GemCount: "+Player.Instance.Party.Convoy.GetAllGems().Count());
        foreach (var gem in Player.Instance.Party.Convoy.GetAllGems())
        {
            var gemGO=Instantiate(gemPrefab, gemParent);
            var selectableItemUI =gemGO.GetComponent<SelectableItemController>();
            selectableItemUI.SetValues(gem);
            selectableItemUI.onClicked += ItemClicked;
            instantiatedItems.Add(selectableItemUI);
        }
        if (selectedGem == null)
        {
            if(instantiatedItems.Count!=0)
                selectedGem = instantiatedItems[0];
        }

        if (selectedGem != null)
        {
            Icon.sprite = selectedGem.item.item.Sprite;
            nameText.text = selectedGem.item.item.Name;
            description.text = selectedGem.item.item.Description;
            int count = Player.Instance.Party.Convoy.GetGemCount((Gem)selectedGem.item.item);
            smallIcon.sprite = selectedGem.item.item.Sprite;
            requiredText.text = "Required " + count + "/4";
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void CombineClicked()
    {
        UpdateUI();
    }
    void ItemClicked(SelectableItemController item)
    {
        selectedGem = item;
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