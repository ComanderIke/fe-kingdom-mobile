using System;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIChronikEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private Image image;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float deselectAlpha;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Sprite selectedBackground;
        [SerializeField] private Sprite normalBackground;
        public event Action<UIChronikEntry> OnClicked;
        public IChronikEntry entry;
        public void SetEntry(IChronikEntry entry)
        {
            this.entry = entry;
            name.text = entry.Name;
            image.sprite = entry.FaceSprite;
        }

        public void Clicked()
        {
            OnClicked?.Invoke(this);
        }
        public void Deselect()
        {
            canvasGroup.alpha=deselectAlpha;
            backgroundImage.sprite = normalBackground;
        }

        public void Select()
        {
            canvasGroup.alpha = 1;
            backgroundImage.sprite = selectedBackground;
        }
    }
}