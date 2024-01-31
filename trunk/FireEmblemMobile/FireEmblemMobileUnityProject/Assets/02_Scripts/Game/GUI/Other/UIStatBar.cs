using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

namespace Game.GUI
{
    public class UIStatBar : IStatBar
    {
        [SerializeField]
        private TextMeshProUGUI valueText;

        [SerializeField] private MMProgressBar progressBar;

        public int currentValue;
        private bool init = false;
        private void Start()
        {
            init = false;
        }

        public override void SetValue(int value, int maxValue, bool animated)
        {
            if (maxValue == 0)
                return;
      
            this.currentValue = value;
            
           // barController.SetFillAmount(currentValue*1.0f/maxValue);
            valueText.SetText(currentValue+"/"+maxValue);
            MyDebug.LogTest("HP Bar Set Value: "+value+" "+maxValue+" "+animated);
            if (progressBar != null)
            {
                if (!animated || !init)
                {
                    progressBar.SetBar01((currentValue * 1.0f )/ maxValue);
                    init = true;
                }
                else
                {
                    progressBar.UpdateBar01((currentValue * 1.0f) / maxValue);
                   
                }
            }
        }
    }
}