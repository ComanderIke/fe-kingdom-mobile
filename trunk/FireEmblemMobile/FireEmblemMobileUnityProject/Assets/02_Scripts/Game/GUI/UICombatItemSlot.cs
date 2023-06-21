using System;
using System.Collections;
using System.Collections.Generic;
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
        public event Action<UICombatItemSlot> OnClicked;
        private StockedItem item;

        public void Clicked()
        {
            Debug.Log("CLICKED COMBAT ITEM SLOT");
            if(item!=null)
                OnClicked?.Invoke(this);
        }
        public void Show(StockedItem unitCombatItem1)
        {
            item = unitCombatItem1;
            if (unitCombatItem1 == null)
            {
                Hide();
                return;
            }

            stockText.enabled = true;
            iconImage.enabled = true;
            emptyIcon.enabled = false;
            this.stockText.text = unitCombatItem1.stock + "";
            this.iconImage.sprite = unitCombatItem1.item.Sprite;
        }

        public void Hide()
        {
            emptyIcon.enabled = true;
            stockText.enabled = false;
            iconImage.enabled = false;
        }

        public StockedItem GetCombatItem()
        {
            return item;
        }
    }
}
