using UnityEngine;

namespace Assets.Utility
{
    public class AnimationTimer : MonoBehaviour
    {
        public float NormalizedTime;

        void Update()
        {
            NormalizedTime += Time.deltaTime;
            NormalizedTime = NormalizedTime % 1.0f;
        }
    }
}
