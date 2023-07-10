using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.GUI.Text
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BlinkTextScript : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private CanvasGroup alphaCanvas;
        [SerializeField] private float blinkTime = .5f;
        [SerializeField] private float pauseTime = .5f;

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
                LeanTween.alphaCanvas(alphaCanvas, 0, blinkTime);
                yield return new WaitForSeconds(pauseTime);
                LeanTween.alphaCanvas(alphaCanvas, 1, blinkTime);
                yield return new WaitForSeconds(pauseTime);
      
            }
        }
    }
}