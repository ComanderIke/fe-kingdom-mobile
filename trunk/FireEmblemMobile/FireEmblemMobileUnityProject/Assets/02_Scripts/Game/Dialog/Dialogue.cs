using Game.GUI.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Dialog
{
	
	[System.Serializable]
	public class Dialogue : MonoBehaviour
	{
		[SerializeField] CanvasGroup mainCanvasGroup;
		[SerializeField] CanvasGroup speakerNameCanvasGroup;
		[SerializeField] CanvasGroup speakerImageLeftCanvasGroup;
		[SerializeField] CanvasGroup speakerImageRightCanvasGroup;
		public TextMeshProUGUI speakerName;
		public Image speakerImageLeft;
		public Image speakerImageRight;
		public TextMeshProUGUI textDisplay;
		public CanvasGroup dialogCanvas;
		public AudioSource letterSource;
		public AudioSource nextLineSource;
		
		//public float typingSpeed;
		private string line;
		public GameObject continueObject;
		private Coroutine textCoroutine;

		
		// IEnumerator TextAnimation()
		// {
		// 	foreach (char letter in line)
		// 	{
		// 		textDisplay.text += letter;
		// 		letterSource.Play();
		// 		yield return new WaitForSeconds(typingSpeed);
		// 		letterSource.Stop();
		// 	}
		// }

		private void Update()
		{
			if (textDisplay.text == line)
			{
				continueObject.SetActive(true);
			}
		}

		public void NextLine(string sentence, string speakerName, Sprite speakerSprite, bool left)
		{
			//dialogCanvas.alpha = 0;
			nextLineSource.Play();
			LeanTween.alphaCanvas(dialogCanvas, 1, 0.2f).setEaseOutQuad();
			continueObject.SetActive(false);
			this.speakerName.text = speakerName;
			if (left)
			{
				speakerImageRightCanvasGroup.alpha = 0;
				TweenUtility.FastFadeIn(speakerImageLeftCanvasGroup);
				speakerImageLeft.sprite = speakerSprite;
				
			}
			else
			{
				speakerImageLeftCanvasGroup.alpha = 0;
				TweenUtility.FastFadeIn(speakerImageRightCanvasGroup);
				speakerImageRight.sprite = speakerSprite;
			}

			this.line = sentence;
			textDisplay.text = ""+line;
			// if(textCoroutine!=null)
			// 	StopCoroutine(textCoroutine);
			//textCoroutine = StartCoroutine(TextAnimation());
		}

		
		public void Show()
		{
			gameObject.SetActive(true);
			mainCanvasGroup.alpha = 0;
			TweenUtility.FadeIn(mainCanvasGroup);
		}
	}
}