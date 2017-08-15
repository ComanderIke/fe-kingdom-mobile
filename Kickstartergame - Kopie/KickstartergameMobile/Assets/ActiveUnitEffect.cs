using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUnitEffect : MonoBehaviour {

    Material m;
    bool hovered = false;
    float time = 0;
    public Texture texture;
    public Texture hoveredTexture;
	// Use this for initialization
	void Start () {
        m = GetComponent<MeshRenderer>().material;
	}
	public void SetHovered(bool hovered)
    {
        this.hovered = hovered;
        if (hovered)
            m.mainTexture = hoveredTexture;
        else
            m.mainTexture = texture;
    }
	// Update is called once per frame
	void Update () {
        m.color = new Color(1, 1, 1, Mathf.PingPong(Time.time * 1.0f, 1f));
	}
}
