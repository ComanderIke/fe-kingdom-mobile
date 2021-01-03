using UnityEngine;

namespace LeanTween.Examples.Scripts
{
	public class GeneralUISpace : MonoBehaviour {

		public RectTransform mainWindow;
		public RectTransform mainParagraphText;
		public RectTransform mainTitleText;
		public RectTransform mainButton1;
		public RectTransform mainButton2;

		public RectTransform pauseRing1;
		public RectTransform pauseRing2;
		public RectTransform pauseWindow;

		public RectTransform chatWindow;
		public RectTransform chatRect;
		public Sprite[] chatSprites;
		public RectTransform chatBar1;
		public RectTransform chatBar2;
		public UnityEngine.UI.Text chatText;

		public RectTransform rawImageRect;

		void Start () {
			// Time.timeScale = 1f/4f;
		
			// *********** Main Window **********
			// Scale the whole window in
			mainWindow.localScale = Vector3.zero;
			Framework.LeanTween.scale( mainWindow, new Vector3(1f,1f,1f), 0.6f).setEase(Framework.LeanTweenType.easeOutBack);
			Framework.LeanTween.alphaCanvas( mainWindow.GetComponent<CanvasGroup>(), 0f, 1f).setDelay(2f).setLoopPingPong().setRepeat(2);

			// Fade the main paragraph in while moving upwards
			mainParagraphText.anchoredPosition3D += new Vector3(0f,-10f,0f);
			Framework.LeanTween.textAlpha( mainParagraphText, 0f, 0.6f).setFrom(0f).setDelay(0f);
			Framework.LeanTween.textAlpha( mainParagraphText, 1f, 0.6f).setEase(Framework.LeanTweenType.easeOutQuad).setDelay(0.6f);
			Framework.LeanTween.move( mainParagraphText, mainParagraphText.anchoredPosition3D + new Vector3(0f,10f,0f), 0.6f).setEase(Framework.LeanTweenType.easeOutQuad).setDelay(0.6f);

			// Flash text to purple and back
			Framework.LeanTween.textColor( mainTitleText, new Color(133f/255f,145f/255f,223f/255f), 0.6f).setEase(Framework.LeanTweenType.easeOutQuad).setDelay(0.6f).setLoopPingPong().setRepeat(-1);

			// Fade button in
			Framework.LeanTween.textAlpha(mainButton2, 1f, 2f ).setFrom(0f).setDelay(0f).setEase(Framework.LeanTweenType.easeOutQuad);
			Framework.LeanTween.alpha(mainButton2, 1f, 2f ).setFrom(0f).setDelay(0f).setEase(Framework.LeanTweenType.easeOutQuad);

			// Pop size of button
			Framework.LeanTween.size(mainButton1, mainButton1.sizeDelta * 1.1f, 0.5f).setDelay(3f).setEaseInOutCirc().setRepeat(6).setLoopPingPong();


			// *********** Pause Button **********
			// Drop pause button in
			pauseWindow.anchoredPosition3D += new Vector3(0f,200f,0f);
			Framework.LeanTween.moveY( pauseWindow, pauseWindow.anchoredPosition3D.y + -200f, 0.6f).setEase(Framework.LeanTweenType.easeOutSine).setDelay(0.6f);

			// Punch Pause Symbol
			RectTransform pauseText = pauseWindow.Find("PauseText").GetComponent<RectTransform>();
			Framework.LeanTween.moveZ( pauseText, pauseText.anchoredPosition3D.z - 80f, 1.5f).setEase(Framework.LeanTweenType.punch).setDelay(2.0f);

			// Rotate rings around in opposite directions
			Framework.LeanTween.rotateAroundLocal(pauseRing1, Vector3.forward, 360f, 12f).setRepeat(-1);
			Framework.LeanTween.rotateAroundLocal(pauseRing2, Vector3.forward, -360f, 22f).setRepeat(-1);
		

			// *********** Chat Window **********
			// Flip the chat window in
			chatWindow.RotateAround(chatWindow.position, Vector3.up, -180f);
			Framework.LeanTween.rotateAround(chatWindow, Vector3.up, 180f, 2f).setEase(Framework.LeanTweenType.easeOutElastic).setDelay(1.2f);

			// Play a series of sprites on the window on repeat endlessly
			Framework.LeanTween.play(chatRect, chatSprites).setLoopPingPong();

			// Animate the bar up and down while changing the color to red-ish
			Framework.LeanTween.color( chatBar2, new Color(248f/255f,67f/255f,108f/255f, 0.5f), 1.2f).setEase(Framework.LeanTweenType.easeInQuad).setLoopPingPong().setDelay(1.2f);
			Framework.LeanTween.scale( chatBar2, new Vector2(1f,0.7f), 1.2f).setEase(Framework.LeanTweenType.easeInQuad).setLoopPingPong();

			// Write in paragraph text
			string origText = chatText.text;
			chatText.text = "";
			Framework.LeanTween.value(gameObject, 0, (float)origText.Length, 6f).setEase(Framework.LeanTweenType.easeOutQuad).setOnUpdate( (float val)=>{
				chatText.text = origText.Substring( 0, Mathf.RoundToInt( val ) );
			}).setLoopClamp().setDelay(2.0f);

			// Raw Image
			Framework.LeanTween.alpha(rawImageRect,0f,1f).setLoopPingPong();
		}

	}
}
