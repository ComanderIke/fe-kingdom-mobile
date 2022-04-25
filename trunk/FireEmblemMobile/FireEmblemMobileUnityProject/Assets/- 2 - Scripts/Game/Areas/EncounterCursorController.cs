using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterCursorController : MonoBehaviour
{
    public float rotationSpeed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.forward, Time.deltaTime*rotationSpeed);
    }

    public void SetPosition(Vector3 transformPosition)
    {
        transform.position = transformPosition;
    }
}
