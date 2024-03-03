using System.Collections.Generic;
using Game.GameActors.Items.Gems;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Player;
using Game.GUI.EncounterUI.Merchant;
using Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Screens
{
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
        [SerializeField] private Button insertButton;
        [SerializeField] private Button removeButton;
        //[SerializeField] private SmithingSlot relic1;
   
    
        private List<SelectableItemController> instantiatedItems;
        private StockedItem selected;
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
            slotGameObject.SetActive(true);
            // relic1.Show(Player.Instance.Party.ActiveUnit.EquippedRelic, currentRelic == Player.Instance.Party.ActiveUnit.EquippedRelic);
      
            gemParent.DeleteAllChildren();
            var equippedGem = currentRelic.GetGem();
            if (selected != null)
            {
                if(selected.item is Gem gem)
                    if (gem.IsInserted())
                    {
                        if(!Equals(currentRelic.GetGem(), gem))
                            selected = null;
                    }
            }
            if (equippedGem != null)
            {
                slotGemImage.sprite = equippedGem.Sprite;
                slotGemImage.enabled = true;
                var gemGO=Instantiate(gemPrefab, gemParent);
                var selectableItemUI =gemGO.GetComponent<SelectableItemController>();
                selectableItemUI.SetValues(new StockedItem(equippedGem, 1));
                selectableItemUI.onClicked += ItemClicked;
                if(selected!=null && selected.item==equippedGem)
                    selectableItemUI.Select();
                instantiatedItems.Add(selectableItemUI);
            }
            else
            {
                slotGemImage.sprite = null;
                slotGemImage.enabled = false;
            }
      
        
            foreach (var gem in Player.Instance.Party.Storage.GetAllGems())
            {
                var gemGO=Instantiate(gemPrefab, gemParent);
                var selectableItemUI =gemGO.GetComponent<SelectableItemController>();
                selectableItemUI.SetValues(gem);
                if(selected==gem)
                    selectableItemUI.Select();
                selectableItemUI.onClicked += ItemClicked;
                instantiatedItems.Add(selectableItemUI);
            }

            if (selected == null && instantiatedItems.Count != 0)
            {
                selected = instantiatedItems[0].item;
                instantiatedItems[0].Select();
            }

            if (selected != null && Equals(selected.item, currentRelic.GetGem()))
            {
                var colors = insertButton.colors;
                removeButton.gameObject.SetActive(true);
                insertButton.gameObject.SetActive(false);
          
                insertButton.colors = colors;
            
            }
            else
            {
                insertButton.interactable = Player.Instance.Party.Storage.HasGems();
                var colors = insertButton.colors;
                removeButton.gameObject.SetActive(false);
                insertButton.gameObject.SetActive(true);
            
                insertButton.colors = colors;
   
            }
        }

        public void InsertClicked()
        {
            if (selected == null)
                return;
            if (Equals(selected.item, currentRelic.GetGem()))
            {
                Debug.Log("Remove Gem!");
                var gem=currentRelic.RemoveGem();
                Player.Instance.Party.AddItem(gem);
            
            }
            else
            {
                Debug.Log("Insert Gem!");
                Debug.Log(currentRelic.Name);
                currentRelic.InsertGem((Gem)selected.item);
                Player.Instance.Party.RemoveItem(selected.item);
            }
            UpdateUI();
        }
        void ItemClicked(SelectableItemController item)
        {
            selected = item.item;
            foreach (var itemUI in instantiatedItems)
            {
                if (itemUI.item == selected)
                {
                    itemUI.Select();
                }
                else
                    itemUI.Deselect();
            }

            UpdateUI();
        }
    }
}