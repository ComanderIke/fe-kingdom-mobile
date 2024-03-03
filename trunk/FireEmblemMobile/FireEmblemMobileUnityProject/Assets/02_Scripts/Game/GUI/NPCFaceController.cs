using TMPro;
using UnityEngine;

namespace Game.GUI
{
    public class NPCFaceController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI speechText;

        public void Show(string text)
        {
            speechText.SetText(text);
        }
    }
}
