using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeTrigger : MonoBehaviour
{
    public CameraShake cameraShake;

    public float magnitude = 0.4f;

    public float duration = 1.9f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(cameraShake.Shake(duration, magnitude));
    }
}
