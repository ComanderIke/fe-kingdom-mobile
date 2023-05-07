﻿using TMPro;
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
        [SerializeField] private GameObject deleteButton;
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