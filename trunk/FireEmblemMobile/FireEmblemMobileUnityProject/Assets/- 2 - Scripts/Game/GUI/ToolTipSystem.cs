using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem instance;
    public ItemToolTip ItemToolTip;

    public void Awake()
    {
        instance = this;
    }

    public static void Show(Item item, Vector3 position, string header, string description, Sprite icon)
    {
        instance.ItemToolTip.SetValues(item, header,description,icon, position);
        
        instance.ItemToolTip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        instance.ItemToolTip.gameObject.SetActive(false);
    }
}
