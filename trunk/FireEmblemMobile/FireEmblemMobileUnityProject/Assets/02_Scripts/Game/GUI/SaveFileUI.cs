using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class SaveFileUI:MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image image;
        [SerializeField] private Color startColor;
        [SerializeField] private Color loadedColor;
        public void UpdateText(string name)
        {
            nameText.SetText(name);
        }

        public void SetLoaded(bool loaded)
        {
            image.color = loaded ? loadedColor : startColor;
        }

        public void SetInteractable(bool b)
        {
            button.interactable = b;
        }

       
    }
}