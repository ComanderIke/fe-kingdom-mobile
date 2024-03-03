using UnityEngine;

namespace Game.Utility
{
    public class AnimationTimer : MonoBehaviour
    {
        public static AnimationTimer Instance;
        public float NormalizedTime;
        public float AnimationTimeBlinkSpritesDuration;
        public float AnimationTimeBlinkSprites;

        private void Awake()
        {
            Instance = this;
        }
        void Update()
        {
            NormalizedTime += Time.deltaTime;
            NormalizedTime = NormalizedTime % 1.0f;
            AnimationTimeBlinkSprites += Time.deltaTime/ AnimationTimeBlinkSpritesDuration;
            AnimationTimeBlinkSprites = AnimationTimeBlinkSprites % 1.0f;
        }
    }
}
