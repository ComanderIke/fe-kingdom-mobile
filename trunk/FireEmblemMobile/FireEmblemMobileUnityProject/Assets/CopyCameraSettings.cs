using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCameraSettings : MonoBehaviour
{
    public Camera copyCam;

    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera=GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        camera.orthographicSize = copyCam.orthographicSize;
    }
}
