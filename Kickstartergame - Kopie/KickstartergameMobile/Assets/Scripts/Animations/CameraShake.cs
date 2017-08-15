using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    // if null.

    // How long the object should shake for.
    public static float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public static float shakeAmount = 0.7f;
    public static float decreaseFactor = 1.0f;
    static Camera shakeCamera;
    static Vector3 originalPos;

    void Awake()
    {
      
    }

    void OnEnable()
    {
       

    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            shakeCamera.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else if(shakeDuration !=0)
        {
            shakeDuration = 0f;
            shakeCamera.transform.localPosition = originalPos;
        }

    }
    public static void Shake(float duration, float strength)
    {
        GameObject go = GameObject.Find("Main Camera");
        if (go != null)
            shakeCamera = go.GetComponent<Camera>();
        if (shakeCamera == null)
            shakeCamera = GameObject.Find("CombatCamera").GetComponent<Camera>();
        Debug.Log("Enable" + shakeCamera.name);
        originalPos = shakeCamera.transform.localPosition;
        originalPos = shakeCamera.transform.localPosition;
        shakeDuration = duration;
        shakeAmount = strength;
    }
}
