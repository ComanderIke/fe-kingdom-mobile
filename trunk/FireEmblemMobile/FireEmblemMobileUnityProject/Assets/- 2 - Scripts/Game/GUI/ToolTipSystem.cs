using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem instance;
    public ItemToolTip ItemToolTip;

    public void Awake()
    {
        instance = this;
    }

    public static void Show(Vector3 position, string header, string description, Sprite icon)
    {
        instance.ItemToolTip.SetValues(header,description,icon, position);
        
        instance.ItemToolTip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        instance.ItemToolTip.gameObject.SetActive(false);
    }
}
