using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPartyMemberDropArea : MonoBehaviour, IDropHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Debug.Log("ON DROP AREA");
            var partyUI=eventData.pointerDrag.gameObject.GetComponent<PartyMemberUIController>();
            if (partyUI != null)
            {
                partyUI.UpdateParent(partyUI.saveParent);
        
            }
            //Invoke("UpdatePartyOrder",0.02f);
           
        }
    }
    void UpdatePartyOrder()
    {
        var UIPartyOverViewController = FindObjectOfType<UIPartyOverViewController>();
        if (UIPartyOverViewController != null)
        {
            UIPartyOverViewController.UpdatePartyOrder();
        }
    }
}
