using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class Music
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;

    [Range(.1f, 3f)]
    public float pitch = 1;
    public bool loop;
    public bool freshRestart;
    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public bool fadingIn;
    [HideInInspector]
    public bool fadingOut;

}
