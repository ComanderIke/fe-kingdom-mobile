using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GameEngine;
using UnityEngine;

namespace Assets.Audio
{
    public class AudioSystem : MonoBehaviour, IEngineSystem
    {
        public static AudioSystem Instance;

        [HideInInspector] private List<MusicData> currentlyPlayedMusic;

        public MusicData[] Music;

        public SoundData[] Sounds;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            foreach (var s in Sounds)
            {
                s.Source = gameObject.AddComponent<AudioSource>();
                s.Source.clip = s.Clip;
                s.Source.volume = s.Volume;
                s.Source.pitch = s.Pitch;
                s.Source.loop = s.Loop;
            }

            foreach (var m in Music)
            {
                m.Source = gameObject.AddComponent<AudioSource>();
                m.Source.clip = m.Clip;
                m.Source.volume = m.Volume;
                m.Source.pitch = m.Pitch;
                m.Source.loop = m.Loop;
            }

            currentlyPlayedMusic = new List<MusicData>();
            PlayMusic("PlayerTheme", 0, 1);
        }

        public void PlaySound(string name, float delay = 0, float fadeDuration = -1.0f)
        {
            var s = Array.Find(Sounds, sound => sound.Name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound no found!");

                return;
            }

            if (fadeDuration != -1.0f)
                FadeIn(name, fadeDuration, delay);
            else
                s.Source.Play();
        }

        public void PlayMusic(string name, float delay = 0, float fadeDuration = -1.0f)
        {
            var m = Array.Find(Music, music => music.Name == name);
            if (m == null)
            {
                Debug.LogWarning("Sound no found!");

                return;
            }

            currentlyPlayedMusic.Add(m);
            if (fadeDuration != -1.0f)
                FadeIn(name, fadeDuration, delay);
            else
                m.Source.Play();
        }

        public void StopSound(string name)
        {
            var s = Array.Find(Sounds, sound => sound.Name == name);
            s.Source.Stop();
        }

        public void StopMusic(string name)
        {
            var m = Array.Find(Music, music => music.Name == name);
            currentlyPlayedMusic.Remove(m);
            m.Source.Stop();
        }

        public void ChangeMusic(string newMusicName, string currentMusicName, bool freshRestart = false,
            float fadeOutDuration = 1.0f, float fadeInDelay = 0.5f, float fadeInDuration = 1.0f)
        {
            FadeOut(currentMusicName, fadeOutDuration, freshRestart);
            PlayMusic(newMusicName, fadeInDelay, fadeInDuration);
        }

        public List<string> GetCurrentlyPlayedMusicTracks()
        {
            return currentlyPlayedMusic.Select(m => m.Name).ToList();
        }

        public void FadeIn(string name, float duration, float delay = 0)
        {
            StartCoroutine(FadeInCoroutine(name, duration, delay));
        }

        public void FadeOut(string name, float duration, bool freshRestart = false)
        {
            StartCoroutine(FadeOutCoroutine(name, duration, freshRestart));
        }
        private IEnumerator FadeInCoroutine(string name, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
            var s = Array.Find(Sounds, sound => sound.Name == name);

            if (s != null)
            {
                s.FadingIn = true;
                s.FadingOut = false;
                s.Source.Play();
                float maxVolume = s.Volume;
                float volume = 0;
                s.Source.volume = volume;
                while (s.Source.volume < maxVolume && s.FadingIn)
                {
                    volume += 0.1f / duration;
                    s.Source.volume = volume;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                var m = Array.Find(Music, music => music.Name == name);
                m.FadingIn = true;
                m.FadingOut = false;
                m.Source.Play();
                float maxVolume = m.Volume;
                float volume = 0;
                m.Source.volume = volume;
                while (m.Source.volume < maxVolume && m.FadingIn)
                {
                    volume += 0.1f / duration;
                    m.Source.volume = volume;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        private IEnumerator FadeOutCoroutine(string name, float duration, bool freshRestart)
        {
            var s = Array.Find(Sounds, sound => sound.Name == name);
            if (s != null)
            {
                s.FadingIn = false;
                s.FadingOut = true;
                float volume = s.Source.volume;
                s.Volume = volume;
                while (s.Source.volume > 0 && s.FadingOut)
                {
                    volume -= 0.1f / duration;
                    s.Source.volume = volume;
                    yield return new WaitForSeconds(0.1f);
                }

                s.Source.Stop();
            }
            else
            {
                var m = Array.Find(Music, music => music.Name == name);
                m.FadingIn = false;
                m.FadingOut = true;
                float volume = m.Source.volume;
                m.Volume = volume;
                while (m.Source.volume > 0 && m.FadingOut)
                {
                    volume -= 0.1f / duration;
                    m.Source.volume = volume;
                    yield return new WaitForSeconds(0.1f);
                }

                if (freshRestart)
                    m.Source.Stop();
                else
                    m.Source.Pause();
                currentlyPlayedMusic.Remove(m);
            }
        }
    }
}