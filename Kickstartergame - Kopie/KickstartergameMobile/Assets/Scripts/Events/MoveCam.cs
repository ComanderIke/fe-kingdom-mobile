using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class MoveCam : MonoBehaviour
	{
		Vector3 targetPos;
		Vector3 startPos;
		float camspeed = 3f;
		const float MOVE_TIME=2.0f;
		bool start=false;
		float currentTime=0;
		void Start(){
			MainScript.moveCamEvent += Move;
		}
		void Move(Vector3 pos){
			start = true;
			targetPos = pos;
			currentTime = 0;
			startPos = GameObject.Find ("CameraRotation2").transform.position;

		}
		void Update(){
			if (!start)
				return;
			//transform.Translate (direction.normalized * camspeed * Time.deltaTime, Space.Self);
			currentTime +=Time.deltaTime;
			float timevalue = currentTime / MOVE_TIME;
			float lerpedx = Mathf.Lerp (startPos.x, targetPos.x, timevalue);
			//float lerpedy = Mathf.Lerp (startPos.y, targetPos.y, timevalue);
			float lerpedz = Mathf.Lerp (startPos.z, targetPos.z, timevalue);
			GameObject.Find ("CameraRotation2").transform.position = new Vector3 (lerpedx,startPos.y, lerpedz);
			if (currentTime >= MOVE_TIME) {
				if(MainScript.endOfMoveCam!=null)
				MainScript.endOfMoveCam ();
				start = false;
			}
		}
	}
}

