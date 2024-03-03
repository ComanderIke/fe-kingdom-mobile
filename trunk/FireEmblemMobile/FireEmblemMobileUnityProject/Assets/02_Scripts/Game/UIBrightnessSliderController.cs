using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Game
{
    public class UIBrightnessSliderController : MonoBehaviour
    {
        [SerializeField]private List<VolumeProfile> volumes;
        List<ColorAdjustments> liftGammaGains;
        [SerializeField] private Slider slider;
        void Start()
        {
            liftGammaGains = new List<ColorAdjustments>();
            bool first = true;
            foreach (var volume in volumes)
            {
                ColorAdjustments color;
                if (volume.TryGet(out color))
                {
                    liftGammaGains.Add(color);
                    if (first)
                    {
                        first = false;
                        slider.value = color.postExposure.value;
                    }
                    else
                    {
                        color.postExposure.value = slider.value;
                    }
                   
                }
            }
           
            
           
        }

        void Update()
        {
        
        }

        public void OnSliderChanged(float value)
        {
            foreach (var gain in liftGammaGains)
            {
               // gain.lift.overrideState = true;
               // gain.gain.overrideState = true;
                gain.postExposure.overrideState = true;
                //gain.lift.value = new Vector4(1, 1, 1, value);
                //gain.gain.value = new Vector4(1, 1, 1, value);
                gain.postExposure.value = value;
            }
        }
    }
}
