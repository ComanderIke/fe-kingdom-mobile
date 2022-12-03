using System;
using Game.GameActors.Items;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public event Action<Item> onClicked;
    private Item item;
    [SerializeField] private Image itemIcon;

    public void Clicked()
    {
        onClicked?.Invoke(item);
    }

    public void SetItem(Item item)
    {
        this.item = item;
        itemIcon.sprite = item.Sprite;
    }
}