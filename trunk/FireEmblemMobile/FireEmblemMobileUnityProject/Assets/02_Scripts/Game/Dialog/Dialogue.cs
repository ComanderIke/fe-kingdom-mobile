using System;
using System.Collections;
using Game.Dialog;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unused.Dialogs
{
	[System.Serializable]
	public class Dialogue : MonoBehaviour
	{
		[SerializeField] CanvasGroup speakerNameLeftCanvasGroup;
		[SerializeField] CanvasGroup speakerNameRightCanvasGroup;
		[SerializeField] CanvasGroup speakerImageLeftCanvasGroup;
		[SerializeField] CanvasGroup speakerImageRightCanvasGroup;
		public TextMeshProUGUI speakerNameLeft;
		public TextMeshProUGUI speakerNameRight;
		public Image speakerImageLeft;
		public Image speakerImageRight;
		public TextMeshProUGUI textDisplay;
		public CanvasGroup dialogCanvas;
		public AudioSource letterSource;
		public AudioSource nextLineSource;
		
		public float typingSpeed;
		private string line;
		public GameObject continueObject;

		IEnumerator TextAnimation()
		{
			foreach (char letter in line)
			{
				textDisplay.text += letter;
				letterSource.Play();
				yield return new WaitForSeconds(typingSpeed);
				letterSource.Stop();
			}
		}

		private void Update()
		{
			if (textDisplay.text == line)
			{
				continueObject.SetActive(true);
			}
		}

		public void NextLine(string sentence, string speakerName, Sprite speakerSprite, bool left)
		{
			dialogCanvas.alpha = 0;
			nextLineSource.Play();
			LeanTween.alphaCanvas(dialogCanvas, 1, 0.2f).setEaseOutQuad();
			continueObject.SetActive(false);
			
			if (left)
			{
				speakerNameRightCanvasGroup.alpha = 0;
				TweenUtility.FastFadeIn(speakerNameLeftCanvasGroup);
				speakerImageRightCanvasGroup.alpha = 0;
				TweenUtility.FastFadeIn(speakerImageLeftCanvasGroup);
				speakerImageLeft.sprite = speakerSprite;
				this.speakerNameLeft.text = speakerName;
			}
			else
			{
				speakerNameLeftCanvasGroup.alpha = 0;
				TweenUtility.FastFadeIn(speakerNameRightCanvasGroup);
				speakerImageLeftCanvasGroup.alpha = 0;
				TweenUtility.FastFadeIn(speakerImageRightCanvasGroup);
				speakerImageRight.sprite = speakerSprite;
				this.speakerNameRight.text = speakerName;
			}

			this.line = sentence;
			textDisplay.text = "";
			StartCoroutine(TextAnimation());
		}
	}
}