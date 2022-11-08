using System;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class TextFieldPopUp : MonoBehaviour
    {
        [SerializeField] private TMP_InputField textInputField;
        public event Action<string> onSubmittedText;
        public void Show(string file)
        {
            gameObject.SetActive(true);
            textInputField.text = file;
        }

        public void Enter()
        {
            if (textInputField.text != "")
            {
                onSubmittedText?.Invoke(textInputField.text );
                Hide();
            }
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}