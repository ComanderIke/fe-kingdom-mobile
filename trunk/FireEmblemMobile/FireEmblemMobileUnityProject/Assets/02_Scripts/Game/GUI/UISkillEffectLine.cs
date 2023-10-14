using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class UISkillEffectLine : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject upgradeArrow;
        [SerializeField] private TextMeshProUGUI value;

        [SerializeField] private TMP_ColorGradient defaultColor;

        [SerializeField] private TMP_ColorGradient upgColor; 
      //  [SerializeField] private TextMeshProUGUI upgradedValue;

        public void SetValues(string label, string value, bool upg)
        {
            this.label.text = label;
            this.value.text = ""+value;
            upgradeArrow.gameObject.SetActive(upg);
            this.value.colorGradientPreset = upg?upgColor:defaultColor;

        }
    }
}
