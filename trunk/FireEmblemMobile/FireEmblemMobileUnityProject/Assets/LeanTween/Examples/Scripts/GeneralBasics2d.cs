using UnityEngine;

namespace LeanTween.Examples.Scripts
{
	public class GeneralBasics2d : MonoBehaviour {

		public Texture2D dudeTexture;
		public GameObject prefabParticles;

#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)

		void Start () {
			// Setup
			GameObject avatarRotate = createSpriteDude( "avatarRotate", new Vector3(-2.51208f,10.7119f,-14.37754f));
			GameObject avatarScale = createSpriteDude( "avatarScale", new Vector3(2.51208f,10.2119f,-14.37754f));
			GameObject avatarMove = createSpriteDude( "avatarMove", new Vector3(-3.1208f,7.100643f,-14.37754f));
	
			// Rotate Example
			Framework.LeanTween.rotateAround( avatarRotate, Vector3.forward, -360f, 5f);

			// Scale Example
			Framework.LeanTween.scale( avatarScale, new Vector3(1.7f, 1.7f, 1.7f), 5f).setEase(Framework.LeanTweenType.easeOutBounce);
			Framework.LeanTween.moveX( avatarScale, avatarScale.transform.position.x + 1f, 5f).setEase(Framework.LeanTweenType.easeOutBounce); // Simultaneously target many different tweens on the same object 

			// Move Example
			Framework.LeanTween.move( avatarMove, avatarMove.transform.position + new Vector3(1.7f, 0f, 0f), 2f).setEase(Framework.LeanTweenType.easeInQuad);

			// Delay
			Framework.LeanTween.move( avatarMove, avatarMove.transform.position + new Vector3(2f, -1f, 0f), 2f).setDelay(3f);

			// Chain properties (delay, easing with a set repeating of type ping pong)
			Framework.LeanTween.scale( avatarScale, new Vector3(0.2f, 0.2f, 0.2f), 1f).setDelay(7f).setEase(Framework.LeanTweenType.easeInOutCirc).setLoopPingPong(3);

			// Call methods after a certain time period
			Framework.LeanTween.delayedCall(gameObject, 0.2f, advancedExamples);
		}
	
		GameObject createSpriteDude( string name, Vector3 pos, bool hasParticles = true ){
			GameObject go = new GameObject(name);
			SpriteRenderer ren = go.AddComponent<SpriteRenderer>();
			go.GetComponent<SpriteRenderer>().color = new Color(0f,181f/255f,1f);
			ren.sprite = Sprite.Create( dudeTexture, new Rect(0.0f,0.0f,256.0f,256.0f), new Vector2(0.5f,0f), 256f);
			go.transform.position = pos;

			if(hasParticles){
				GameObject particles = (GameObject)Instantiate(prefabParticles, Vector3.zero, prefabParticles.transform.rotation );
				particles.transform.parent = go.transform;
				particles.transform.localPosition = prefabParticles.transform.position;
			}
			return go;
		}

		// Advanced Examples
		// It might be best to master the basics first, but this is included to tease the many possibilies LeanTween provides.

		void advancedExamples(){
			Framework.LeanTween.delayedCall(gameObject, 14f, ()=>{
				for(int i=0; i < 10; i++){
					// Instantiate Container
					GameObject rotator = new GameObject("rotator"+i);
					rotator.transform.position = new Vector3(2.71208f,7.100643f,-12.37754f);

					// Instantiate Avatar
					GameObject dude = createSpriteDude( "dude"+i, new Vector3(-2.51208f,7.100643f,-14.37754f), false);//(GameObject)GameObject.Instantiate(prefabAvatar, Vector3.zero, prefabAvatar.transform.rotation );
					dude.transform.parent = rotator.transform;
					dude.transform.localPosition = new Vector3(0f,0.5f,0.5f*i);

					// Scale, pop-in
					dude.transform.localScale = new Vector3(0f,0f,0f);
					Framework.LeanTween.scale(dude, new Vector3(0.65f,0.65f,0.65f), 1f).setDelay(i*0.2f).setEase(Framework.LeanTweenType.easeOutBack);

					// Color like the rainbow
					float period = Framework.LeanTween.tau/10*i;
					float red   = Mathf.Sin(period + Framework.LeanTween.tau*0f/3f) * 0.5f + 0.5f;
					float green = Mathf.Sin(period + Framework.LeanTween.tau*1f/3f) * 0.5f + 0.5f;
					float blue  = Mathf.Sin(period + Framework.LeanTween.tau*2f/3f) * 0.5f + 0.5f;
					Color rainbowColor = new Color(red, green, blue);
					Framework.LeanTween.color(dude, rainbowColor, 0.3f).setDelay(1.2f + i*0.4f);
				
					// Push into the wheel
					Framework.LeanTween.moveLocalZ(dude, -2f, 0.3f).setDelay(1.2f + i*0.4f).setEase(Framework.LeanTweenType.easeSpring).setOnComplete(
						()=>{
							Framework.LeanTween.rotateAround(rotator, Vector3.forward, -1080f, 12f);
						}
					);

					// Jump Up and back down
					Framework.LeanTween.moveLocalY(dude,1.17f,1.2f).setDelay(5f + i*0.2f).setLoopPingPong(1).setEase(Framework.LeanTweenType.easeInOutQuad);
			
					// Alpha Out, and destroy
					Framework.LeanTween.alpha(dude, 0f, 0.6f).setDelay(9.2f + i*0.4f).setDestroyOnComplete(true).setOnComplete(
						()=>{
							Destroy( rotator ); // destroying parent as well
						}
					);	
				}

			}).setOnCompleteOnStart(true).setRepeat(-1); // Have the OnComplete play in the beginning and have the whole group repeat endlessly
		}

#endif
	}
}
