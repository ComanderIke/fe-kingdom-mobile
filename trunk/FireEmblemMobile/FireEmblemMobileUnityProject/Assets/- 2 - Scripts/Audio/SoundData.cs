using System;
using UnityEngine;

namespace Assets.Audio
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Audio/Sound", fileName = "Sound")]
    public class SoundData : ScriptableObject
    {
        public AudioClip Clip;

        [HideInInspector] public bool FadingIn;

        [HideInInspector] public bool FadingOut;

        public bool Loop;

        public string Name;

        [Range(.1f, 3f)] public float Pitch;

        [HideInInspector] public AudioSource Source;

        [Range(0f, 1f)] public float Volume;
    }
}