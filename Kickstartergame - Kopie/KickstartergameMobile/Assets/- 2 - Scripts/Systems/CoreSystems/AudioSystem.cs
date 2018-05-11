using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Engine;

public class AudioSystem : MonoBehaviour , EngineSystem{

    public SoundData[] sounds;
    public MusicData[] music;
    [HideInInspector]
    private List<MusicData> currentlyPlayedMusic;

    public static bool singleton = false;
	// Use this for initialization
	void Awake () {
        if (singleton == false)
            singleton = true;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
		foreach(SoundData s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (MusicData m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
        currentlyPlayedMusic = new List<MusicData>();
        PlayMusic("PlayerTheme",0,1);

    }

    public void PlaySound(string name, float delay = 0, float fadeDuration = -1.0f)
    {
        SoundData s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound no found!");

            return;
        }
        
        if(fadeDuration!=-1.0f)
            FadeIn(name, fadeDuration, delay);
        else
            s.source.Play();
    }
    public void PlayMusic(string name, float delay = 0, float fadeDuration = -1.0f)
    {
        MusicData m = Array.Find(music, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("Sound no found!");

            return;
        }
        currentlyPlayedMusic.Add(m);
        if (fadeDuration != -1.0f)
            FadeIn(name, fadeDuration, delay);
        else
        {
            m.source.Play();
        }
    }

    public void StopSound(string name)
    {
        SoundData s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
    public void StopMusic(string name)
    {
        MusicData m = Array.Find(music, music => music.name == name);
        currentlyPlayedMusic.Remove(m);
        m.source.Stop();
    }

    public void ChangeMusic(string newMusicName, string currentMusicName, bool freshRestart = false,float  fadeOutDuration=1.0f, float fadeInDelay = 0.5f, float fadeInDuration=1.0f)
    {
        FadeOut(currentMusicName, fadeOutDuration, freshRestart);
        PlayMusic(newMusicName, fadeInDelay, fadeInDuration);
    }

    public List<string> GetCurrentlyPlayedMusicTracks()
    {
        List<string> musicNames = new List<string>();
        foreach(MusicData m in currentlyPlayedMusic)
        {
            musicNames.Add(m.name);
        }
        return musicNames;
    }
    public void FadeIn(string name, float duration, float delay=0)
    {
        StartCoroutine(FadeInCoroutine(name, duration, delay));
    }
    public void FadeOut(string name, float duration, bool freshRestart=false)
    {
        StartCoroutine(FadeOutCoroutine(name, duration, freshRestart));
    }
    IEnumerator FadeInCoroutine(string name, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundData s = Array.Find(sounds, sound => sound.name == name);
        
        if (s != null)
        {
            s.fadingIn = true;
            s.fadingOut = false;
            s.source.Play();
            float maxVolume = s.volume;
            float volume = 0;
            s.source.volume = volume;
            while (s.source.volume < maxVolume && s.fadingIn)
            {
                volume += 0.1f / duration;
                s.source.volume = volume;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            MusicData m = Array.Find(music, music => music.name == name);

            m.fadingIn = true;
            m.fadingOut = false;
            m.source.Play();
            float maxVolume = m.volume;
            float volume = 0;
            m.source.volume = volume;
            while (m.source.volume < maxVolume && m.fadingIn)
            {
                volume += 0.1f / duration;
                m.source.volume = volume;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    IEnumerator FadeOutCoroutine(string name, float duration, bool freshRestart)
    {
        SoundData s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.fadingIn = false;
            s.fadingOut = true;
            float volume = s.source.volume;
            s.volume = volume;
            while (s.source.volume > 0 && s.fadingOut)
            {

                volume -= 0.1f / duration;
                s.source.volume = volume;
                yield return new WaitForSeconds(0.1f);
            }
            s.source.Stop();
        }
        else
        {
            MusicData m = Array.Find(music, music => music.name == name);
            m.fadingIn = false;
            m.fadingOut = true;
            float volume = m.source.volume;
            m.volume = volume;
            while (m.source.volume > 0 && m.fadingOut)
            {

                volume -= 0.1f / duration;
                m.source.volume = volume;
                yield return new WaitForSeconds(0.1f);
            }
            if (freshRestart)
                m.source.Stop();
            else
                m.source.Pause();
            currentlyPlayedMusic.Remove(m);
        }
    }
}
