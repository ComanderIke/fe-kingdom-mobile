using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Dialog.Animation
{
    public class TextAnimation : MonoBehaviour
    {
        private int letterIndex = 0;
        private string text;
        public TextMeshProUGUI TextMesh;
        private const float SPEED = 0.03f;
        private IEnumerator enumerator;

        private void Awake()
        {
            TextMesh = GetComponent<TextMeshProUGUI>();
        }

        public void StartAnimation()
        {
            letterIndex = 0;
            text = TextMesh.text;
            TextMesh.text = "";

            enumerator = DisplayTimer();
            StartCoroutine(enumerator);
        }

        private IEnumerator DisplayTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(SPEED);
                if (letterIndex > text.Length)
                {
                    continue;
                }

                TextMesh.text = text.Substring(0, letterIndex);
                letterIndex++;
                GetComponent<AudioSource>().Play();
            }
        }

        public void StopAnimation()
        {
            StopCoroutine(enumerator);
        }
    }
}