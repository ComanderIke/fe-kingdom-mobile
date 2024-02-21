using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UICombatItemSlot : MonoBehaviour
    {
       
        [SerializeField] private Image emptyIcon;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI stockText;
        [SerializeField] Image backGround;
        [SerializeField] Color bGNormalColor;
        [SerializeField] Color bGSelectedColor;
        [SerializeField] private GameObject toolTipButtonArea;
        public event Action<UICombatItemSlot> OnClicked;
        private StockedCombatItem item;
        private bool selected = false;
        [SerializeField]
        public int slotNumber=1;

        public void Clicked()
        {
            Debug.Log("CLICKED COMBAT ITEM SLOT");
            if(item!=null)
                OnClicked?.Invoke(this);
        }
        public void Show(StockedCombatItem unitCombatItem1, bool selected=false)
        {
            this.selected = selected;
            item = unitCombatItem1;
            stockText.enabled = true;
            iconImage.enabled = true;
            emptyIcon.enabled = false;
           
            UpdateUI();
        }

        void UpdateUI()
        {
            if (item == null)
            {
                toolTipButtonArea.gameObject.SetActive(false);
                Hide();
                
                return;
            }
            toolTipButtonArea.gameObject.SetActive(true);
            this.stockText.text = item.stock + "";
            this.iconImage.sprite = item.item.GetIcon();
            if (selected)
            {
                backGround.color = bGSelectedColor;
            }
            else
            {
                backGround.color = bGNormalColor;
            }
        }

        public void Hide()
        {
            emptyIcon.enabled = true;
            stockText.enabled = false;
            iconImage.enabled = false;
        }

        public void TooltipClicked()
        {
            ToolTipSystem.Show(new StockedItem((Item)item.item, item.stock), transform.position);
        }
        public StockedCombatItem GetCombatItem()
        {
            return item;
        }
        public void Select()
        {
            selected = true;
            UpdateUI();
        }

        public void Deselect()
        {
            selected = false;
            UpdateUI();
        }
    }
}
