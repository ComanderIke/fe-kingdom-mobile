using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabGroup : MonoBehaviour
{
    public List<UITabButton> tabButtons;
    public Sprite idle;
    public Sprite selected;
    public UITabButton selectedTab;
    public List<GameObject> objectsToSwap;
    public void Subscribe(UITabButton tabButton)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<UITabButton>();
        }
        tabButtons.Add(tabButton);
    }

    private void ResetTabs()
    {
        foreach (var tabButton in tabButtons)
        {
            if (selectedTab != null && tabButton == selectedTab) 
                continue;
            tabButton.backGround.sprite = idle;
        }
    }

    public void OnTabSelected(UITabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.backGround.sprite = selected;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }
}
