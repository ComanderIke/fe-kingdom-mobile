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
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float newGameTextAlpha;
        [SerializeField] private float loadedTextAlpha;
        [SerializeField] private GameObject deleteButton;
        public void UpdateText(string name)
        {
            nameText.SetText(name);
        }

        public void SetLoaded(bool loaded)
        {
            canvasGroup.alpha = loaded ? loadedTextAlpha : newGameTextAlpha;
        }

        public void SetInteractable(bool b)
        {
            button.interactable = b;
        }


        public void ShowDeleteButton()
        {
            deleteButton.SetActive(true);
        }
        public void HideDeleteButton()
        {
            deleteButton.SetActive(false);
        }
    }
}