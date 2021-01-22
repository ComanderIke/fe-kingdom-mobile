using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEventMaskScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var cam = GetComponent<Camera>();
        cam.eventMask = LayerMask.GetMask();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
