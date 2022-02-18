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

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            Debug.Log("EncounterClicked!" + gameObject.name);
            Debug.Log("EncounterClicked: " + encounterNode);
            FindObjectOfType<AreaGameManager>().NodeClicked(encounterNode);
        }


    }
}
