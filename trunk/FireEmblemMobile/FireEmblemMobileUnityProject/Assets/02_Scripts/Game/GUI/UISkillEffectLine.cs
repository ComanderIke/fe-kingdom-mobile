using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class UISkillEffectLine : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private TextMeshProUGUI upgradedValue;

        public void SetValues(string label, int value, int upg)
        {
            this.label.text = label;
            this.value.text = ""+value;
            if(upg!=value)
                this.upgradedValue.text = "-> "+upg;
            else
            {
                this.upgradedValue.text = "";
            }
        }
    }
}
