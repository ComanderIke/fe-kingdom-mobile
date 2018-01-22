using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _CamMovement : MonoBehaviour 
{
	public float speed = 1;
	public bool maximumX;
	public bool minimumX;

	// Use this for initialization
	void Start () 
    {

	}

	
	// Update is called once per frame
	void Update () 
    {



		if(maximumX == false)
		{

			if(Input.GetKeyDown(KeyCode.D))
			{

				transform.Translate(Vector3.right * speed);
			}

		}

		if(minimumX == false)
		{
			//Debug.Log ("");
			if(Input.GetKeyDown(KeyCode.A))
			{

				transform.Translate(Vector3.left * speed);
			}
		}
	}

	void OnTriggerEnter(Collider c)
	{
		if(c.CompareTag("maxX"))
		{
			Debug.Log("im hit");
			maximumX = true;
		}


		if(c.CompareTag("minX"))
		{
			minimumX = true;
		}
	}

	void OnTriggerExit(Collider c)
	{
		maximumX = false;
		minimumX = false;
	}
}
