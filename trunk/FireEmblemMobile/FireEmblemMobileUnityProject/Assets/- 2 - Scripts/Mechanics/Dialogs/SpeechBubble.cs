using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Mechanics.Dialogs
{
    public class SpeechBubble : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private TextAnimation textAnimation;

        private void Awake()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            textAnimation = GetComponentInChildren<TextAnimation>();
        }

        public void Show(string text)
        {
            gameObject.SetActive(true);
            //GetComponent<TransitionScale>().TransitionIn();

            this.text.text = text;
            textAnimation.StartAnimation();
            StartCoroutine(Hide());
        }

        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(2);
            //GetComponent<TransitionScale>().TransitionOut();

            // this.gameObject.SetActive(false);
        }
    }
}