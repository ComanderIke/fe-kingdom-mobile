using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCircleUI : MonoBehaviour
{
    private float rotateAmount = 360f/24f;

    [SerializeField] private float angleOffset = 90f;
    private float currentZRot = 0;
    // Start is called before the first frame update
    private Quaternion startRot;
    private Quaternion targetRot;
    private float time = 0;
    private bool rotate = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            time += Time.deltaTime;
            if (time <= 1)
            {
                transform.rotation = Quaternion.Lerp(startRot, targetRot, time);
            }
            else
            {
                rotate = false;
            }
        }
    }

    public void Rotate(float hour)
    {
        startRot = Quaternion.Euler(0,0,currentZRot+angleOffset);
        currentZRot = rotateAmount*hour;
        targetRot = Quaternion.Euler(0,0,currentZRot+angleOffset);
        time = 0;
        rotate = true;
    }
}
