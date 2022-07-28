using TMPro;
using UnityEngine;

namespace Game.GUI
{
    public class UIStatBar : IStatBar
    {
        [SerializeField]
        private UIFilledBarController barController;
        [SerializeField]
        private TextMeshProUGUI valueText;
        public override void SetValue(int value, int maxValue)
        {
            barController.SetFillAmount(value*1.0f/maxValue);
            valueText.SetText(value+"/"+maxValue);
        }
    }
}