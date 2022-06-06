using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EncounterClickController : MonoBehaviour
{
    // Start is called before the first frame update
    public EncounterNode encounterNode;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
