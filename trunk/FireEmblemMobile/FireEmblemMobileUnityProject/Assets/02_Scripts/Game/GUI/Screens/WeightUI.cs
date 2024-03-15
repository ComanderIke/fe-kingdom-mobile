using TMPro;
using UnityEngine;

namespace Game.GUI.Screens
{
    public class WeightUI : MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI weightValue;
        [SerializeField] private TMP_ColorGradient tooHeavyColor;
        [SerializeField] private TMP_ColorGradient normalColor;

        public void Show(int weight, bool heavy)
        {
            gameObject.SetActive(true);
            weightValue.text = ""+weight;
            weightValue.colorGradientPreset = heavy ? tooHeavyColor : normalColor;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}