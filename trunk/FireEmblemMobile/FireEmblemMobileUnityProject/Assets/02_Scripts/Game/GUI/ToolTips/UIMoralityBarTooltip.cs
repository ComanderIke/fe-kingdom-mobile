using TMPro;
using UnityEngine;

namespace Game.GUI.ToolTips
{
    public class UIMoralityBarTooltip:MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI text;
        public void Show(float morality)
        {
            text.text = "" + morality;
        }
    }
}