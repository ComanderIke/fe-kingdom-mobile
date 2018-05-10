using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public Music[] music;
    [HideInInspector]
    private List<Music> currentlyPlayedMusic;

    public static AudioManager instance;
	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
		foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Music m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
        currentlyPlayedMusic = new List<Music>();
        PlayMusic("PlayerPhase",0,1);

    }
    public void PlaySound(string name, float delay = 0, float fadeDuration = -1.0f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
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
        Music m = Array.Find(music, music => music.name == name);
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
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
    public void StopMusic(string name)
    {
        Music m = Array.Find(music, music => music.name == name);
        currentlyPlayedMusic.Remove(m);
        m.source.Stop();
    }

    public void ChangeMusic(string newMusicName, string currentMusicName, bool freshRestart = false,float  fadeOutDuration=1.0f, float fadeInDelay = 0.5f, float fadeInDuration=1.0f)
    {
        FadeOut(currentMusicName, fadeOutDuration, freshRestart);
        PlayMusic(newMusicName, fadeInDelay, fadeInDuration);
    }
    // Update is called once per frame
    void Update () {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ChangeMusic(sounds[1].name, sounds[0].name);
        //}
	}

    public List<string> GetCurrentlyPlayedMusicTracks()
    {
        List<string> musicNames = new List<string>();
        foreach(Music m in currentlyPlayedMusic)
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
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
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
            Music m = Array.Find(music, music => music.name == name);

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
        Sound s = Array.Find(sounds, sound => sound.name == name);
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
            Music m = Array.Find(music, music => music.name == name);
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
