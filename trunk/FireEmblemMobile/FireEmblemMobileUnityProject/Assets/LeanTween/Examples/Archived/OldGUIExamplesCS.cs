using UnityEngine;

namespace LeanTween.Examples.Archived
{
	public class OldGUIExamplesCS : MonoBehaviour {
		public Texture2D grumpy;
		public Texture2D beauty;

		private float w;
		private float h;

		private Framework.LTRect buttonRect1;
		private Framework.LTRect buttonRect2;
		private Framework.LTRect buttonRect3;
		private Framework.LTRect buttonRect4;
		private Framework.LTRect grumpyRect;
		private Framework.LTRect beautyTileRect;


		// Use this for initialization
		void Start () {
			w = Screen.width;
			h = Screen.height;
			buttonRect1 = new Framework.LTRect(0.10f*w, 0.8f*h, 0.2f*w, 0.14f*h );
			buttonRect2 = new Framework.LTRect(1.2f*w, 0.8f*h, 0.2f*w, 0.14f*h );
			buttonRect3 = new Framework.LTRect(0.35f*w, 0.0f*h, 0.3f*w, 0.2f*h, 0f );
			buttonRect4 = new Framework.LTRect(0.0f*w, 0.4f*h, 0.3f*w, 0.2f*h, 1.0f, 15.0f );
		
			grumpyRect = new Framework.LTRect(0.5f*w - grumpy.width*0.5f, 0.5f*h - grumpy.height*0.5f, grumpy.width, grumpy.height );
			beautyTileRect = new Framework.LTRect(0.0f,0.0f,1.0f,1.0f );

			Framework.LeanTween.move( buttonRect2, new Vector2(0.55f*w, buttonRect2.rect.y), 0.7f ).setEase(Framework.LeanTweenType.easeOutQuad);
		}

		public void catMoved(){
			Debug.Log("cat moved...");
		}
	
		// Update is called once per frame
		void OnGUI () {
			GUI.DrawTexture( grumpyRect.rect, grumpy);

			Rect staticRect = new Rect(0.0f*w, 0.0f*h, 0.2f*w, 0.14f*h);
			if(GUI.Button( staticRect, "Move Cat")){
				if(Framework.LeanTween.isTweening(grumpyRect)==false){ // Check to see if the cat is already tweening, so it doesn't freak out
					Vector2 orig = new Vector2( grumpyRect.rect.x, grumpyRect.rect.y );
					Framework.LeanTween.move( grumpyRect, new Vector2( 1.0f*Screen.width - grumpy.width, 0.0f*Screen.height ), 1.0f).setEase(Framework.LeanTweenType.easeOutBounce).setOnComplete(catMoved);
					Framework.LeanTween.move( grumpyRect, orig, 1.0f ).setDelay(1.0f).setEase( Framework.LeanTweenType.easeOutBounce);
				}
			}

			if(GUI.Button(buttonRect1.rect, "Scale Centered")){
				Framework.LeanTween.scale( buttonRect1, new Vector2(buttonRect1.rect.width, buttonRect1.rect.height) * 1.2f, 0.25f ).setEase( Framework.LeanTweenType.easeOutQuad );
				Framework.LeanTween.move( buttonRect1, new Vector2(buttonRect1.rect.x-buttonRect1.rect.width*0.1f, buttonRect1.rect.y-buttonRect1.rect.height*0.1f), 0.25f ).setEase(Framework.LeanTweenType.easeOutQuad);
			}

			if(GUI.Button(buttonRect2.rect, "Scale")){
				Framework.LeanTween.scale( buttonRect2, new Vector2(buttonRect2.rect.width, buttonRect2.rect.height) * 1.2f, 0.25f ).setEase(Framework.LeanTweenType.easeOutBounce);
			}

			staticRect = new Rect(0.76f*w, 0.53f*h, 0.2f*w, 0.14f*h);
			if(GUI.Button( staticRect, "Flip Tile")){
				Framework.LeanTween.move( beautyTileRect, new Vector2( 0f, beautyTileRect.rect.y + 1.0f ), 1.0f ).setEase(Framework.LeanTweenType.easeOutBounce);
			}

			GUI.DrawTextureWithTexCoords( new Rect(0.8f*w, 0.5f*h - beauty.height*0.5f, beauty.width*0.5f, beauty.height*0.5f), beauty, beautyTileRect.rect);


			if(GUI.Button(buttonRect3.rect, "Alpha")){
				Framework.LeanTween.alpha( buttonRect3, 0.0f, 1.0f).setEase(Framework.LeanTweenType.easeOutQuad);
				Framework.LeanTween.alpha( buttonRect3, 1.0f, 1.0f).setDelay(1.0f).setEase( Framework.LeanTweenType.easeInQuad);

				Framework.LeanTween.alpha( grumpyRect, 0.0f, 1.0f).setEase(Framework.LeanTweenType.easeOutQuad);
				Framework.LeanTween.alpha( grumpyRect, 1.0f, 1.0f).setDelay(1.0f).setEase(Framework.LeanTweenType.easeInQuad);
			}
			GUI.color = new Color(1.0f,1.0f,1.0f,1.0f); // Reset to normal alpha, otherwise other gui elements will be effected

			if(GUI.Button(buttonRect4.rect, "Rotate")){
				Framework.LeanTween.rotate( buttonRect4, 150.0f, 1.0f ).setEase(Framework.LeanTweenType.easeOutElastic);
				Framework.LeanTween.rotate( buttonRect4, 0.0f, 1.0f ).setDelay(1.0f).setEase(Framework.LeanTweenType.easeOutElastic);
			}
			GUI.matrix = Matrix4x4.identity;
		}
	}
}
