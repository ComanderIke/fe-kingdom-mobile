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
		public TextMeshProUGUI speakerName;
		public Image speakerImage;
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

		public void NextLine(string sentence, string speakerName, Sprite speakerSprite)
		{
			dialogCanvas.alpha = 0;
			nextLineSource.Play();
			LeanTween.alphaCanvas(dialogCanvas, 1, 0.2f).setEaseOutQuad();
			continueObject.SetActive(false);
			speakerImage.sprite = speakerSprite;
			this.speakerName.text = speakerName;
			this.line = sentence;
			textDisplay.text = "";
			StartCoroutine(TextAnimation());
		}
	}
}