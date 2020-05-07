
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DynamicAmbientLight : MonoBehaviour
{
    public Gradient gradient;
    public Light2D lightSource;
    public float dayDuration = 1;
    private float lightBaseIntensity;
    // Start is called before the first frame update
    void Start()
    {
        lightBaseIntensity = lightSource.intensity;
    }
    float time = 0;
    // Update is called once per frame
    void Update()
    {
        time+= Time.deltaTime/dayDuration;
        if (time > 1)
        {
            time = 0;
        }
        if(time<=0.5f)
            lightSource.intensity = Mathf.SmoothStep(0.4f, lightBaseIntensity, time*2);
        else
        {
            lightSource.intensity = Mathf.SmoothStep(lightBaseIntensity, 0.4f, (time-0.5f) * 2);
        }
        lightSource.color = gradient.Evaluate(time);
       
    }
}
