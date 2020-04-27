using UnityEngine;

namespace Assets.Utility
{
    public class AnimationTimer : MonoBehaviour
    {
        public float NormalizedTime;
        public float AnimationTimeBlinkSpritesDuration;
        public float AnimationTimeBlinkSprites;

        void Update()
        {
            NormalizedTime += Time.deltaTime;
            NormalizedTime = NormalizedTime % 1.0f;
            AnimationTimeBlinkSprites += Time.deltaTime/ AnimationTimeBlinkSpritesDuration;
            AnimationTimeBlinkSprites = AnimationTimeBlinkSprites % 1.0f;
        }
    }
}
