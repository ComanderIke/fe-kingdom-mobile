using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class StatContainerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private TMP_ColorGradient neutralColorGradient;
        [SerializeField] private TMP_ColorGradient negativeColorGradient;
        [SerializeField] private TMP_ColorGradient positiveColorGradient;

        public enum ColorState
        {
            Increasing,
            Decreasing,
            Same
        }
        public void SetValue(string label, int value, bool additive, ColorState colorState)
        {
            this.label.text = label;
            if (additive)
            {
                this.value.text = value >= 0 ? "+" + value : ""+value;
               
            }
            else
            {
                this.value.text = "" + value;
              
            }

            switch (colorState)
            {
                case ColorState.Decreasing: this.value.colorGradientPreset = negativeColorGradient;break;
                case ColorState.Increasing: this.value.colorGradientPreset = positiveColorGradient;break;
                case ColorState.Same: this.value.colorGradientPreset = neutralColorGradient;break;
            }
        }
    }
}
