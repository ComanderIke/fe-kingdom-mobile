using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterCursorController : MonoBehaviour
{
    public float scaleTime = 0.8f;
    public float scaleFactor = 1.08f;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(gameObject, new Vector3(scaleFactor, scaleFactor,scaleFactor), scaleTime).setEaseInOutQuad().setLoopType(LeanTweenType.pingPong);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(Vector3.forward, Time.deltaTime*rotationSpeed);
       
    }

    public void SetPosition(Vector3 transformPosition)
    {
        transform.position = transformPosition;
    }
}
