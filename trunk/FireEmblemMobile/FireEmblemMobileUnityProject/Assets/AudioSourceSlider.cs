using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class AudioSourceSlider : MonoBehaviour
    {
       public Slider slider;
       public AudioSource audioS;
       
       void Update()
       {
           if(audioS!=null)
            slider.maxValue = audioS.clip.length;
       }
       
       public void OnSliderValueChanged()
       {
           if(audioS!=null)
            audioS.time = slider.value;
       }
       
       private void FixedUpdate()
       {
           if(audioS!=null)
            slider.value = audioS.time;
       }
    }
}
