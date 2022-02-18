using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GUI;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIConvoyController:MonoBehaviour
{

    private bool enabled;
    public GameObject convoyItemPrefab;
    public List<ConvoyDropArea> DropAreas;

    private List<GameObject> instantiatedItems;
    private List<StockedItem> convoy;
    private bool init = false;
    public Canvas characterCanvas;
    public Vector3 leftPosition;
    public Vector3 rightPosition;
    public void Toogle()
    {
       
        enabled = !enabled;
        
        GetComponent<Canvas>().enabled = enabled;
        if(enabled)
            UpdateValues();
    }
    public void UpdateValues()
    {
        if (characterCanvas.enabled)
        {
            GetComponent<RectTransform>().anchoredPosition = rightPosition;
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = leftPosition;
        }
        this.convoy = Player.Instance.Party.Convoy;
        if (!init)
        {
            init = true;
            Player.Instance.Party.convoyUpdated += UpdateConvoy;
            instantiatedItems = new List<GameObject>();
        }

        if (instantiatedItems.Count != 0)
        {
            for (int i = instantiatedItems.Count - 1; i >= 0; i--)
            {
                Destroy(instantiatedItems[i]);
            }
            instantiatedItems.Clear();
        }
        
        for(int i=0; i < convoy.Count; i++)
        {
            
            var go=Instantiate(convoyItemPrefab, DropAreas[i].transform);
            go.GetComponent<UIConvoyItemController>().SetValues(convoy[i]);
            go.GetComponent<UIDragable>().SetItem(convoy[i].item);
            go.GetComponent<UIDragable>().SetCanvas(GetComponent<Canvas>());
            instantiatedItems.Add(go);
        }

    }

    private void OnDestroy()
    {
        Player.Instance.Party.convoyUpdated -= UpdateConvoy;
    }

    private void UpdateConvoy()
    {
        UpdateValues();
    }
    public void Hide()
    {
        enabled = false;
        GetComponent<Canvas>().enabled = enabled;
    }
}