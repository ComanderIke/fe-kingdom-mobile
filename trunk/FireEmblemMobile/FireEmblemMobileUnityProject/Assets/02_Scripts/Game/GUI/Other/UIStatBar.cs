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

        public int currentValue;
        public override void SetValue(int value, int maxValue)
        {
            this.currentValue = value;
            barController.SetFillAmount(currentValue*1.0f/maxValue);
            valueText.SetText(currentValue+"/"+maxValue);
        }
    }
}