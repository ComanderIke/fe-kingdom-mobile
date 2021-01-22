using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraEventMaskScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().eventMask = LayerMask.GetMask("UI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
