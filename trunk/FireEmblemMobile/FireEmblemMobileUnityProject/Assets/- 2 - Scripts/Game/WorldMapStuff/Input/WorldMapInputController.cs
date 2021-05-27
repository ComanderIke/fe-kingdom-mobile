using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapInputController : MonoBehaviour
{
    public IWorldMapInputReceiver InputReceiver { get; set; }

    // Update is called once per frame
    public void OnMouseDown()
    {
        if(InputReceiver!=null)
            InputReceiver.WorldClicked();
    }
}
