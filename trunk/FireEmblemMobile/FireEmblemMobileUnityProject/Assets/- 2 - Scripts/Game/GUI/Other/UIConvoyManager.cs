using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using UnityEngine;

public class UIConvoyManager : MonoBehaviour
{
    public Transform itemListParent;

    public GameObject convoyItemPrefab;
    // Start is called before the first frame update
    private void OnEnable()
    {
        foreach (var item in Player.Instance.convoy)
        {
            var go= Instantiate(convoyItemPrefab, itemListParent);
            go.GetComponentInChildren<ItemDisplay>().item = item;
            go.GetComponentInChildren<ItemDisplay>().UpdateSprite();
        }
    }
}
