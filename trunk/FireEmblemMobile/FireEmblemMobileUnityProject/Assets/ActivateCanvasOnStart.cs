using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCanvasOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled=true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
