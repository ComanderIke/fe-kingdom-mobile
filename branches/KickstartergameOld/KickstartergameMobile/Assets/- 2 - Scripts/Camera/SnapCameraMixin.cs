using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SnapCameraMixin : CameraMixin
{
    const float LERP_SPEED = 0.1f;

    private float lerpTime;
    
    private void Update()
    {
        lerpTime = Time.deltaTime / LERP_SPEED;

        Vector3 targetPosition = new Vector3(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y), Mathf.Round(transform.localPosition.z));
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, lerpTime);
    }
   
}

