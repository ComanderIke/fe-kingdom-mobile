using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEngine;
using UnityEngine;

namespace Game.Audio
{
    public class AudioSystem : MonoBehaviour, IEngineSystem
    {
        public static AudioSystem Instance;

        [HideInInspector] private List<MusicData> currentlyPlayedMusic;

        public MusicData[] Music;

        public SoundData[] Sounds;
        [SerializeField] float battleVolumeFadeTime=1.0f;

        private bool initialized = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Init();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            

           
        }

        public void Init()
        {
            if (!initialized)
            {
                foreach (var s in Sounds)
                {
                    s.Source = gameObject.AddComponent<AudioSource>();
                    s.Source.clip = s.Clip;
                    s.Source.volume = s.Volume;
                    s.Source.pitch = s.Pitch;
                    s.Source.loop = s.Loop;
                    s.Source.playOnAwake = false;
                }

                foreach (var m in Music)
                {
                    m.Source = gameObject.AddComponent<AudioSource>();
                    m.Source.clip = m.Clip;
                    m.Source.volume = m.Volume;
                    m.Source.pitch = m.Pitch;
                    m.Source.loop = m.Loop;
                    m.Source.playOnAwake = false;
                }

                initialized = true;
            }

            currentlyPlayedMusic = new List<MusicData>();
            //PlayMusic("PlayerTheme", 0, 1);
        }
        private void OnDisable()
        {
            Deactivate();
        }
        public void Deactivate()
        {
            
        }

        public void Activate()
        {
            
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

        // private void Update()
        // {
        //     foreach (var cpm in currentlyPlayedMusic)
        //     {
        //         if (cpm.playAfter != null)
        //         {
        //             cpm.Source.pl
        //         }
        //     }
        // }

        private void PlayMusic(string name, float delay = 0, float fadeDuration = -1.0f)
        {
            var m = Array.Find(Music, music => music.Name == name);
            if (m == null)
            {
                Debug.LogWarning("Sound no found!");

                return;
            }

            if (currentlyPlayedMusic.Contains(m))
                return;

            currentlyPlayedMusic.Add(m);
            if (fadeDuration != -1.0f)
                FadeIn(name, fadeDuration, delay);
            else
                m.Source.Play();
            if(m.playAfter!=null)
                m.playAfter.Source.PlayDelayed(m.Clip.length+delay);
        }

        public void SwitchIntoBattle()
        {
            MyDebug.LogMusic("Switch  into Battle");
            foreach (var playedMusic in currentlyPlayedMusic)
            {
                if (playedMusic is BattleMusicData battleMusicData)
                {
                    var tmpTime = battleMusicData.Source.time;
                    battleMusicData.Source.clip = battleMusicData.ClipInBattle;
                    battleMusicData.Source.time = tmpTime;
                    LeanTween.value(gameObject, battleMusicData.Source.volume, battleMusicData.VolumeBattle, battleVolumeFadeTime)
                        .setEaseInOutQuad().setOnUpdate((val) =>
                        {
                            battleMusicData.Source.volume = val;
                        });
                    //battleMusicData.Source.volume = battleMusicData.VolumeBattle;
                    battleMusicData.Source.Play();
                }
            }
        }
        public void SwitchOutofBattle()
        {
            MyDebug.LogMusic("Switch out of Battle");
            foreach (var playedMusic in currentlyPlayedMusic)
            {
                if (playedMusic is BattleMusicData battleMusicData)
                {
                    var tmpTime = battleMusicData.Source.time;
                    battleMusicData.Source.clip = battleMusicData.Clip;
                    battleMusicData.Source.time = tmpTime;
                    //battleMusicData.Source.volume = battleMusicData.Volume;
                    battleMusicData.Source.Play();
                    LeanTween.value(gameObject, battleMusicData.Source.volume, battleMusicData.Volume, battleVolumeFadeTime)
                        .setEaseInOutQuad().setOnUpdate((val) =>
                        {
                            battleMusicData.Source.volume = val;
                        });
                }
            }
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
            Debug.Log("Change Music: "+newMusicName+" "+currentMusicName);
            FadeOut(currentMusicName, fadeOutDuration, freshRestart);
            PlayMusic(newMusicName, fadeInDelay, fadeInDuration);
        }
        public void ChangeAllMusic(string newMusicName,  bool freshRestart = false,
            float fadeOutDuration = 1.0f, float fadeInDelay = 0.5f, float fadeInDuration = 1.0f)
        {
            MyDebug.LogMusic("Change All Music: "+newMusicName);
            foreach (var musicName in GetCurrentlyPlayedMusicTracks())
            {
                if (musicName == newMusicName)
                    continue;
                FadeOut(musicName, fadeOutDuration, freshRestart);
            }
           
            PlayMusic(newMusicName, fadeInDelay, fadeInDuration);
        }

        public List<string> GetCurrentlyPlayedMusicTracks()
        {
            if (currentlyPlayedMusic == null || currentlyPlayedMusic.Count == 0)
                return new List<string>();
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