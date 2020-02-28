using UnityEngine;

public class AnimationTimer : MonoBehaviour
{
    public float normalizedTime;

    void Update()
    {
        normalizedTime += Time.deltaTime;
        normalizedTime = normalizedTime % 1.0f;
    }
}
