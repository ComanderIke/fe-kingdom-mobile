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

    [SerializeField] private Animator animator;

    private float musicVolume = 0;
    private static readonly int Show1 = Animator.StringToHash("Show");

    public override void Show()
    {
        animator.SetBool(Show1, true);
        base.Show();
    }
    public override void Hide()
    {
        StartCoroutine(HideCoroutine());
        
    }

    IEnumerator HideCoroutine()
    {
        animator.SetBool(Show1, false);
        yield return new WaitForSeconds(1.0f);
        base.Hide();
        
    }
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