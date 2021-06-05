using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsController : UIMenu
{
    // Start is called before the first frame update
   
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private AudioMixer masterMixer;

    private float musicVolume = 0;
    void OnEnable()
    {
        masterMixer.GetFloat("MusicVolume", out musicVolume);
        //Debug.Log("TODO Convert Normalized Value to dB");
        musicVolumeSlider.value = musicVolume;
           
    }

    public void OnMusicVolumeSliderChanged()
    {
        musicVolume = musicVolumeSlider.value;
        masterMixer.SetFloat("MusicVolume", musicVolume);
        //Debug.Log("TODO Convert Normalized Value to dB");
    }
   
}