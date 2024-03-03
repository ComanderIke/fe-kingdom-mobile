using System;
using UnityEngine;

namespace Game.Audio
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Audio/Music", fileName = "Music")]
    public class MusicData : ScriptableObject
    {
        public AudioClip Clip;

        [HideInInspector] public bool FadingIn;

        [HideInInspector] public bool FadingOut;

        public bool FreshRestart;
        public bool Loop;
        public string Name;

        [Range(.1f, 3f)] public float Pitch = 1;

        [HideInInspector] public AudioSource Source;

        [Range(0f, 1f)] public float Volume = 1;
        public MusicData playAfter;
    }
}