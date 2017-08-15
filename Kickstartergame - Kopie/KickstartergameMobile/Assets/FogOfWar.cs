using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {

    int width = 32;
    int height = 32;
    public GameObject[] lightSource;
	// Use this for initialization
	void Start () {
        Texture2D texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, Color.black);
            }
        }
        
        
        foreach(GameObject light in lightSource)
        {
            int x =(int) light.transform.position.x;
            int y =(int) light.transform.position.z;
            //texture.SetPixel(x, y, new Color(1,1,1,0));

        }
        texture.SetPixel(0, 0, new Color(1, 1, 1, 0));
        texture.Apply();
        GetComponent<MeshRenderer>().material.mainTexture = texture;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
