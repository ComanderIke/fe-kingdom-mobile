using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPose = transform.localPosition;
        float startmagnitude = magnitude;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            magnitude = Mathf.SmoothStep(startmagnitude ,0,elapsed / duration);
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = new Vector3(x,y, originalPose.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPose;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
