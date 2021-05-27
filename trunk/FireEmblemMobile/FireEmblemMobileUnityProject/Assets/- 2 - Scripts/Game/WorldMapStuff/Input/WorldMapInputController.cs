using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMapInputController : MonoBehaviour
{
    public IWorldMapInputReceiver InputReceiver { get; set; }

    // Update is called once per frame
    public void OnMouseDown()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
            if(InputReceiver!=null)
                InputReceiver.WorldClicked();
    }
}
