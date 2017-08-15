using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fpscounter : MonoBehaviour {

		float deltaTime = 0.0f;

		void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			string text =  fps+" FPS";
			this.GetComponent<Text>().text = text;
		}
		
			
}