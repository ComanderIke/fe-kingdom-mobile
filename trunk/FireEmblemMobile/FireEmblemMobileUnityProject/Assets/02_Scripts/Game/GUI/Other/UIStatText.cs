using TMPro;
using UnityEngine;

namespace Game.GUI.Other
{
    public class UIStatText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private TMP_ColorGradient neutralColorGradient;
        [SerializeField] private TMP_ColorGradient negativeColorGradient;
        [SerializeField] private TMP_ColorGradient positiveColorGradient;

        public void SetValue(int value, AttributeBonusState attributeBonusState)
        {

            this.value.text = "" + value;
              
            

            switch (attributeBonusState)
            {
                case AttributeBonusState.Decreasing: this.value.colorGradientPreset = negativeColorGradient;break;
                case AttributeBonusState.Increasing: this.value.colorGradientPreset = positiveColorGradient;break;
                case AttributeBonusState.Same: this.value.colorGradientPreset = neutralColorGradient;break;
            }
        }
    }
}
