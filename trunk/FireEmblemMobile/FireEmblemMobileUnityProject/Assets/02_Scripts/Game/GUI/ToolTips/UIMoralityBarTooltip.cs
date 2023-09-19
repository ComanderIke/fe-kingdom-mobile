using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMoralityBarTooltip:MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;
    public void Show(float morality)
    {
        text.text = "" + morality;
    }
}