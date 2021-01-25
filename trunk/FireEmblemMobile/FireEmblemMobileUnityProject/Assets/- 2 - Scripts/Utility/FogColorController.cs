using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FogColorController : MonoBehaviour
{
    public Color color;
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
