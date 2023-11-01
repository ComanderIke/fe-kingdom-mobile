using System.Collections;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsController : UIMenu
{
    // Start is called before the first frame update

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private AudioMixer masterMixer;

    [SerializeField] private Animator animator;
    [SerializeField] private Toggle debugModeToggle;
    [SerializeField] private Toggle tutorialToogle;
    private float musicVolume = 0;
    private static readonly int Show1 = Animator.StringToHash("Show");

    public override void Show()
    {
        animator.SetBool(Show1, true);
        debugModeToggle.SetIsOnWithoutNotify(GameConfig.Instance.ConfigProfile.debugModeEnabled);
        tutorialToogle.SetIsOnWithoutNotify(GameConfig.Instance.ConfigProfile.tutorialEnabled);
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

    public void OnTutorialToggled(bool value)
    {
        GameConfig.Instance.ConfigProfile.tutorialEnabled = value;
    }
    public void OnFixedGrowthsToggled(bool value)
    {
        GameConfig.Instance.ConfigProfile.fixedGrowths = value;
    }

    public void OnDebugModeToggled(bool value)
    {
        GameConfig.Instance.ConfigProfile.debugModeEnabled = value;
    }
   
}