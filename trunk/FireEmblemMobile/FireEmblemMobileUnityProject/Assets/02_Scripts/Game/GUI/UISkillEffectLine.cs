using TMPro;
using UnityEngine;

namespace Game.GUI
{
    public class UISkillEffectLine : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject upgradeArrow;
        [SerializeField] private GameObject downgradeArrow;
        [SerializeField] private TextMeshProUGUI value;

        [SerializeField] private TMP_ColorGradient defaultColor;

        [SerializeField] private TMP_ColorGradient upgColor; 
        [SerializeField] private TMP_ColorGradient downGradeColor; 
      //  [SerializeField] private TextMeshProUGUI upgradedValue;

        public void SetValues(string label, string value, bool upg, bool downgrade=false)
        {
            this.label.text = label;
            this.value.text = ""+value;
            upgradeArrow.gameObject.SetActive(upg&&!downgrade);
            downgradeArrow.gameObject.SetActive(downgrade);
            this.value.colorGradientPreset = downgrade?downGradeColor:upg?upgColor:defaultColor;

        }
    }
}
