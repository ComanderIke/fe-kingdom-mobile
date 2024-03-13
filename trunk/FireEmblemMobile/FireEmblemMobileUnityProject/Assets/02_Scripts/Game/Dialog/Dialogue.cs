using System.Runtime.InteropServices;
using Game.GUI;
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
		public UIDialogFaceController speakerImageLeft;
		public UIDialogFaceController speakerImageRight;
		public TextMeshProUGUI textDisplay;
		public CanvasGroup dialogCanvas;
		public AudioSource letterSource;
		public AudioSource nextLineSource;
		
		//public float typingSpeed;
		private string line;
		public GameObject continueObject;
		private Coroutine textCoroutine;

		

		private void Update()
		{
			if (textDisplay.text == line)
			{
				continueObject.SetActive(true);
			}
		}

		public void NextLine(string sentence, string speakerName, bool left)
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
				//speakerImageLeft.sprite = speakerSprite;
				speakerImageLeft.Init(currentLine.Actor.dialogComponent.DialogueSpriteSet);
				
			}
			else
			{
				speakerImageLeftCanvasGroup.alpha = 0;
				TweenUtility.FastFadeIn(speakerImageRightCanvasGroup);
				speakerImageRight.Init(currentLine.Actor.dialogComponent.DialogueSpriteSet);
				//speakerImageRight.sprite = speakerSprite;
			}

			this.line = sentence;
			textDisplay.text = ""+line;
			// if(textCoroutine!=null)
			// 	StopCoroutine(textCoroutine);
			//textCoroutine = StartCoroutine(TextAnimation());
		}

		private Conversation conversation;
		private int currentLineIndex = 0;
		private Line currentLine;
		public void Show(Conversation conversation)
		{
			this.conversation = conversation;
			currentLineIndex = 0;
			currentLine = this.conversation.lines[currentLineIndex];
			NextLine(currentLine.sentence, currentLine.Actor.Name, currentLine.left);
			
			gameObject.SetActive(true);
			mainCanvasGroup.alpha = 0;
			TweenUtility.FadeIn(mainCanvasGroup);
		}
	}
}