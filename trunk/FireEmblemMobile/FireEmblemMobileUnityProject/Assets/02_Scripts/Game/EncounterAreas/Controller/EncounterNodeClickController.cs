using System;
using System.Collections;
using System.Collections.Generic;
using Game.Manager;
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
           ServiceProvider.Instance.StartChildCoroutine(DelayBy1Frame(()=>  FindObjectOfType<AreaGameManager>().NodeClicked(encounterNode)));
          
        }
    }

    IEnumerator DelayBy1Frame(Action action)
    {
        yield return null;
        action?.Invoke();
    }
}
