using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharScreenTabController : MonoBehaviour
{
    public Image[] tabs;
    public Color tabActiveColor;
    public Color tabInActiveColor;
    public void TabClicked(int index)
    {
        for(int i=0; i< tabs.Length; i++)
        {
            if (i == index)
            {
                tabs[i].color = tabActiveColor;
            }
            else
            {
                tabs[i].color = tabInActiveColor;
            }
        }
    }
}
