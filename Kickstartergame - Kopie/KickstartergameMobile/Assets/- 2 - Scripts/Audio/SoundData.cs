
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
[CreateAssetMenu(menuName = "GameData/Audio/Sound", fileName = "Sound")]
public class SoundData :ScriptableObject
{

    public new string name;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public bool fadingIn;
    [HideInInspector]
    public bool fadingOut;
}

