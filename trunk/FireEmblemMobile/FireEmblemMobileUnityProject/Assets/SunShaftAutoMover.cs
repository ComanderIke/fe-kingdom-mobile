using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SunShaftAutoMover : MonoBehaviour
{
    public float moveSpeed;

    public Vector2 maxMovement;

    public Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
     //   startPos = transform.position;
    }

    // Update is called once per frame
    private float time = 0;
    void Update()
    {
        time += Time.deltaTime*moveSpeed;
        transform.position = Vector2.Lerp(startPos, maxMovement, time);
        if (time >= 1)
        {
            time = 0;
        }
    }
}
