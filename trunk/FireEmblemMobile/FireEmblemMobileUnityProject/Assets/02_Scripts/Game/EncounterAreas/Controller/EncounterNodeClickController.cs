using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EncounterNodeClickController : MonoBehaviour
{
    public EncounterNode encounterNode;
    
    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    private void OnMouseDown()
    {
        if (!IsPointerOverUIObject())
        {
           
            FindObjectOfType<AreaGameManager>().NodeClicked(encounterNode);
        }
    }
}
