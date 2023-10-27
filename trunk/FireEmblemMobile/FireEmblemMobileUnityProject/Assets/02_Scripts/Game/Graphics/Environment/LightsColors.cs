using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightsColors : MonoBehaviour, DayNightInterface
{
    public Gradient Gradient;

    public UnityEngine.Rendering.Universal.Light2D []lights;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParameter(float time)
    {
        foreach (var light in lights)
        {
            light.color = Gradient.Evaluate(time);
        }
    }
}
