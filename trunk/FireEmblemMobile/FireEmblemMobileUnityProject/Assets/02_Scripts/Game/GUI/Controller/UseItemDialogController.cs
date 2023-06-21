using System;
using Game.GameActors.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseItemDialogController : OKCancelDialogController
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemIcon;
    public void Show(Item item, Action action)
    {
        itemIcon.sprite = item.Sprite;
        itemName.text = item.Name;
        base.Show(item.Description, action);

     
    }
}