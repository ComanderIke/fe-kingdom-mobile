using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.GUI.Text
{
    public class BlinkTextScript : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private CanvasGroup alphaCanvas;

        // Use this for initialization
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            alphaCanvas = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            StartCoroutine(Blink());
        }

        private void OnDisable()
        {
            StopCoroutine("Blink");
        }

        private IEnumerator Blink()
        {
            while (true)
            {
                LeanTween.alphaCanvas(alphaCanvas, 0, 0.5f);
                yield return new WaitForSeconds(0.5f);
                LeanTween.alphaCanvas(alphaCanvas, 1, 0.5f);
                yield return new WaitForSeconds(0.5f);
      
            }
        }
    }
}