using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStartZone : MonoBehaviour {

    // Use this for initialization
    List<Vector2> startZone;
    int gridheight = 8;
    int gridwidth = 8;
	void Start () {
        startZone = new List<Vector2>();
        for(int x = 0; x < gridwidth; x++)
        {
            for (int y = 0; y < gridheight; y++)
            {
                startZone.Add(new Vector3(x, y));
            }
        }
        GetComponent<GenerateQuads>().GenerateMesh(startZone);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
