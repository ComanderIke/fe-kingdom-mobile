using TMPro;
using UnityEngine;

namespace Game.GUI
{
    public enum AttributeBonusState
    {
        Increasing,
        Decreasing,
        Same
    }
    public class StatContainerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private TMP_ColorGradient neutralColorGradient;
        [SerializeField] private TMP_ColorGradient negativeColorGradient;
        [SerializeField] private TMP_ColorGradient positiveColorGradient;

        
        public void SetValue(string label, float value, bool additive, AttributeBonusState attributeBonusState)
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

            switch (attributeBonusState)
            {
                case AttributeBonusState.Decreasing: this.value.colorGradientPreset = negativeColorGradient;break;
                case AttributeBonusState.Increasing: this.value.colorGradientPreset = positiveColorGradient;break;
                case AttributeBonusState.Same: this.value.colorGradientPreset = neutralColorGradient;break;
            }
        }
    }
}
