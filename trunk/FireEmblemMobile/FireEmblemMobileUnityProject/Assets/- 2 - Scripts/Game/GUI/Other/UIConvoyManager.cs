using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GUI;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIConvoyManager : MonoBehaviour
{
    public Transform itemListParent;

    public GameObject convoyItemPrefab;
    // Start is called before the first frame update
    public void Show(Party party)
    {
        foreach (var item in party.convoy)
        {
            var go= Instantiate(convoyItemPrefab, itemListParent);
            go.GetComponent<UIDragable>().SetItem(item);
            
        }
    }
}
